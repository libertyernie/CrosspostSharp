using DeviantArtFs;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class StatusPostForm : Form {
		private readonly Dictionary<CheckBox, Func<Task<Uri>>> _postFunctions;

		public string CurrentText => txtStatus.Text;
		public string CurrentHtml => WebUtility.HtmlEncode(CurrentText);

		public StatusPostForm() {
			InitializeComponent();
			_postFunctions = new Dictionary<CheckBox, Func<Task<Uri>>>();
		}

		private async void StatusPostForm_Shown(object sender, EventArgs e) {
			splitContainer1.Enabled = false;

			Settings settings = Settings.Load();
			settings.UpdateFormat();

			foreach (var da in settings.DeviantArtTokens) {
				var checkbox = new CheckBox {
					Text = $"DeviantArt ({da.Username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToDeviantArt(da));
			}
			foreach (var m in settings.Mastodon) {
				var checkbox = new CheckBox {
					Text = $"{m.Instance} ({m.username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToMastodon(m));
			}
			foreach (var p in settings.Pillowfort) {
				var checkbox = new CheckBox {
					Text = $"Pillowfort ({p.username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToPillowfort(p));
			}
			foreach (var t in settings.Twitter) {
				var checkbox = new CheckBox {
					Text = $"Twitter ({t.screenName})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToTwitter(t));
			}
			foreach (var t in settings.Tumblr) {
				var checkbox = new CheckBox {
					Text = $"Tumblr ({t.blogName})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToTumblr(t));
			}

			splitContainer1.Enabled = true;
		}

		private async Task<Uri> PostToDeviantArt(IDeviantArtAccessToken token) {
			Guid id = await DeviantArtFs.Requests.User.StatusPost.ExecuteAsync(
				token,
				new DeviantArtFs.Requests.User.StatusPostRequest(CurrentHtml));
			try {
				var status = await DeviantArtFs.Requests.User.StatusById.ExecuteAsync(token, id);
				return new Uri(status.Url);
			} catch (DeviantArtException) {
				return new Uri("https://www.deviantart.com/" + await DeviantArtFs.Requests.User.Whoami.ExecuteAsync(token));
			}
		}

		private async Task<Uri> PostToMastodon(Settings.MastodonSettings m) {
			var status = await MapleFedNet.Api.Statuses.Posting(
				m,
				CurrentText,
				spoiler_text: textBox1.Text == "" ? null : textBox1.Text);
			return new Uri(status.Url);
		}

		private async Task<Uri> PostToPillowfort(Settings.PillowfortSettings p) {
			var client = new PillowfortFs.PillowfortClient() {
				Cookie = p.cookie
			};
			await client.SubmitPostAsync(new PillowfortFs.PostRequest(
				title: "",
				content: CurrentHtml,
				tags: Enumerable.Empty<string>(),
				privacy: PillowfortFs.PrivacyLevel.Public,
				rebloggable: true,
				commentable: true,
				nsfw: false,
				media: PillowfortFs.PillowfortMediaBuilder.None));
			string username = await client.WhoamiAsync();
			return new Uri($"https://pillowfort.social/{username}");
		}

		private async Task<Uri> PostToTwitter(Settings.TwitterSettings t) {
			var tweet = await Tweetinvi.Auth.ExecuteOperationWithCredentials(
				t.GetCredentials(),
				async () => {
					return await Tweetinvi.TweetAsync.PublishTweet(CurrentText);
				});
			if (tweet == null) {
				throw new Exception("Could not post tweet");
			}
			return new Uri(tweet.Url);
		}

		private async Task<Uri> PostToTumblr(Settings.TumblrSettings t) {
			var client = new TumblrClientFactory().Create<TumblrClient>(
				OAuthConsumer.Tumblr.CONSUMER_KEY,
				OAuthConsumer.Tumblr.CONSUMER_SECRET,
				new DontPanic.TumblrSharp.OAuth.Token(t.tokenKey, t.tokenSecret));
			PostData post = PostData.CreateText(CurrentHtml);
			PostCreationInfo info = await client.CreatePostAsync(t.blogName, post);
			var created = await client.GetPostAsync(t.blogName, info.PostId);
			return new Uri(created.Url);
		}

		private async Task Run(KeyValuePair<CheckBox, Func<Task<Uri>>> pair) {
			try {
				var uri = await pair.Value();
				pair.Key.Checked = false;
				pair.Key.Enabled = false;
				pair.Key.Text = uri.OriginalString;
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name);
			}
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			btnPost.Enabled = false;
			List<Task<Uri>> tasks = new List<Task<Uri>>();
			await Task.WhenAll(_postFunctions
				.Where(x => x.Key.Checked)
				.Select(x => Run(x)));
			btnPost.Enabled = true;
		}
	}
}
