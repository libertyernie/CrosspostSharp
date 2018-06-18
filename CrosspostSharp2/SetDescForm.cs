using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ArtSourceWrapper;

namespace CrosspostSharp {
    public partial class SetDescForm : Form {
        public SetDescForm() {
            InitializeComponent();
        }

        public ISubmissionWrapper SubmissionWrapper { get; set; }

        private void SetDescForm_Shown(object sender, EventArgs e) {
            this.txtTitle.Text = SubmissionWrapper.Title;
            this.txtDesc.Text = SubmissionWrapper.HTMLDescription;
            this.txtTags.Text = string.Join(" ", SubmissionWrapper.Tags.Select(s => $"#{s}"));
            this.chkPotentiallySensitive.Checked = SubmissionWrapper.Mature || SubmissionWrapper.Adult;
        }

        private void btnOkay_Click(object sender, EventArgs e) {
            SubmissionWrapper = new MetadataModificationSubmissionWrapper(
                SubmissionWrapper,
                title: txtTitle.Text,
                htmlDescription: txtDesc.Text,
                tags: txtTags.Text.Replace("#", "").Split(' '),
                mature: chkPotentiallySensitive.Checked);
        }
    }
}
