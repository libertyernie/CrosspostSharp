using DeviantArtFs;
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

		private void StatusPostForm_Shown(object sender, EventArgs e) {
			splitContainer1.Enabled = false;

			Settings settings = Settings.Load();

			foreach (var da in settings.DeviantArtTokens) {
				var checkbox = new CheckBox {
					Text = $"DeviantArt ({da.Username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToDeviantArt(da));
			}
			foreach (var m in settings.Pixelfed) {
				var checkbox = new CheckBox {
					Text = $"{m.AppRegistration.Instance} ({m.Username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToMastodon(m));
			}
			foreach (var m in settings.Pleronet) {
				var checkbox = new CheckBox {
					Text = $"{m.AppRegistration.Instance} ({m.Username})",
					AutoSize = true
				};
				pnlAccounts.Controls.Add(checkbox);
				_postFunctions.Add(checkbox, () => PostToMastodon(m));
			}

			splitContainer1.Enabled = true;
		}

		private async Task<Uri> PostToDeviantArt(IDeviantArtAccessToken token) {
			var post = await DeviantArtFs.Api.User.PostStatusAsync(
				token,
				DeviantArtFs.Api.User.EmbeddableStatusContent.None,
				CurrentHtml);
			try {
				var status = await DeviantArtFs.Api.User.GetStatusAsync(token, post.statusid);
				return new Uri(status.url.Value);
			} catch (Exception) {
				var user = await DeviantArtFs.Api.User.WhoamiAsync(token);
				return new Uri("https://www.deviantart.com/" + user);
			}
		}

		private async Task<Uri> PostToMastodon(Settings.PleronetSettings m) {
			var status = await m.GetClient().PostStatus(
				CurrentText,
				spoilerText: textBox1.Text == "" ? null : textBox1.Text);
			return new Uri(status.Url);
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
