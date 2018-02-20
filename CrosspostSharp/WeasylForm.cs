using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;
using DontPanic.TumblrSharp.Client;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.OAuth;
using System.Text;
using System.Collections.Generic;
using System.Diagnostics;
using InkbunnyLib;
using Tweetinvi.Models;
using Tweetinvi;
using System.Text.RegularExpressions;
using Tweetinvi.Parameters;
using ArtSourceWrapper;
using FlickrNet;
using System.Drawing.Imaging;

namespace CrosspostSharp {
	public partial class WeasylForm : Form {
		private static Settings GlobalSettings;

		public ISiteWrapper SourceWrapper { get; private set; }
        public int WrapperPosition { get; private set; }

		private TumblrClient Tumblr;
		public string TumblrUsername { get; private set; }

		private ITwitterCredentials TwitterCredentials;
		private int shortURLLength;
		private int shortURLLengthHttps;
		private List<ITweet> tweetCache;

        private Flickr Flickr;
        private FlickrNet.Auth FlickrAuth;

        private InkbunnyClient Inkbunny;

        // Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
        private WeasylThumbnail[] thumbnails;

        private TabControlHelper _helper;

        // The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
        private ISubmissionWrapper currentSubmission;
		private BinaryFile currentImage;

		// The image displayed in the main panel. This is used again if WeasylSync needs to add padding to the image to force a square aspect ratio.
		private Bitmap currentImageBitmap;

		// Allows WeasylThumbnail access to the progress bar.
        public LProgressBar LProgressBar => lProgressBar1;

        public WeasylForm(ISiteWrapper initialWrapper = null) {
			InitializeComponent();
			tweetCache = new List<ITweet>();

			GlobalSettings = Settings.Load();

			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3 };
            _helper = new TabControlHelper(tabControl1);

            txtSaveDir.Text = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
            this.Shown += (o, e) => LoadFromSettings(initialWrapper);
		}

        private void ShowException(Exception e, string source) {
            MessageBox.Show(this, e.Message, $"Error in {source}: {e.GetType()}");
        }

        private void ShowProgressBar() {
            LProgressBar.Visible = true;
            tabControl1.Enabled = false;
        }

        private void HideProgressBar() {
            LProgressBar.Visible = false;
            tabControl1.Enabled = true;
        }

        public void ShowSetDescForm() {
            using (var f = new SetDescForm()) {
                f.SubmissionWrapper = this.currentSubmission;
                if (f.ShowDialog(this) == DialogResult.OK) {
                    SetCurrentImage(f.SubmissionWrapper, currentImage);
                }
            }
        }

        #region GUI updates
        private async Task DeviantArtLogin() {
            lblDeviantArtStatus2.Text = "";

            if (!string.IsNullOrEmpty(GlobalSettings.DeviantArt.RefreshToken)) {
                try {
                    string oldToken = GlobalSettings.DeviantArt.RefreshToken;
                    string newToken = await DeviantArtLoginStatic.UpdateTokens(
                        OAuthConsumer.DeviantArt.CLIENT_ID,
                        OAuthConsumer.DeviantArt.CLIENT_SECRET,
                        oldToken);
                    if (oldToken != newToken) {
                        GlobalSettings.DeviantArt.RefreshToken = newToken;
                        GlobalSettings.Save();
                    }
                    lblDeviantArtStatus2.Text = (await DeviantArtDeviationWrapper.GetUserAsync()).Username;
                    return;
                } catch (DeviantArtException e) when (e.Message == "User canceled") {
                    GlobalSettings.DeviantArt.RefreshToken = null;
                } catch (DeviantArtException e) when (e.Message == "The refresh_token is invalid.") {
                    GlobalSettings.DeviantArt.RefreshToken = null;
                } catch (DeviantArtException e) {
                    ShowException(e, nameof(DeviantArtLogin));
                }
            }
        }

