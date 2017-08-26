using ArtSourceWrapper;
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
    public partial class SourceChoiceForm : Form {
        public IEnumerable<ISiteWrapper> Wrappers { get; private set; }
        
        public ISiteWrapper SelectedWrapper { get; set; }

        public SourceChoiceForm(IEnumerable<ISiteWrapper> wrappers) {
            InitializeComponent();

            this.Wrappers = wrappers;
            this.SelectedWrapper = null;

            this.Height = 91 + 23 * this.Wrappers.Count();
        }

        private void btnOkay_Click(object sender, EventArgs e) {
            if (this.SelectedWrapper != null) {
                this.DialogResult = DialogResult.OK;
            }
        }

        private void SourceChoiceForm_Load(object sender, EventArgs e) {
            foreach (var w in this.Wrappers) {
                var radioButton = new RadioButton {
                    Text = w.WrapperName,
                    Checked = w.WrapperName == SelectedWrapper?.WrapperName,
                    AutoSize = true
                };
                if (radioButton.Checked) SelectedWrapper = w;
                radioButton.CheckedChanged += (x, y) => {
                    if (radioButton.Checked) this.SelectedWrapper = w;
                };
                this.flowLayoutPanel1.Controls.Add(radioButton);
            }
        }
    }
}
