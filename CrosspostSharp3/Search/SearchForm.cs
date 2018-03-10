using ArtSourceWrapper;
using System;
using System.Collections.Async;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3.Search {
	public partial class SearchForm : Form {
		private MetaWrapper _wrapper;
		private int _offset;
		private int _count = 12;

		public SearchForm() {
			InitializeComponent();
		}

		private IEnumerable<ISiteWrapper> GetWrappers() {
			var settings = Settings.Load();
			if (settings.DeviantArt.RefreshToken != null) {
				yield return new DeviantArtWrapper(new DeviantArtSearchWrapper(txtSearch.Text));
			}
			if (settings.FurAffinity.Any()) {
				var fa = settings.FurAffinity.First();
				yield return new FurAffinityWrapper(new FurAffinitySearchWrapper(fa.a, fa.b, txtSearch.Text));
			}
		}

		private void btnSearch_Click(object sender, EventArgs e) {
			var settings = Settings.Load();

			_offset = 0;
			_count = Math.Max(1, (int)numCount.Value);
			_wrapper = new MetaWrapper("All", GetWrappers());

			Populate();
		}

		private async void Populate() {
			flowLayoutPanel1.Controls.Clear();
			if (_wrapper == null) return;

			var e = await _wrapper.Skip(_offset).Take(_count).GetAsyncEnumeratorAsync();
			while (await e.MoveNextAsync()) {
				var w = e.Current;
				var t = new Thumbnail(w);
				foreach (Control c in t.Controls) {
					c.Click += (o, a) => {
						Process.Start(w.ViewURL);
					};
					c.MouseEnter += (o, a) => {
						lock (lblHoverUrl) {
							lblHoverUrl.Text = w.ViewURL;
						}
					};
					c.MouseLeave += (o, a) => {
						lock (lblHoverUrl) {
							if (lblHoverUrl.Text == w.ViewURL) lblHoverUrl.Text = "";
						}
					};
				}
				flowLayoutPanel1.Controls.Add(t);
			}
		}

		private void btnPrevious_Click(object sender, EventArgs e) {
			_offset -= _count;
			if (_offset < 0) _offset = 0;
			Populate();
		}

		private void btnNext_Click(object sender, EventArgs e) {
			_offset += _count;
			Populate();
		}

		private void numCount_ValueChanged(object sender, EventArgs e) {
			_count = (int)numCount.Value;
		}
	}
}