        private async Task GetNewWrapper(ISiteWrapper initialWrapper = null, Type preferredWrapperType = null) {
            List<ISiteWrapper> wrappers = new List<ISiteWrapper>();

            if (initialWrapper != null) {
                wrappers.Add(initialWrapper);
            } else {
                if (GlobalSettings.DeviantArt.RefreshToken != null) {
                    try {
                        wrappers.Add(new DeviantArtWrapper(new DeviantArtGalleryDeviationWrapper()));
                        wrappers.Add(new StashOrderedWrapper());
                    } catch (Exception e) {
                        ShowException(e, nameof(GetNewWrapper));
                    }
                }

                if (!string.IsNullOrEmpty(GlobalSettings.FurAffinity.a) && !string.IsNullOrEmpty(GlobalSettings.FurAffinity.b)) {
                    wrappers.Add(new FurAffinityWrapper(new FurAffinityIdWrapper(GlobalSettings.FurAffinity.a, GlobalSettings.FurAffinity.b, scraps: true)));
                    wrappers.Add(new FurAffinityWrapper(new FurAffinityIdWrapper(GlobalSettings.FurAffinity.a, GlobalSettings.FurAffinity.b, scraps: false)));
                }

                if (!string.IsNullOrEmpty(GlobalSettings.Weasyl.APIKey)) {
                    wrappers.Add(new WeasylWrapper(new WeasylGalleryIdWrapper(GlobalSettings.Weasyl.APIKey)));
                    wrappers.Add(new WeasylWrapper(new WeasylCharacterWrapper(GlobalSettings.Weasyl.APIKey)));
                }

                if (Inkbunny != null) {
                    wrappers.Add(new InkbunnyWrapper(Inkbunny));
                }

                if (TwitterCredentials != null) {
                    wrappers.Add(new TwitterWrapper(TwitterCredentials));
                }

                if (Tumblr != null) {
                    wrappers.Add(new TumblrWrapper(Tumblr, GlobalSettings.Tumblr.BlogName));
                }

                if (Flickr != null) {
                    wrappers.Add(new FlickrWrapper(Flickr));
				}

				if (!string.IsNullOrEmpty(GlobalSettings.Pixiv.Username) && !string.IsNullOrEmpty(GlobalSettings.Pixiv.Password)) {
					wrappers.Add(new PixivWrapper(GlobalSettings.Pixiv.Username, GlobalSettings.Pixiv.Password));
				}

				wrappers = wrappers.OrderBy(w => w.WrapperName).ToList();

                wrappers.Add(new UserChosenLocalFolderWrapper { Parent = this });
            }

            if (preferredWrapperType != null) {
                wrappers = wrappers.Where(w => w.GetType().Equals(preferredWrapperType)).ToList();
                if (!wrappers.Any()) return;
            }

            if (wrappers.Count == 1) {
                SourceWrapper = wrappers.Single();
            } else {
                var form = new SourceChoiceForm(wrappers) {
                    SelectedWrapper = SourceWrapper ?? wrappers.First()
                };
                var dialogResult = form.ShowDialog(this);
                SourceWrapper = dialogResult == DialogResult.OK
                    ? form.SelectedWrapper
                    : new EmptyWrapper();
            }
            WrapperPosition = 0;

            picUserIcon.Image = null;
            lblWeasylStatus1.Text = SourceWrapper.SiteName ?? "";

            string user = null;
            try {
                user = await SourceWrapper.WhoamiAsync();
            } catch (Exception e) {
                ShowException(e, nameof(GetNewWrapper));
            }
            lblWeasylStatus2.Text = user ?? "";

            try {
                string picUrl = await SourceWrapper.GetUserIconAsync(picUserIcon.Height);
                if (picUrl != null) {
                    var req = CreateWebRequest(picUrl);
                    using (WebResponse resp = await req.GetResponseAsync())
                    using (var stream1 = resp.GetResponseStream())
                    using (var stream2 = new MemoryStream()) {
                        await stream1.CopyToAsync(stream2);
                        stream2.Position = 0;
                        Image image = Image.FromStream(stream2);
                        picUserIcon.Image = image;
                        if (image.Height > picUserIcon.Height && image.Height <= picUserIcon.Height + 4) {
                            picUserIcon.SizeMode = PictureBoxSizeMode.CenterImage;
                        } else {
                            picUserIcon.SizeMode = PictureBoxSizeMode.StretchImage;
                        }
                    }
                }
            } catch (Exception) { }
        }

        private async Task GetNewTumblrClient() {
            Token token = GlobalSettings.TumblrToken;
            if (token != null && token.IsValid) {
                if (Tumblr != null) Tumblr.Dispose();
                Tumblr = new TumblrClientFactory().Create<TumblrClient>(
                    OAuthConsumer.Tumblr.CONSUMER_KEY,
                    OAuthConsumer.Tumblr.CONSUMER_SECRET,
                    token);
            }

            lblTumblrStatus2.Text = "";
            if (Tumblr != null) {
                try {
                    TumblrUsername = (await Tumblr.GetUserInfoAsync()).Name;
                    lblTumblrStatus2.Text = TumblrUsername ?? "";
                } catch (Exception e) {
                    TumblrUsername = null;
                    ShowException(e, nameof(GetNewTumblrClient));
                }
            }
        }

        private async Task GetNewInkbunnyClient() {
            if (GlobalSettings.Inkbunny.Sid != null && GlobalSettings.Inkbunny.UserId != null) {
                Inkbunny = new InkbunnyClient(GlobalSettings.Inkbunny.Sid, GlobalSettings.Inkbunny.UserId.Value);
            }

            lblInkbunnyStatus2.Text = "";
            if (Inkbunny != null) {
                try {
                    lblInkbunnyStatus2.Text = await Inkbunny.GetUsernameAsync();
                } catch (Exception e) {
                    Inkbunny = null;
                    ShowException(e, nameof(GetNewInkbunnyClient));
                }
            }
        }

