using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
    public partial class DeviantArtCategoryBrowser : Form {
        public string SelectedPath => treeView1.SelectedNode.Name;

        public DeviantArtCategoryBrowser() {
            InitializeComponent();
        }

        private async Task Populate(TreeNodeCollection nodes, string path = null) {
            var result = await new DeviantartApi.Requests.Browse.CategorytreeRequest {
                Catpath = path ?? "/"
            }.ExecuteAsync();
            if (result.IsError) {
                throw new Exception(result.ErrorText);
            }
            if (!string.IsNullOrEmpty(result.Object.Error)) {
                throw new Exception(result.Object.ErrorDescription);
            }
            foreach (var c in result.Object.Categories) {
                TreeNode node = nodes.Add(c.Catpath, c.Title);
                if (c.HasSubcategory) {
                    node.Nodes.Add("loading", "Loading...");
                }
            }
        }

        private async void DeviantArtCategoryBrowser_Load(object sender, EventArgs e) {
            try {
                treeView1.Nodes.Add("", "None");
                await Populate(treeView1.Nodes);
            } catch (Exception ex) {
                MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType().Name}: {ex.GetType().Name}");
            }
        }

        private async void treeView1_BeforeExpand(object sender, TreeViewCancelEventArgs e) {
            try {
                TreeNode target = e.Node;
                if (target.Nodes.ContainsKey("loading")) {
                    target.Nodes.Clear();
                    await Populate(target.Nodes, target.Name);
                    target.Expand();
                }
            } catch (Exception ex) {
                MessageBox.Show(this.ParentForm, ex.Message, $"{this.GetType().Name}: {ex.GetType().Name}");
            }
        }
    }
}
