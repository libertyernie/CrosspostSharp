using System;
using System.Configuration;
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
using System.Security.Cryptography;

namespace ArtSync {
	public partial class WeasylForm : Form {
		private static Settings GlobalSettings;

		public IWrapper SourceWrapper { get; private set; }
		public string SourceUsername { get; private set; }

		private TumblrClient Tumblr;
		public string TumblrUsername { get; private set; }

		private ITwitterCredentials TwitterCredentials;
		private int shortURLLength;
		private int shortURLLengthHttps;
		private List<ITweet> tweetCache;

		public InkbunnyClient Inkbunny;

		// Stores references to the four WeasylThumbnail controls along the side. Each of them is responsible for fetching the submission information and image.
		private WeasylThumbnail[] thumbnails;

		// The current submission's details and image, which are fetched by the WeasylThumbnail and passed to SetCurrentImage.
		private ISubmissionWrapper currentSubmission;
		private BinaryFile currentImage;

		// The image displayed in the main panel. This is used again if WeasylSync needs to add padding to the image to force a square aspect ratio.
		private Bitmap currentImageBitmap;

		// The existing Tumblr post for the selected Weasyl submission, if any - looked up by using the #weasylXXXXXX tag.
		private BasePost ExistingTumblrPost;

		// Allows WeasylThumbnail access to the progress bar.
		public LProgressBar LProgressBar {
			get {
				return lProgressBar1;
			}
		}

		private void InvokeAndForget(Action f) {
			if (this.IsHandleCreated & this.InvokeRequired) {
				this.BeginInvoke(f);
			} else {
				f();
			}
		}

		public WeasylForm() {
			InitializeComponent();
			tweetCache = new List<ITweet>();

			GlobalSettings = Settings.Load();

			thumbnails = new WeasylThumbnail[] { thumbnail1, thumbnail2, thumbnail3 };

			this.Shown += (o, e) => LoadFromSettings();
		}

		#region GUI updates
		private async void LoadFromSettings() {
			try {
                LProgressBar.Report(0);
				LProgressBar.Visible = true;
                
                bool refreshGallery = false;
                if (string.IsNullOrEmpty(GlobalSettings.DeviantArt.RefreshToken) ^ !(SourceWrapper is DeviantArtWrapper)) {
                    SourceWrapper = null;
                    refreshGallery = true;

                    if (!string.IsNullOrEmpty(GlobalSettings.DeviantArt.RefreshToken)) {
                        try {
                            var w = new DeviantArtWrapper(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);
                            SourceWrapper = w;
                            string oldToken = GlobalSettings.DeviantArt.RefreshToken;
                            string newToken = await w.UpdateTokens(oldToken);
                            if (oldToken != newToken) {
                                GlobalSettings.DeviantArt.RefreshToken = newToken;
                                GlobalSettings.Save();
                            }
                        } catch (DeviantArtException e) when (e.Message == "User canceled") {
                            GlobalSettings.DeviantArt.RefreshToken = null;
                            SourceWrapper = null;
                        }
                    }

                    if (SourceWrapper == null) {
                        SourceWrapper = new WeasylWrapper(GlobalSettings.Weasyl.APIKey);
                    }

                    lblWeasylStatus1.Text = SourceWrapper.SiteName + ":";
                }

                Token token = GlobalSettings.TumblrToken;
				if (token != null && token.IsValid) {
					if (Tumblr != null) Tumblr.Dispose();
					Tumblr = new TumblrClientFactory().Create<TumblrClient>(
						OAuthConsumer.Tumblr.CONSUMER_KEY,
						OAuthConsumer.Tumblr.CONSUMER_SECRET,
						token);
				}

                if (GlobalSettings.Inkbunny.Sid != null && GlobalSettings.Inkbunny.UserId != null) {
                    Inkbunny = new InkbunnyClient(GlobalSettings.Inkbunny.Sid, GlobalSettings.Inkbunny.UserId.Value);
                }

				string user = null;
				try {
					user = await SourceWrapper.WhoamiAsync();
					lblWeasylStatus2.Text = user ?? "not logged in";
					lblWeasylStatus2.ForeColor = string.IsNullOrEmpty(lblWeasylStatus2.Text)
						? SystemColors.WindowText
						: Color.DarkGreen;
				} catch (Exception e) {
					lblWeasylStatus2.Text = ((e as WebException)?.Response as HttpWebResponse)?.StatusDescription ?? e.Message;
					lblWeasylStatus2.ForeColor = Color.DarkRed;
				}
				if (user == null || SourceUsername != user) {
                    refreshGallery = true;
                }
                SourceUsername = user;

                LProgressBar.Report(1 / 4f);

                if (Tumblr == null) {
					lblTumblrStatus2.Text = "not logged in";
					lblTumblrStatus2.ForeColor = SystemColors.WindowText;
				} else {
					try {
						TumblrUsername = (await Tumblr.GetUserInfoAsync()).Name;
						lblTumblrStatus2.Text = TumblrUsername ?? "not logged in";
						lblTumblrStatus2.ForeColor = TumblrUsername == null
							? SystemColors.WindowText
							: Color.DarkGreen;
					} catch (Exception e) {
						TumblrUsername = null;
						lblTumblrStatus2.Text = e.Message;
						lblTumblrStatus2.ForeColor = Color.DarkRed;
					}
                }

                LProgressBar.Report(2 / 4f);

                if (Inkbunny == null) {
                    lblInkbunnyStatus2.Text = "not logged in";
                    lblInkbunnyStatus2.ForeColor = SystemColors.WindowText;
                } else {
                    try {
                        lblInkbunnyStatus2.Text = await Inkbunny.GetUsername();
                        lblInkbunnyStatus2.ForeColor = Color.DarkGreen;
                    } catch (Exception e) {
                        Inkbunny = null;
                        lblTumblrStatus2.Text = e.Message;
                        lblTumblrStatus2.ForeColor = Color.DarkRed;
                    }
                }

                LProgressBar.Report(3 / 4f);

                TwitterCredentials = GlobalSettings.TwitterCredentials;
				try {
					string screenName = TwitterCredentials?.GetScreenName();
					lblTwitterStatus2.Text = screenName ?? "not logged in";
					lblTwitterStatus2.ForeColor = screenName == null
						? SystemColors.WindowText
						: Color.DarkGreen;

					if (screenName != null) {
						Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
							var conf = Tweetinvi.Help.GetTwitterConfiguration();
							shortURLLength = conf.ShortURLLength;
							shortURLLengthHttps = conf.ShortURLLengthHttps;
						});

						if (!tweetCache.Any()) {
							tweetCache.AddRange(await GetMoreOldTweets());
						}
					}
				} catch (Exception e) {
					TwitterCredentials = null;
					lblTwitterStatus2.Text = e.Message;
					lblTwitterStatus2.ForeColor = Color.DarkRed;
				}