        private async Task GetNewTwitterClient() {
            TwitterCredentials = GlobalSettings.TwitterCredentials;
            try {
                string screenName = TwitterCredentials?.GetScreenName();
                lblTwitterStatus2.Text = screenName ?? "";
                if (TwitterCredentials != null && screenName == null) {
                    MessageBox.Show(this, "Twitter credentials are no longer valid - please log in again.");
                }

                if (screenName != null) {
                    Tweetinvi.Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
                        if (shortURLLength == 0 || shortURLLengthHttps == 0) {
                            var conf = Tweetinvi.Help.GetTwitterConfiguration();
                            shortURLLength = conf.ShortURLLength;
                            shortURLLengthHttps = conf.ShortURLLengthHttps;
                        }
                    });

                    if (!tweetCache.Any()) {
                        tweetCache.AddRange(await GetMoreOldTweets());
                    }
                }
            } catch (Exception e) {
                TwitterCredentials = null;
                statusStrip1.Text = e.Message;
            }
        }

        private async Task GetNewFlickrClient() {
            Flickr = null;
            FlickrAuth = null;

            if (GlobalSettings.Flickr?.TokenKey != null && GlobalSettings.Flickr?.TokenSecret != null) {
                Flickr = new Flickr(OAuthConsumer.Flickr.KEY, OAuthConsumer.Flickr.SECRET) {
                    OAuthAccessToken = GlobalSettings.Flickr.TokenKey,
                    OAuthAccessTokenSecret = GlobalSettings.Flickr.TokenSecret
                };
            }

            lblFlickrStatus2.Text = "";
            if (Flickr != null) {
                try {
                    var t2 = new TaskCompletionSource<FlickrNet.Auth>();
                    Flickr.AuthOAuthCheckTokenAsync(a => {
                        if (a.HasError) {
                            t2.SetException(a.Error);
                        } else {
                            t2.SetResult(a.Result);
                        }
                    });
                    FlickrAuth = await t2.Task;
                    lblFlickrStatus2.Text = FlickrAuth.User.UserName;

                    await PopulateFlickrLicenses();
                } catch (Exception e) {
                    Flickr = null;
                    FlickrAuth = null;
                    ShowException(e, nameof(GetNewFlickrClient));
                }
            }
        }

        private async Task PopulateFlickrLicenses() {
            var t = new TaskCompletionSource<LicenseCollection>();
            Flickr.PhotosLicensesGetInfoAsync(result => {
                if (result.HasError) {
                    t.SetException(result.Error);
                } else {
                    t.SetResult(result.Result);
                }
            });
            var licenses = await t.Task;

            ddlFlickrLicense.Items.Clear();
            ddlFlickrLicense.DisplayMember = nameof(License.LicenseName);
            ddlFlickrLicense.Items.AddRange(licenses.Where(l => (int)l.LicenseId != 7).ToArray());
        }

        private async void LoadFromSettings(ISiteWrapper initialWrapper = null) {
			try {
                LProgressBar.Report(0);
				ShowProgressBar();

                var tasks = new Task[] {
                    GetNewTumblrClient(),
                    GetNewInkbunnyClient(),
                    GetNewTwitterClient(),
                    GetNewFlickrClient(),
                    DeviantArtLogin()
                };

                _helper.SetPageVisible(tabTumblr, Tumblr != null);
                _helper.SetPageVisible(tabInkbunny, Inkbunny != null);
                _helper.SetPageVisible(tabTwitter, TwitterCredentials != null);
                _helper.SetPageVisible(tabFlickr, Flickr != null);
                _helper.SetPageVisible(tabDeviantArt, GlobalSettings.DeviantArt.RefreshToken != null);

                int progress = 0;
                foreach (var t in tasks) {
                    var _ = t.ContinueWith(x => LProgressBar.Report(++progress / (tasks.Length + 1.0)));
                }

                await Task.WhenAll(tasks);

                await GetNewWrapper(initialWrapper);

				HideProgressBar();
                
				txtFooter.Text = GlobalSettings.Defaults.FooterHTML ?? "";

				// Global tags that you can include in each Tumblr submission if you want.
				txtTags2.Text = GlobalSettings.Defaults.Tags ?? "";

                UpdateGalleryAsync();
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
                ShowException(e, nameof(LoadFromSettings));
            }
		}

		// This function is called after clicking on a WeasylThumbnail.
		public void SetCurrentImage(ISubmissionWrapper submission, BinaryFile file) {
			this.currentSubmission = submission;
			tabControl1.Enabled = submission?.OwnWork == true;

            var tags = new List<string>();

			if (submission != null) {
                txtHeader.Text = string.IsNullOrEmpty(submission.Title)
                    ? ""
                    : GlobalSettings.Defaults.HeaderHTML?.Replace("{TITLE}", submission.Title) ?? "";
                txtInkbunnyTitle.Text = txtFlickrTitle.Text = submission.Title;
				txtDescription.Text = submission.HTMLDescription;
                txtFlickrDesc.Text = Regex.Replace(submission.HTMLDescription ?? "", @"<br ?\/?>\r?\n?", Environment.NewLine);
                string bbCode = HtmlToBBCode.ConvertHtml(txtFlickrDesc.Text);
				txtInkbunnyDescription.Text = bbCode;
				txtURL.Text = submission.ViewURL ?? "";

				ResetTweetText();

				lnkTwitterLinkToInclude.Text = submission.ViewURL ?? "";
                if (submission.PotentiallySensitive) {
                    chkTweetPotentiallySensitive.Checked = true;
                    radFlickrRestricted.Checked = true;
                } else {
                    chkTweetPotentiallySensitive.Checked = false;
                    radFlickrSafe.Checked = true;
                }

                tags.AddRange(submission.Tags);
				txtTags1.Text = txtInkbunnyTags.Text = txtFlickrTags.Text = string.Join(" ", tags.Select(s => "#" + s));

                pickDate.Value = pickTime.Value = submission.Timestamp;
			}
			this.currentImage = file;
            this.lnkOriginalUrl.Text = submission?.ViewURL ?? "";
			if (file == null) {
				mainPictureBox.Image = null;
                txtSaveFilename.Text = "";
            } else {
                char[] invalid = Path.GetInvalidFileNameChars();
                string basename = submission?.Title;
                if (string.IsNullOrEmpty(basename)) {
                    basename = "image";
                }
                basename = new string(basename.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
                string ext = file.MimeType.StartsWith("image/")
                    ? $".{file.MimeType.Replace("image/", "")}"
                    : "";
                txtSaveFilename.Text = basename + ext;

                try {
					this.currentImageBitmap = (Bitmap)Image.FromStream(new MemoryStream(file.Data));
                    if (file.MimeType == "image/png" && this.currentImageBitmap.RawFormat.Guid == ImageFormat.Jpeg.Guid) {
                        MessageBox.Show(this, "This image has a .png extension, but it's in JPEG format. If you have a PNG version, you might want to post that instead.", submission?.Title, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    }

                    mainPictureBox.Image = this.currentImageBitmap;
                } catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
            }
            
            deviantArtUploadControl1.SetSubmission(
                data: file?.Data,
                title: submission?.Title,
                htmlDescription: submission?.HTMLDescription,
                tags: tags,
                mature: submission?.PotentiallySensitive == true,
                originalUrl: submission?.ViewURL);

            if (submission is LocalFileSubmissionWrapper) {
                ShowSetDescForm();
            }
        }

		private void ResetTweetText() {
			List<string> plainTextList = new List<string>(2);
			if (chkIncludeTitle.Checked) {
				plainTextList.Add(currentSubmission.Title);
			}
			if (chkIncludeDescription.Checked) {
				string bbCode = HtmlToBBCode.ConvertHtml(txtDescription.Text);
				plainTextList.Add(Regex.Replace(bbCode, @"\[\/?(b|i|u|q|url=?[^\]]*)\]", ""));
			}
			string plainText = string.Join("﹘", plainTextList.Where(s => s != ""));

			int maxLength = 280;
			if (chkIncludeLink.Checked) {
				maxLength -= (shortURLLengthHttps + 1);
			}

			txtTweetText.Text = plainText.Length > maxLength
				? $"{plainText.Substring(0, maxLength - 1)}…"
				: plainText;
		}

		private Task<IEnumerable<ITweet>> GetMoreOldTweets() {
			if (TwitterCredentials == null)
				return Task.FromResult(Enumerable.Empty<ITweet>());

			long sinceId = tweetCache.Select(t => t.Id).DefaultIfEmpty(20).Max();
			return Task.Run(() => Tweetinvi.Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
				var user = User.GetAuthenticatedUser();
				var parameters = new UserTimelineParameters {
					MaximumNumberOfTweetsToRetrieve = 200,
					TrimUser = true,
					SinceId = sinceId
				};

				TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;
				var tweets = Timeline.GetUserTimeline(user, parameters);
				if (tweets == null) {
					var x = Tweetinvi.ExceptionHandler.GetLastException();
					throw new Exception(x.TwitterDescription, x.WebException);
				}
				return tweets
					.Where(t => t.CreatedBy.Id == user.Id);
			}));
		}
        
		private async void UpdateGalleryAsync(bool back = false, bool next = false) {
			try {
                if (SourceWrapper == null) return;

                LProgressBar.Report(0);
                ShowProgressBar();

                int addedCount = this.thumbnails.Length;

                if (back) WrapperPosition -= addedCount;
                if (next) WrapperPosition += addedCount;
                if (WrapperPosition < 0) WrapperPosition = 0;

                int totalCount = WrapperPosition + addedCount;

                btnUp.Enabled = WrapperPosition > 0;
                btnDown.Enabled = true;
                
                while (true) {
                    int got = SourceWrapper.Cache.Count() - WrapperPosition;
                    int outOf = totalCount - WrapperPosition;
                    if (got >= outOf) break;

                    LProgressBar.Report((double)got / outOf);

                    int read = await SourceWrapper.FetchAsync();
                    if (read == -1) {
                        btnDown.Enabled = false;
                        break;
                    }
                }

                var slice = SourceWrapper.Cache.Skip(WrapperPosition).Take(addedCount).ToList();

                for (int i = 0; i < this.thumbnails.Length; i++) {
                    LProgressBar.Report((double)i / this.thumbnails.Length);
                    await this.thumbnails[i].SetSubmission(i < slice.Count
						? slice[i]
						: null);
				}
			} catch (Exception ex) {
                ShowException(ex, nameof(UpdateGalleryAsync));
            } finally {
                HideProgressBar();
            }
        }
		#endregion

        #region HTML compilation
        public string CompileHTML() {
			StringBuilder html = new StringBuilder();

			if (chkHeader.Checked) {
				html.Append(txtHeader.Text);
			}

			if (chkDescription.Checked) {
				html.Append(txtDescription.Text);
			}

			if (chkFooter.Checked) {
				html.Append(txtFooter.Text);
			}

            if (this.SourceWrapper is LocalPathWrapper) {
                string s = html.ToString();
                if (s.Contains("{URL}") || s.Contains("{{SITENAME}}")) {
                    throw new Exception("Cannot include {URL} or {SITENAME} when posting from local file.");
                }
            }

			html.Replace("{URL}", txtURL.Text)
                .Replace("{SITENAME}", SourceWrapper.SiteName);

			return html.ToString();
		}

		private static string HTML_PREVIEW = @"
