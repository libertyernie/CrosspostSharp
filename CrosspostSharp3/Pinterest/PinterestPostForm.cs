using PinSharp;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Tweetinvi;
using Tweetinvi.Models;

namespace CrosspostSharp3 {
	public partial class PinterestPostForm : Form {
		private readonly PinSharpClient _client;
		private readonly string _boardName;
		private readonly ArtworkData _artworkData;

		public PinterestPostForm(Settings.PinterestSettings s, ArtworkData d) {
			InitializeComponent();
			_client = new PinSharpClient(s.accessToken);
			_boardName = s.boardName;
			_artworkData = d;
			lblUsername1.Text = s.username;
			lblUsername2.Text = s.boardName;

			textBox1.Text = HtmlConversion.ConvertHtmlToText(d.description);
		}

		private async void PinterestPostForm_Shown(object sender, EventArgs e) {
			try {
				var user = await _client.Me.GetUserAsync();
				lblUsername1.Text = user.UserName;

				var req = WebRequestFactory.Create(user.Images.W60.Url);
				using (var resp = await req.GetResponseAsync())
				using (var stream = resp.GetResponseStream())
				using (var ms = new MemoryStream()) {
					await stream.CopyToAsync(ms);
					ms.Position = 0;
					picUserIcon.Image = Image.FromStream(ms);
				}
			} catch (Exception) { }
		}

		private async void btnPost_Click(object sender, EventArgs e) {
			try {
				await _client.Pins.CreatePinFromBase64Async(_boardName, Convert.ToBase64String(_artworkData.data), textBox1.Text, _artworkData.url);
				Close();
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.StackTrace, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}
	}
}
