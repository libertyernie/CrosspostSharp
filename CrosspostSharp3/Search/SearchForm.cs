using ArtSourceWrapper;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using FAExportLib;
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
		public class ListItem {
			public readonly string _name;
			public readonly Func<ISiteWrapper> _wrapperFactory;

			public ListItem(string name, Func<ISiteWrapper> wrapper) {
				_name = name;
				_wrapperFactory = wrapper;
			}

			public ISiteWrapper CreateWrapper() {
				return _wrapperFactory();
			}

			public override string ToString() {
				return _name;
			}
		}

		private MetaWrapper _wrapper;
		private int _offset;
		private int _count = 12;

		public SearchForm() {
			InitializeComponent();

			var settings = Settings.Load();
			if (settings.DeviantArt.RefreshToken != null) {
				listBox1.Items.Add(new ListItem("DeviantArt", () => new DeviantArtWrapper(new DeviantArtSearchWrapper(txtSearch.Text))));
			}
			if (settings.FurAffinity.Any()) {
				var fa = settings.FurAffinity.First();
				listBox1.Items.Add(new ListItem("FurAffinity", () => {
					FARating r = 0;
					if (chkGeneral.Checked) r |= FARating.general;
					if (chkMature.Checked) r |= FARating.mature;
					if (chkAdult.Checked) r |= FARating.adult;
					return new FurAffinityWrapper(new FurAffinitySearchWrapper(fa.a, fa.b, txtSearch.Text, rating: r));
				}));
			}
			if (settings.Tumblr.Any()) {
				var t = settings.Tumblr.First();
				var tcf = new TumblrClientFactory();
				listBox1.Items.Add(new ListItem("Tumblr", () => new TumblrSearchWrapper(tcf.Create<TumblrClient>(
					OAuthConsumer.Tumblr.CONSUMER_KEY,
					OAuthConsumer.Tumblr.CONSUMER_SECRET,
					new DontPanic.TumblrSharp.OAuth.Token(t.tokenKey, t.tokenSecret)), txtSearch.Text)));
			}
			if (settings.Inkbunny.Any()) {
				var i = settings.Inkbunny.First();
				listBox1.Items.Add(new ListItem("Inkbunny", () => new InkbunnySearchWrapper(new InkbunnyLib.InkbunnyClient(i.sid, i.userId), txtSearch.Text)));
			}

			for (int i = 0; i < listBox1.Items.Count; i++) {
				listBox1.SetSelected(i, true);
			}
		}

		private IEnumerable<ISiteWrapper> GetSelectedWrappers() {
			foreach (var o in listBox1.SelectedItems) {
				if (o is ListItem l) yield return l.CreateWrapper();
			}
		}

		private void btnSearch_Click(object sender, EventArgs e) {
			var settings = Settings.Load();

			_offset = 0;
			_count = Math.Max(1, (int)numCount.Value);
			_wrapper = new MetaWrapper("All", GetSelectedWrappers());

			Populate();
		}

		private async void Populate() {
			flowLayoutPanel1.Controls.Clear();
			if (_wrapper == null) return;

			var e = await _wrapper.Skip(_offset).Take(_count).GetAsyncEnumeratorAsync();
			while (await e.MoveNextAsync()) {
				var w = e.Current;
				if (w.Adult) {
					if (!chkAdult.Checked) continue;
				} else if (w.Mature) {
					if (!chkMature.Checked) continue;
				} else {
					if (!chkGeneral.Checked) continue;
				}

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
