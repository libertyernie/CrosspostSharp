using ArtSourceWrapper;
using System;
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

		private void btnSearch_Click(object sender, EventArgs e) {
			var settings = Settings.Load();

			_offset = 0;
			_count = Math.Max(1, (int)numCount.Value);
			_wrapper = new MetaWrapper("All", new ISiteWrapper[] {
				new DeviantArtWrapper(),
				new WeasylWrapper(new WeasylGalleryIdWrapper(settings.Weasyl.First().apiKey))
			});

			Populate();
		}

		private async void Populate() {
			flowLayoutPanel1.Controls.Clear();
			if (_wrapper == null) return;

			int current = 0;

			while (_wrapper.Cache.Skip(_offset).Count() < _count && !_wrapper.IsEnded) {
				await _wrapper.FetchAsync();

				int i = 0;
				foreach (var w in _wrapper.Cache.Skip(_offset).Take(_count)) {
					if (i >= current) {
						var l = new LinkLabel {
							Text = w.Title
						};
						l.Click += (o, a) => Process.Start(w.ViewURL);
						flowLayoutPanel1.Controls.Add(l);
						current++;
					}
					i++;
				}
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
	}
}
