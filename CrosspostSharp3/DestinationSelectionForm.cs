using DeviantArtControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class DestinationSelectionForm : Form {
		public struct ArtworkParameters {
			public byte[] data;
			public string title;
			public string htmlDescription;
			public IEnumerable<string> tags;
			public bool mature;
			public string originalUrl;
		}

		private ArtworkParameters _artwork;

		public DestinationSelectionForm(ArtworkParameters artwork) {
			InitializeComponent();
			_artwork = artwork;
		}

		private void DestinationSelectionForm_Shown(object sender, EventArgs e) {
			Settings s = Settings.Load();
			if (s.DeviantArt?.RefreshToken == null) {
				btnDeviantArt.Enabled = false;
			}
			if (!s.Twitter.Any()) {
				btnTwitter.Enabled = false;
			}
		}

		private void btnDeviantArt_Click(object sender, EventArgs e) {
			using (var f = new Form()) {
				f.Width = 600;
				f.Height = 350;
				var d = new DeviantArtUploadControl {
					Dock = DockStyle.Fill
				};
				f.Controls.Add(d);
				d.Uploaded += url => f.Close();
				d.SetSubmission(
					_artwork.data,
					_artwork.title,
					_artwork.htmlDescription,
					_artwork.tags,
					_artwork.mature,
					_artwork.originalUrl);
				f.ShowDialog(this);
			}
		}

		private void btnTwitter_Click(object sender, EventArgs e) {

		}
	}
}
