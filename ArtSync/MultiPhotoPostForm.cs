using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
    public partial class MultiPhotoPostForm : Form {
        private ISiteWrapper _wrapper;
        private ISubmissionWrapper[] _submissions;
        private PictureBox[] _picBoxes;

        private IEnumerable<ISubmissionWrapper> GetSelectedSubmissions() {
            foreach (ISubmissionWrapper submission in listBox1.SelectedItems) {
                yield return submission;
            }
        }

        public MultiPhotoPostForm(ISiteWrapper wrapper) {
            InitializeComponent();

            _wrapper = wrapper;
            foreach (var w in wrapper.Cache) {
                listBox1.Items.Add(w);
            }

            _picBoxes = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4 };
            _submissions = new ISubmissionWrapper[_picBoxes.Length];
            
            for (int i = 0; i < _picBoxes.Length; i++) {
                int j = i;
                _picBoxes[j].Click += (o, e) => {
                    string url = _submissions[j]?.ViewURL;
                    if (url != null) Process.Start(url);
                };
            }
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            var selected = GetSelectedSubmissions().Take(4).ToList();
            for (int i = 0; i < _picBoxes.Length; i++) {
                if (i < selected.Count) {
                    _submissions[i] = selected[i];
                    _picBoxes[i].ImageLocation = selected[i].ThumbnailURL;
                    _picBoxes[i].Cursor = Cursors.Hand;
                } else {
                    _submissions[i] = null;
                    _picBoxes[i].ImageLocation = null;
                    _picBoxes[i].Cursor = Cursors.Default;
                }
            }
        }

        private void txtBody_TextChanged(object sender, EventArgs e) {
            lblTweetLength.Text = $"{txtBody.Text.Where(c => !char.IsLowSurrogate(c)).Count()}/140";
        }
    }
}