				LProgressBar.Visible = false;

				txtHeader.Text = GlobalSettings.Defaults.HeaderHTML ?? "";
				txtFooter.Text = GlobalSettings.Defaults.FooterHTML ?? "";
				// Global tags that you can include in each submission if you want.
				txtTags2.Text = GlobalSettings.Defaults.Tags ?? "";

				chkWeasylSubmitIdTag.Checked = GlobalSettings.Defaults.IncludeWeasylTag;

				if (refreshGallery) UpdateGalleryAsync();
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				MessageBox.Show(e.Message);
			}
		}

		private void UpdateSettingsInWindow(bool refreshGallery) {
			
		}

		// This function is called after clicking on a WeasylThumbnail.
		// It needs to be run on the GUI thread - WeasylThumbnail handles this using Invoke.
		public async void SetCurrentImage(ISubmissionWrapper submission, BinaryFile file) {
			this.currentSubmission = submission;
			if (submission != null) {
				txtTitle.Text = submission.Title;
				txtDescription.Text = submission.HTMLDescription;
				string bbCode = HtmlToBBCode.ConvertHtml(txtDescription.Text);
				txtInkbunnyDescription.Text = bbCode;
				txtURL.Text = submission.URL;

				ResetTweetText();

				lnkTwitterLinkToInclude.Text = submission.URL;
                chkTweetPotentiallySensitive.Checked = submission.PotentiallySensitive;

				txtTags1.Text = string.Join(" ", submission.Tags.Select(s => "#" + s));
                chkWeasylSubmitIdTag.Text = submission.GeneratedUniqueTag;

				pickDate.Value = pickTime.Value = submission.Timestamp;
				UpdateHTMLPreview();
			}
			this.currentImage = file;
			if (file == null) {
				mainPictureBox.Image = null;
			} else {
				try {
					this.currentImageBitmap = (Bitmap)Image.FromStream(new MemoryStream(file.Data));
					mainPictureBox.Image = this.currentImageBitmap;
				} catch (ArgumentException) {
					MessageBox.Show("This submission is not an image file.");
					mainPictureBox.Image = null;
				}
            }
            UpdateExistingTweetLink();
            try {
                await Task.WhenAll(
                    UpdateExistingTumblrPostLink(),
                    UpdateExistingInkbunnyPostLink()
                );
            } catch (Exception) {
                MessageBox.Show("Could not check for existing post on one or more sites.");
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

			int maxLength = 140;
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
			return Task.Run(() => Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
				var user = Tweetinvi.User.GetAuthenticatedUser();
				var parameters = new UserTimelineParameters {
					MaximumNumberOfTweetsToRetrieve = 200,
					TrimUser = true,
					SinceId = sinceId
				};

				TweetinviConfig.CurrentThreadSettings.TweetMode = TweetMode.Extended;
				var tweets = Timeline.GetUserTimeline(user, parameters);
				if (tweets == null) {
					var x = ExceptionHandler.GetLastException();
					throw new Exception(x.TwitterDescription, x.WebException);
				}
				return tweets
					.Where(t => t.CreatedBy.Id == user.Id);
			}));
		}

		// Progress is posted back to the LProgressBar, which handles its own thread safety using BeginInvoke.
		private async void UpdateGalleryAsync(bool back = false, bool next = false) {
			try {
                if (SourceWrapper == null) return;

                LProgressBar.Report(0);
                LProgressBar.Visible = true;

                var result =
                    back ? await SourceWrapper.PreviousPageAsync()
                    : next ? await SourceWrapper.NextPageAsync()
                    : await SourceWrapper.UpdateGalleryAsync(new UpdateGalleryParameters {
                        Count = 4,
                        Weasyl_LoadCharacters = loadCharactersToolStripMenuItem.Checked,
                        Progress = LProgressBar
                    });
				for (int i = 0; i < this.thumbnails.Length; i++) {
					this.thumbnails[i].Submission = i < result.Submissions.Count
						? result.Submissions[i]
						: null;
				}

                InvokeAndForget(() => {
                    btnUp.Enabled = result.HasLess;
                    btnDown.Enabled = result.HasMore;
                });
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name);
            } finally {
                LProgressBar.Visible = false;
            }
        }
		#endregion

		#region Lookup
		private async Task UpdateExistingTumblrPostLink() {
            if (Tumblr != null) {
                this.btnPost.Enabled = false;
                this.lnkTumblrFound.Enabled = false;
                this.lnkTumblrFound.Text = $"checking your Tumblr for tag {chkWeasylSubmitIdTag.Text}...";
                var posts = await this.GetTaggedPostsForSubmissionAsync();
				this.ExistingTumblrPost = posts.Result.FirstOrDefault();
				this.btnPost.Enabled = true;
				if (this.ExistingTumblrPost == null) {
					this.lnkTumblrFound.Text = $"tag not found ({chkWeasylSubmitIdTag.Text})";
				} else {
					this.lnkTumblrFound.Text = this.ExistingTumblrPost.Url;
                    this.lnkTumblrFound.Enabled = true;
                }
			}
        }

        private async Task UpdateExistingInkbunnyPostLink() {
            if (Inkbunny != null) {
                using (var m = MD5.Create()) {
                    byte[] hash = m.ComputeHash(this.currentImage.Data);
                    string hashStr = string.Join("", hash.Select(b => ((int)b).ToString("X2")));
                    this.lnkInkbunnyFound.Enabled = false;
                    this.lnkInkbunnyFound.Text = $"checking Inkbunny for MD5 hash {hashStr}...";
                    var resp = await Inkbunny.SearchByMD5(hashStr);
                    var existing = resp.submissions.FirstOrDefault();
                    if (existing == null) {
                        this.lnkInkbunnyFound.Text = $"MD5 hash not found ({hashStr})";
                    } else {
                        this.lnkInkbunnyFound.Text = $"https://inkbunny.net/submissionview.php?id={existing.submission_id}";
                        this.lnkInkbunnyFound.Enabled = true;
                    }
                }
            }
        }

        private void UpdateExistingTweetLink() {
            string url = this.currentSubmission.URL;
            foreach (var tweet in tweetCache) {
                if (tweet.Entities.Urls.Any(u => u.ExpandedURL == url)) {
                    this.lnkTwitterFound.Enabled = true;
                    this.lnkTwitterFound.Text = "https://mobile.twitter.com/twitter/status/" + tweet.IdStr;
                    return;
                }
            }
            this.lnkTwitterFound.Enabled = false;
            this.lnkTwitterFound.Text = $"Link to original not found in {tweetCache.Count} most recent tweets";
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

			html.Replace("{TITLE}", WebUtility.HtmlEncode(txtTitle.Text))
                .Replace("{URL}", txtURL.Text)
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

		private void UpdateHTMLPreview() {
			previewPanel.Visible = chkHTMLPreview.Checked;
			previewPanel.Controls.Clear();
			if (chkHTMLPreview.Checked) {
				previewPanel.Controls.Add(new WebBrowser {
					DocumentText = HTML_PREVIEW.Replace("{HTML}", this.CompileHTML()),
					Dock = DockStyle.Fill
				});
			}
		}
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

		private async Task<Posts> GetTaggedPostsForSubmissionAsync() {
			string uniquetag = chkWeasylSubmitIdTag.Text.Replace("#", "");
			return await Tumblr.GetPostsAsync(GlobalSettings.Tumblr.BlogName, 0, 1, PostType.All, false, false, PostFilter.Html, uniquetag);
		}

		private async void PostToTumblr1() {
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
				LProgressBar.Visible = true;

				long? updateid = null;
				if (this.ExistingTumblrPost != null) {
					DialogResult result = new PostAlreadyExistsDialog(chkWeasylSubmitIdTag.Text, this.ExistingTumblrPost.Url).ShowDialog();
					if (result == DialogResult.Cancel) {
						LProgressBar.Visible = false;
						return;
					} else if (result == PostAlreadyExistsDialog.Result.Replace) {
						updateid = this.ExistingTumblrPost.Id;
					}
				}

                LProgressBar.Report(0.5);

				var tags = new List<string>();
				if (chkTags1.Checked) tags.AddRange(txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""));
				if (chkTags2.Checked) tags.AddRange(txtTags2.Text.Replace("#", "").Split(' ').Where(s => s != ""));
				if (chkWeasylSubmitIdTag.Checked) tags.AddRange(chkWeasylSubmitIdTag.Text.Replace("#", "").Split(' ').Where(s => s != ""));

				BinaryFile imageToPost = GlobalSettings.Tumblr.AutoSidePadding && this.currentImageBitmap.Height > this.currentImageBitmap.Width
					? MakeSquare(this.currentImageBitmap)
					: currentImage;

				PostData post = PostData.CreatePhoto(new BinaryFile[] { imageToPost }, CompileHTML(), txtURL.Text, tags);
				post.Date = chkNow.Checked
					? (DateTimeOffset?)null
					: (pickDate.Value.Date + pickTime.Value.TimeOfDay);

				Task<PostCreationInfo> task = updateid == null
					? Tumblr.CreatePostAsync(GlobalSettings.Tumblr.BlogName, post)
					: Tumblr.EditPostAsync(GlobalSettings.Tumblr.BlogName, updateid.Value, post);
				PostCreationInfo info = await task;
				await UpdateExistingTumblrPostLink();
			} catch (Exception e) {
				Console.Error.WriteLine(e.Message);
				Console.Error.WriteLine(e.StackTrace);
				var messages = (e as AggregateException)?.InnerExceptions?.Select(x => x.Message) ?? new string[] { e.Message };
				MessageBox.Show("An error occured: \"" + string.Join(", ", messages) + "\"\r\nCheck to see if the blog name is correct.");
			} finally {
				LProgressBar.Visible = false;
			}
		}
		#endregion

		#region Inkbunny
		public async void PostToInkbunny1() {
			try {
				if (this.currentImage == null) {
					MessageBox.Show("No image is selected.");
					return;
				}

				if (Inkbunny == null) {
					MessageBox.Show("You must log into Inkbunny before posting.");
					return;
				}

				var rating = new InkbunnyRating() {
					Nudity = chkInbunnyTag2.Checked,
					Violence = chkInbunnyTag3.Checked,
					SexualThemes = chkInbunnyTag4.Checked,
					StrongViolence = chkInbunnyTag5.Checked,
				};
				if (currentSubmission.PotentiallySensitive && !rating.Any) {
					DialogResult r = MessageBox.Show(this, $"This image has a non-general rating on the source site. Are you sure you want to post it on Inkbunny without any ratings?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Warning);
					if (r != DialogResult.OK) return;
				}

                LProgressBar.Report(0);
                LProgressBar.Visible = true;

				long submission_id = await Inkbunny.Upload(files: new byte[][] {
					currentImage.Data
				});

                LProgressBar.Report(0.5);

                var o = await Inkbunny.EditSubmission(
					submission_id: submission_id,
					title: txtTitle.Text,
					desc: txtInkbunnyDescription.Text,
					convert_html_entities: true,
					type: SubmissionType.Picture,
					scraps: chkInkbunnyScraps.Checked,
					isPublic: chkInkbunnyPublic.Checked,
					notifyWatchersWhenPublic: chkInkbunnyNotifyWatchers.Checked,
					keywords: txtTags1.Text.Replace("#", "").Split(' ').Where(s => s != ""),
					tag: rating
				);
				Console.WriteLine(o.submission_id);
				Console.WriteLine(o.twitter_authentication_success);
                await UpdateExistingInkbunnyPostLink();
            } catch (Exception ex) {
				Console.Error.WriteLine(ex.Message);
				Console.Error.WriteLine(ex.StackTrace);
				MessageBox.Show("An error occured: \"" + ex.Message + "\"\r\nCheck to see if the blog name is correct.");
			} finally {
				LProgressBar.Visible = false;
			}
		}
		#endregion

		#region Event handlers
		private void btnUp_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(back: true);
		}

		private void btnDown_Click(object sender, EventArgs e) {
			UpdateGalleryAsync(next: true);
		}

		private void chkNow_CheckedChanged(object sender, EventArgs e) {
			pickDate.Visible = pickTime.Visible = !chkNow.Checked;
		}

		private void btnPost_Click(object sender, EventArgs args) {
			PostToTumblr1();
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


		private void loadCharactersToolStripMenuItem_Click(object sender, EventArgs e) {
			UpdateGalleryAsync();
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
			UpdateHTMLPreview();
		}

		private void aboutToolStripMenuItem_Click(object sender, EventArgs e) {
			using (var d = new AboutDialog()) d.ShowDialog(this);
        }

        private void lnkTumblrFound_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (lnkTumblrFound.Text.StartsWith("http"))
                Process.Start(lnkTumblrFound.Text);
        }

        private void lnkInkbunnyFound_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (lnkInkbunnyFound.Text.StartsWith("http"))
                Process.Start(lnkInkbunnyFound.Text);
        }

        private void lnkTwitterFound_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
            if (lnkTwitterFound.Text.StartsWith("http"))
                Process.Start(lnkTwitterFound.Text);
        }

        private void lnkTwitterLinkToInclude_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e) {
			Process.Start(lnkTwitterLinkToInclude.Text);
		}

		private void btnInkbunnyPost_Click(object sender, EventArgs e) {
			PostToInkbunny1();
		}

		private void chkInkbunnyPublic_CheckedChanged(object sender, EventArgs e) {
			chkInkbunnyNotifyWatchers.Enabled = chkInkbunnyPublic.Checked;
		}

		private void txtTweetText_TextChanged(object sender, EventArgs e) {
			int length = txtTweetText.Text.Where(c => !char.IsLowSurrogate(c)).Count();
			if (chkIncludeLink.Checked) {
				length += (shortURLLengthHttps + 1);
			}
			lblTweetLength.Text = $"{length}/140";
		}

		private void chkIncludeLink_CheckedChanged(object sender, EventArgs e) {
			ResetTweetText();
		}

		private void btnTweet_Click(object sender, EventArgs e) {
			if (TwitterCredentials == null) {
				MessageBox.Show("You must log into Twitter from the Options screen to send a tweet.");
				return;
			}

			string text = txtTweetText.Text;

			int length = text.Where(c => !char.IsLowSurrogate(c)).Count();
			if (chkIncludeLink.Checked) {
				text += $" {lnkTwitterLinkToInclude.Text}";
				length += (shortURLLengthHttps + 1);
			}
			if (length > 140) {
				MessageBox.Show("This tweet is over 140 characters. Please shorten it or remove the Weasyl link (if present.)");
				return;
			}

            LProgressBar.Report(0);
            LProgressBar.Visible = true;
			Task.Run(() => Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
				var options = new PublishTweetOptionalParameters();

				if (chkIncludeImage.Checked) {
					IMedia media = Upload.UploadImage(currentImage.Data);
					options.Medias = new List<IMedia> { media };
                }
                LProgressBar.Report(0.5);

                if (chkTweetPotentiallySensitive.Checked) {
					options.PossiblySensitive = true;
				}

				ITweet tweet = Tweet.PublishTweet(text, options);

				if (tweet == null) {
					string desc = ExceptionHandler.GetLastException().TwitterDescription;
					MessageBox.Show(this, desc, "Could not send tweet");
				} else {
					this.tweetCache.Add(tweet);
					this.InvokeAndForget(() => UpdateExistingTweetLink());
				}
				LProgressBar.Visible = false;
			}));
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
		#endregion

		private static BinaryFile MakeSquare(Bitmap oldBitmap) {
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
    }
}
