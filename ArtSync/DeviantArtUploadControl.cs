using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
    public partial class DeviantArtUploadControl : UserControl {
        public DeviantArtUploadControl() {
            InitializeComponent();

            radNone.CheckedChanged += MatureChanged;
            radModerate.CheckedChanged += MatureChanged;
            radStrict.CheckedChanged += MatureChanged;
        }

        private void MatureChanged(object sender, EventArgs e) {
            grpMatureClassification.Enabled != radNone.Checked;
        }
    }
}
