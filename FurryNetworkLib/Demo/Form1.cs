using FurryNetworkLib;
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

namespace Demo {
    public partial class Form1 : Form {
        private FurryNetworkClient _client;

        public Form1() {
            InitializeComponent();
        }

        private async void btnLogin_Click(object sender, EventArgs e) {
            try {
                _client = await FurryNetworkClient.LoginAsync(textBox1.Text, textBox2.Text);
                btnGetUser.Enabled = btnLogOut.Enabled = true;
            } catch (WebException ex) {
                using (var sr = new StreamReader(ex.Response.GetResponseStream())) {
                    string text = await sr.ReadToEndAsync();
                    throw new Exception(ex.Message + "/" + text, ex);
                }
            }
        }

        private async void btnGetUser_Click(object sender, EventArgs e) {
            btnGetUser.Enabled = false;
            using (var f = new Form()) {
                var grid = new PropertyGrid { Dock = DockStyle.Fill };
                f.Controls.Add(grid);

                grid.SelectedObject = await _client.GetUserAsync();

                f.ShowDialog();
            }
            btnGetUser.Enabled = true;
        }

        private async void btnLogOut_Click(object sender, EventArgs e) {
            await _client.LogoutAsync();
            _client = null;
            btnGetUser.Enabled = btnLogOut.Enabled = false;
        }

        private async void btnShowMostRecentArtwork_Click(object sender, EventArgs e) {
            var user = await _client.GetUserAsync();
            var searchResults = await _client.SearchByCharacterAsync(user.DefaultCharacter.Name, new[] { "artwork" });
            foreach (var submission in searchResults.Hits.Select(h => h.Submission)) {
                if (submission is Artwork a) {
                    Process.Start(a.Images.Original);
                    Process.Start($"https://beta.furrynetwork.com/artwork/{a.Id}");
                    return;
                }
            }
            MessageBox.Show(this, "No search results found.");
        }
    }
}
