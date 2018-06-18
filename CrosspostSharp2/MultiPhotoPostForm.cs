using ArtSourceWrapper;
using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
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
using Tweetinvi;
using Tweetinvi.Models;
using Tweetinvi.Parameters;

namespace CrosspostSharp {
    public partial class MultiPhotoPostForm : Form {
        private ISiteWrapper _wrapper;
        private Settings _settings;
        private List<int> _selectedIds;
        private PictureBox[] _picBoxes;

        public TumblrClient TumblrClient { get; set; }
        public ITwitterCredentials TwitterCredentials { get; set; }

        private IEnumerable<ISubmissionWrapper> GetSelectedSubmissions() {
            foreach (int index in _selectedIds) {
                if (listBox1.Items[index] is ISubmissionWrapper submission) {
                    yield return submission;
                }
            }
        }

        public MultiPhotoPostForm(Settings settings, ISiteWrapper wrapper) {
            InitializeComponent();

            _wrapper = wrapper ?? throw new ArgumentNullException(nameof(wrapper));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
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

        private async void btnPostToTwitter_Click(object sender, EventArgs e) {
            if (TwitterCredentials == null) {
                MessageBox.Show(this, "You are not currently logged into Twitter.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selected = GetSelectedSubmissions().ToList();
            if (selected.Count > 4) {
                MessageBox.Show(this, "CrosspostSharp only supports posting 4 photos at a time.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            string text = txtBody.Text;
            var tags = txtTags.Text.Replace("#", "").Split(' ').Where(s => s != "");
            foreach (string tag in tags) {
                text += " #" + tag;
            }
            
            if (text.Where(c => !char.IsLowSurrogate(c)).Count() > 140) {
                MessageBox.Show("This tweet is over 140 characters. Please shorten it or remove the Weasyl link (if present.)");
                return;
            }


            btnPostToTwitter.Enabled = false;
            try {
                IEnumerable<byte[]> datas = await Task.WhenAll(selected.Select(async w => {
                    var request = WeasylForm.CreateWebRequest(w.ImageURL);
                    using (var response = await request.GetResponseAsync())
                    using (var stream = response.GetResponseStream())
                    using (var ms = new MemoryStream()) {
                        await stream.CopyToAsync(ms);
                        return ms.ToArray();
                    }
                }));

                ITweet tweet = await Task.Run(() => {
                    return Auth.ExecuteOperationWithCredentials(TwitterCredentials, () => {
                        var options = new PublishTweetOptionalParameters {
                            Medias = datas.Select(data => Upload.UploadBinary(data)).ToList()
                        };

                        return Tweet.PublishTweet(text, options);
                    });
                });

                if (tweet == null) {
                    string desc = ExceptionHandler.GetLastException().TwitterDescription;
                    MessageBox.Show(this, "Could not send tweet: " + desc, Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                } else {
                    MessageBox.Show(this, "Tweet ID: " + tweet.IdStr, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, $"{Text}: {ex.GetType().Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                btnPostToTwitter.Enabled = true;
            }
        }

        private async void btnPostToTumblr_Click(object sender, EventArgs e) {
            if (TumblrClient == null) {
                MessageBox.Show(this, "You are not currently logged into Tumblr.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            var selected = GetSelectedSubmissions().ToList();
            if (selected.Count > 4) {
                MessageBox.Show(this, "CrosspostSharp only supports posting 4 photos at a time.", Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                return;
            }

            btnPostToTumblr.Enabled = false;
            try {
                var tags = txtTags.Text.Replace("#", "").Split(' ').Where(s => s != "");
                var imagesToPost = await Task.WhenAll(selected.Select(async w => {
                    var request = WeasylForm.CreateWebRequest(w.ImageURL);
                    using (var response = await request.GetResponseAsync())
                    using (var stream = response.GetResponseStream())
                    using (var ms = new MemoryStream()) {
                        await stream.CopyToAsync(ms);

                        ms.Position = 0;
                        Image image = Image.FromStream(ms);
                        if (_settings.Tumblr.AutoSidePadding && image.Height > image.Width) {
                            return WeasylForm.MakeSquare(image);
                        } else {
                            ms.Position = 0;
                            return new BinaryFile(ms, mimeType: response.ContentType);
                        }
                    }
                }));

                PostData post = PostData.CreatePhoto(
                    imagesToPost.ToArray(),
                    txtBody.Text,
                    null,
                    tags);
                post.Format = PostFormat.Markdown;

                PostCreationInfo info = await TumblrClient.CreatePostAsync(_settings.Tumblr.BlogName, post);

                MessageBox.Show(this, "Post ID: " + info.PostId, Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
            } catch (Exception ex) {
                MessageBox.Show(this, ex.Message, $"{Text}: {ex.GetType().Name}", MessageBoxButtons.OK, MessageBoxIcon.Error);
            } finally {
                btnPostToTumblr.Enabled = true;
            }
        }
    }
}