<html>
	<head>
	<style type='text/css'>
		body {
			font-family: ""Helvetica Neue"",""HelveticaNeue"",Helvetica,Arial,sans-serif;
			font-weight: 400;
			line-height: 1.4;
			font-size: 14px;
			font-style: normal;
			color: #444;
		}
		p {
			margin: 0 0 10px;
			padding: 0px;
			border: 0px none;
			font: inherit;
			vertical-align: baseline;
		}
		a img {
			border: 0;
		}
	</style>
</head>
	<body>{HTML}</body>
</html>";
		#endregion

		#region Tumblr
		private void CreateTumblrClient_GetNewToken() {
			Token token = TumblrKey.Obtain(OAuthConsumer.Tumblr.CONSUMER_KEY, OAuthConsumer.Tumblr.CONSUMER_SECRET);
			if (token == null) {
				return;
			} else {
				GlobalSettings.TumblrToken = token;
				GlobalSettings.Save();
				Tumblr = new TumblrClientFactory().Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					token);
			}
		}

		private async void PostToTumblr() {
			try {
				if (this.currentImage == null) {
					MessageBox.Show("No image is selected.");
					return;
				}

				if (Tumblr == null) CreateTumblrClient_GetNewToken();
				if (Tumblr == null) {
					MessageBox.Show("Posting cancelled.");
					return;
				}

                LProgressBar.Report(0);
				ShowProgressBar();

				var tags = new List<string>();
				if (chkTags1.Checked) tags.AddRange(txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""));
				if (chkTags2.Checked) tags.AddRange(txtTags2.Text.Replace("#", "").Split(' ').Where(s => s != ""));

				BinaryFile imageToPost = GlobalSettings.Tumblr.AutoSidePadding && this.currentImageBitmap.Height > this.currentImageBitmap.Width
					? MakeSquare(this.currentImageBitmap)
					: currentImage;

				PostData post = PostData.CreatePhoto(new BinaryFile[] { imageToPost }, CompileHTML(), txtURL.Text, tags);
				post.Date = chkNow.Checked
					? (DateTimeOffset?)null
					: (pickDate.Value.Date + pickTime.Value.TimeOfDay);
                
				PostCreationInfo info = await Tumblr.CreatePostAsync(GlobalSettings.Tumblr.BlogName, post);

                LProgressBar.Report(0.5);
                try {
                    var newPost = await Tumblr.GetPostAsync(info.PostId);
                    lblPosted1.Visible = true;
                    lblPosted2.Visible = true;
                    lblPosted2.Text = newPost.Url;
                } catch (Exception e) when (e.Message == "Not Found") {
                    // Fine, let's just guess the url.
                    lblPosted1.Visible = true;
                    lblPosted2.Visible = true;
                    lblPosted2.Text = $"https://{GlobalSettings.Tumblr.BlogName}.tumblr.com/post/{info.PostId}";
                }
            } catch (Exception e) {
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				var messages = (e as AggregateException)?.InnerExceptions?.Select(x => x.Message) ?? new string[] { e.Message };
				MessageBox.Show("An error occured: \"" + string.Join(", ", messages) + "\"\r\nCheck to see if the blog name is correct.");
			} finally {
				HideProgressBar();
			}
		}
		#endregion

		#region Inkbunny
		public async void PostToInkbunny() {
			try {
				if (this.currentImage == null) {
					MessageBox.Show("No image is selected.");
					return;
				}

				if (Inkbunny == null) {
					MessageBox.Show("You must log into Inkbunny before posting.");
					return;
				}

				var rating = new List<InkbunnyRatingTag>();
				if (chkInbunnyTag2.Checked) rating.Add(InkbunnyRatingTag.Nudity);
				if (chkInbunnyTag3.Checked) rating.Add(InkbunnyRatingTag.Violence);
				if (chkInbunnyTag4.Checked) rating.Add(InkbunnyRatingTag.SexualThemes);
				if (chkInbunnyTag5.Checked) rating.Add(InkbunnyRatingTag.StrongViolence);
				if (currentSubmission.PotentiallySensitive && !rating.Any()) {
					DialogResult r = MessageBox.Show(this, $"This image has a non-general rating on the source site. Are you sure you want to post it on Inkbunny without any ratings?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (r != DialogResult.OK) return;
				}

                LProgressBar.Report(0);
                ShowProgressBar();

				long submission_id = await Inkbunny.UploadAsync(files: new byte[][] {
					currentImage.Data
				});

                LProgressBar.Report(0.5);

                var keywords = txtInkbunnyTags.Text.Replace("#", "").Split(' ').Where(s => s != "").ToList();

                if (txtInkbunnyTitle.Text == "") {
                    MessageBox.Show("Please enter a title for this submission to use on Inkbunny.");
                } else {
                    var o = await Inkbunny.EditSubmissionAsync(
                        submission_id: submission_id,
                        title: txtInkbunnyTitle.Text,
                        desc: txtInkbunnyDescription.Text,
                        convert_html_entities: true,
                        type: InkbunnySubmissionType.Picture,
                        scraps: chkInkbunnyScraps.Checked,
                        isPublic: chkInkbunnyPublic.Checked,
                        notifyWatchersWhenPublic: chkInkbunnyNotifyWatchers.Checked,
                        keywords: keywords,
                        tag: rating
                    );
                    
                    lblPosted1.Visible = true;
                    lblPosted2.Visible = true;
                    lblPosted2.Text = $"https://inkbunny.net/submissionview.php?id={o.submission_id}";
                }
            } catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
                ShowException(ex, nameof(PostToInkbunny));
            } finally {
				HideProgressBar();
			}
		}
        #endregion

        #region Flickr
        public async void PostToFlickr() {
            try {
                var contentType = radFlickrPhoto.Checked ? ContentType.Photo
                        : radFlickrScreenshot.Checked ? ContentType.Screenshot
                        : radFlickrOther.Checked ? ContentType.Other
                        : ContentType.None;
                var safetyLevel = radFlickrSafe.Checked ? SafetyLevel.Safe
                    : radFlickrModerate.Checked ? SafetyLevel.Moderate
                    : radFlickrRestricted.Checked ? SafetyLevel.Restricted
                    : SafetyLevel.None;
                using (var ms = new MemoryStream(currentImage.Data, false)) {
                    var t1 = new TaskCompletionSource<string>();

                    LProgressBar.Report(0);
                    ShowProgressBar();

                    Flickr.UploadPictureAsync(
                        ms,
                        currentImage.FileName,
                        txtFlickrTitle.Text,
                        txtFlickrDesc.Text,
                        txtFlickrTags.Text.Replace("#", ""),
                        chkFlickrPublic.Checked,
                        chkFlickrFamily.Checked,
                        chkFlickrFriend.Checked,
                        contentType,
                        safetyLevel,
                        chkFlickrHidden.Checked ? HiddenFromSearch.Hidden : HiddenFromSearch.Visible,
                        result => {
                            if (result.HasError) {
                                t1.SetException(result.Error);
                            } else {
                                t1.SetResult(result.Result);
                            }
                        });

                    string photoId = await t1.Task;

                    lblPosted1.Visible = true;
                    lblPosted2.Visible = true;
                    lblPosted2.Text = $"https://www.flickr.com/photos/{FlickrAuth.User.UserId}/{photoId}";

                    var license = ddlFlickrLicense.SelectedItem as License;
                    if (license != null) {
                        LProgressBar.Report(0.5);

                        var t2 = new TaskCompletionSource<NoResponse>();
                        Flickr.PhotosLicensesSetLicenseAsync(photoId, license.LicenseId, result => {
                            if (result.HasError) {
                                t2.SetException(result.Error);
                            } else {
                                t2.SetResult(result.Result);
                            }
                        });
                        await t2.Task;
                    }
                }
            } catch (Exception ex) {
                ShowException(ex, nameof(PostToFlickr));
            } finally {
                HideProgressBar();
            }
        }
        #endregion

        #region Event handlers
        private void btnUp_Click(object sender, EventArgs e) {
            try {
                UpdateGalleryAsync(back: true);
            } catch (Exception ex) {
                Console.Error.WriteLine(ex.Message);
                Console.Error.WriteLine(ex.StackTrace);
                ShowException(ex, nameof(btnUp_Click));
            }
		}

		private void btnDown_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(next: true);
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Visible = pickTime.Visible = !chkNow.Checked;
		}

		private void btnPost_Click(object sender, EventArgs args) {
			PostToTumblr();
		}

		private void chkTitle_CheckedChanged(object sender, EventArgs e) {
			txtHeader.Enabled = chkHeader.Checked;
		}

		private void chkDescription_CheckedChanged(object sender, EventArgs e) {
			txtDescription.Enabled = chkDescription.Checked;
		}

		private void chkFooter_CheckedChanged(object sender, EventArgs e) {
			txtFooter.Enabled = chkFooter.Checked;
			txtURL.Enabled = chkFooter.Checked;
		}

		private void chkTags1_CheckedChanged(object sender, EventArgs e) {
			txtTags1.Enabled = chkTags1.Checked;
		}

		private void chkTags2_CheckedChanged(object sender, EventArgs e) {
			txtTags2.Enabled = chkTags2.Checked;
        }

        private async void changeSourceToolStripMenuItem_Click(object sender, EventArgs e) {
            try {
                await GetNewWrapper();
            } catch (Exception ex) {
                ShowException(ex, nameof(changeSourceToolStripMenuItem_Click));
            }
            UpdateGalleryAsync();
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e) {
            WrapperPosition = 0;
            UpdateGalleryAsync();
        }

        private void multiPhotoPostToolStripMenuItem_Click(object sender, EventArgs e) {
            if (SourceWrapper == null) return;
            using (var f = new MultiPhotoPostForm(GlobalSettings, SourceWrapper)) {
                f.TwitterCredentials = TwitterCredentials;
                f.TumblrClient = Tumblr;
                f.ShowDialog(this);
            }
        }

        private void optionsToolStripMenuItem_Click(object sender, EventArgs args) {
			using (SettingsDialog dialog = new SettingsDialog(GlobalSettings)) {
				if (dialog.ShowDialog() != DialogResult.Cancel) {
					GlobalSettings = dialog.Settings;
					GlobalSettings.Save();
					LoadFromSettings();
				}
            }
		}

		private void chkHTMLPreview_CheckedChanged(object sender, EventArgs e) {
            using (var form = new Form()) {
                form.Width = 400;
                form.Height = 300;
                var browser = new WebBrowser {
                    Dock = DockStyle.Fill
                };
                form.Controls.Add(browser);
                form.Load += (x, y) => {
                    browser.Navigate("about:blank");
                    browser.Document.Write(HTML_PREVIEW.Replace("{HTML}", CompileHTML()));
                };
                form.ShowDialog(this);
            }
        }

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var d = new AboutDialog()) d.ShowDialog(this);
        }

        private void lnkOriginalUrl_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (lnkOriginalUrl.Text.StartsWith("http"))
                Process.Start(lnkOriginalUrl.Text);
        }

        private void lnkTwitterLinkToInclude_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(lnkTwitterLinkToInclude.Text);
		}

		private void btnInkbunnyPost_Click(object sender, EventArgs e) {
			PostToInkbunny();
		}

		private void chkInkbunnyPublic_CheckedChanged(object sender, EventArgs e) {
			chkInkbunnyNotifyWatchers.Enabled = chkInkbunnyPublic.Checked;
		}

		private void txtTweetText_TextChanged(object sender, EventArgs e) {
			int length = txtTweetText.Text.Where(c => !char.IsLowSurrogate(c)).Count();
			if (chkIncludeLink.Checked) {
				length += (shortURLLengthHttps + 1);
			}
			lblTweetLength.Text = $"{length}/280";
		}

		private void chkIncludeLink_CheckedChanged(object sender, EventArgs e) {
			ResetTweetText();
		}

		private async void btnTweet_Click(object sender, EventArgs e) {
			if (TwitterCredentials == null) {
				MessageBox.Show("You must log into Twitter from the Options screen to send a tweet.");
				return;
			}

			string text = txtTweetText.Text;

			int length = text.Where(c => !char.IsLowSurrogate(c)).Count();
			if (chkIncludeLink.Checked) {
                if (this.SourceWrapper is LocalPathWrapper) {
                    MessageBox.Show(this, "Cannot include link when posting from local file.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }
                text += $" {lnkTwitterLinkToInclude.Text}";
				length += (shortURLLengthHttps + 1);
			}
			if (length > 280) {
				MessageBox.Show("This tweet is over 280 characters. Please shorten it or remove the link (if present.)");
				return;
			}

            LProgressBar.Report(0);
            ShowProgressBar();
            try {
                ITweet tweet = await Task.Run(() => Tweetinvi.Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
                    var options = new PublishTweetOptionalParameters();

                    if (chkIncludeImage.Checked) {
                        IMedia media = Upload.UploadImage(currentImage.Data);
                        options.Medias = new List<IMedia> { media };
                    }
                    LProgressBar.Report(0.5);

                    if (chkTweetPotentiallySensitive.Checked) {
                        options.PossiblySensitive = true;
                    }

                    return Tweet.PublishTweet(text, options);
			    }));

                if (tweet == null) {
                    string desc = Tweetinvi.ExceptionHandler.GetLastException().TwitterDescription;
                    MessageBox.Show(this, desc, "Could not send tweet");
                } else {
                    this.tweetCache.Add(tweet);

                    lblPosted1.Visible = true;
                    lblPosted2.Visible = true;
                    lblPosted2.Text = tweet.Url;
                }
            } catch (Exception ex) {
                ShowException(ex, nameof(btnTweet_Click));
            }

            HideProgressBar();
        }

        private void btnPostToFlickr_Click(object sender, EventArgs e) {
            PostToFlickr();
        }

        private void chkIncludeTitle_CheckedChanged(object sender, EventArgs e) {
			ResetTweetText();
		}

		private void chkIncludeDescription_CheckedChanged(object sender, EventArgs e) {
			ResetTweetText();
		}

		private void chkIncludeTag_CheckedChanged(object sender, EventArgs e) {
			ResetTweetText();
		}
        
        private void btnSaveDirBrowse_Click(object sender, EventArgs e) {
            using (var dialog = new FolderBrowserDialog()) {
                dialog.SelectedPath = txtSaveDir.Text;
                if (dialog.ShowDialog(this) == DialogResult.OK) {
                    txtSaveDir.Text = dialog.SelectedPath;
                }
            }
		}

		private void btnSaveLocal_Click(object sender, EventArgs e) {
            string path = Path.Combine(txtSaveDir.Text, txtSaveFilename.Text);
            if (File.Exists(path)) {
                var result = MessageBox.Show(this, $"The file {txtSaveFilename.Text} already exists. Would you like to overwrite it?", "Save", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (result == DialogResult.No) return;
            }

            File.WriteAllBytes(path, this.currentImage.Data);
        }

        private void deviantArtUploadControl1_UploadProgressChanged(double percentage) {
            ShowProgressBar();
            LProgressBar.Report(percentage);
        }

        private void deviantArtUploadControl1_Uploaded(string url) {
            HideProgressBar();

            lblPosted1.Visible = true;
            lblPosted2.Visible = true;
            lblPosted2.Text = url;
        }

        private void deviantArtUploadControl1_UploadError(Exception ex) {
            HideProgressBar();
        }

        private void lblPosted2_Click(object sender, EventArgs e) {
            if (lblPosted2.Text.StartsWith("http")) Process.Start(lblPosted2.Text);
        }

        private void reduceHeightToolStripMenuItem_Click(object sender, EventArgs e) {
            var last = thumbnails.Last();
            thumbnails = thumbnails.Take(thumbnails.Length - 1).ToArray();
            int h = last.Top - thumbnails.Last().Top;
            last.Parent.Controls.Remove(last);
            panel1.Height -= h;
            Height -= h;

            reduceHeightToolStripMenuItem.Enabled = thumbnails.Length > 1;
        }
        
        private void setTitleCommentsallTabsToolStripMenuItem_Click(object sender, EventArgs e) {
            ShowSetDescForm();
        }

        private void lnkFAC_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            Process.Start(lnkFAC.Text);
        }

        private void btnLaunchEFC_Click(object sender, EventArgs e) {
            string jsonFile = null, imageFile = null;
            if (this.currentImage != null) {
                char[] invalid = Path.GetInvalidFileNameChars();
                string basename = this.currentSubmission?.Title;
                if (string.IsNullOrEmpty(basename)) {
                    basename = "image";
                }
                basename = new string(basename.Select(c => invalid.Contains(c) ? '_' : c).ToArray());
                string ext = this.currentImage.MimeType.StartsWith("image/")
                    ? $".{this.currentImage.MimeType.Replace("image/", "")}"
                    : $".{Path.GetExtension(this.currentImage.FileName)}";
                string imageFilename = basename + ext;

                imageFile = Path.Combine(Path.GetTempPath(), imageFilename);
                File.WriteAllBytes(imageFile, this.currentImage.Data);

                string bbCode = HtmlToBBCode.ConvertHtml(txtDescription.Text);
                string plainText = Regex.Replace(bbCode, @"\[\/?(b|i|u|q|url=?[^\]]*)\]", "");

                jsonFile = Path.GetTempFileName();
                File.WriteAllText(jsonFile, JsonSerializer.ToJson(new {
                    imagePath = imageFile,
                    title = this.currentSubmission.Title,
                    description = plainText,
                    tags = this.currentSubmission.Tags,
                    nudity = new {
                        @explicit = this.currentSubmission.PotentiallySensitive
                    }
                }));
            }
            
            Process p = Process.Start(new ProcessStartInfo("java", $"-jar efc.jar {jsonFile}") {
                RedirectStandardError = true,
                UseShellExecute = false,
                WorkingDirectory = Environment.CurrentDirectory
            });

            Task.Run(() => p.WaitForExit())
                .ContinueWith(t => {
                    if (p.ExitCode != 0) {
                        string stderr = p.StandardError.ReadToEnd();
                        MessageBox.Show(null, stderr, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }

                    if (jsonFile != null) File.Delete(jsonFile);
                    if (imageFile != null) File.Delete(imageFile);
                });
        }
        #endregion

        public static BinaryFile MakeSquare(Image oldBitmap) {
			int newSize = Math.Max(oldBitmap.Width, oldBitmap.Height);
			Bitmap newBitmap = new Bitmap(newSize, newSize);

			int offsetX = (newSize - oldBitmap.Width) / 2;
			int offsetY = (newSize - oldBitmap.Height) / 2;

			using (Graphics g = Graphics.FromImage(newBitmap)) {
				g.DrawImage(oldBitmap, offsetX, offsetY, oldBitmap.Width, oldBitmap.Height);
			}

			using (MemoryStream stream = new MemoryStream()) {
				newBitmap.Save(stream, oldBitmap.RawFormat);
				return new BinaryFile(stream.ToArray());
			}
		}

        public static WebRequest CreateWebRequest(string url) {
            var req = WebRequest.Create(url);
			if (req is HttpWebRequest httpreq) {
				if (req.RequestUri.Host.EndsWith(".pximg.net")) {
					httpreq.Referer = "https://app-api.pixiv.net/";
				} else {
					httpreq.UserAgent = "CrosspostSharp/2.2 (https://github.com/libertyernie/CrosspostSharp)";
				}
			}
            return req;
        }
    }
}
