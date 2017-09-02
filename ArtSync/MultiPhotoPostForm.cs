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

        private List<int> _selectedIds;
        
        private PictureBox[] _picBoxes;

        private IEnumerable<ISubmissionWrapper> GetSelectedSubmissions() {
            foreach (int index in _selectedIds) {
                if (listBox1.Items[index] is ISubmissionWrapper submission) {
                    yield return submission;
                }
            }
        }

        public MultiPhotoPostForm(ISiteWrapper wrapper) {
            InitializeComponent();

            _wrapper = wrapper;
            RepopulateList();

            _selectedIds = new List<int>();

            _picBoxes = new[] { pictureBox1, pictureBox2, pictureBox3, pictureBox4 };
            
            for (int i = 0; i < _picBoxes.Length; i++) {
                int j = i;
                _picBoxes[j].Click += (o, e) => {
                    ISubmissionWrapper submission = listBox1.Items[j] as ISubmissionWrapper;
                    string url = submission?.ViewURL;
                    if (url != null) Process.Start(url);
                };
            }
        }

        private void RepopulateList() {
            listBox1.Items.Clear();
            foreach (var w in _wrapper.Cache) {
                listBox1.Items.Add(w);
            }
            btnLoadMore.Enabled = !_wrapper.IsEnded;
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e) {
            foreach (int index in listBox1.SelectedIndices) {
                if (!_selectedIds.Contains(index)) _selectedIds.Add(index);
            }
            foreach (int index in _selectedIds.ToList()) {
                if (!listBox1.SelectedIndices.Contains(index)) _selectedIds.Remove(index);
            }

            var selected = GetSelectedSubmissions().Take(4).ToList();
            for (int i = 0; i < _picBoxes.Length; i++) {
                if (i < selected.Count) {
                    _picBoxes[i].ImageLocation = selected[i].ThumbnailURL;
                    _picBoxes[i].Cursor = Cursors.Hand;
                } else {
                    _picBoxes[i].ImageLocation = null;
                    _picBoxes[i].Cursor = Cursors.Default;
                }
            }
        }

        private void txtBody_TextChanged(object sender, EventArgs e) {
            lblTweetLength.Text = $"{txtBody.Text.Where(c => !char.IsLowSurrogate(c)).Count()}/140";
        }

        private async void btnLoadMore_Click(object sender, EventArgs e) {
            btnLoadMore.Enabled = false;
            await _wrapper.FetchAsync();
            RepopulateList();
        }
    }
}
