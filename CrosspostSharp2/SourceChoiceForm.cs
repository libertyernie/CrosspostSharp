using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace CrosspostSharp {
    public partial class SourceChoiceForm : Form {
        public ISiteWrapper SelectedWrapper {
            get {
                return comboBox1.SelectedItem as ISiteWrapper;
            }
            set {
                int i = 0;
                foreach (var item in comboBox1.Items) {
                    if ((item as ISiteWrapper)?.WrapperName == value.WrapperName) comboBox1.SelectedIndex = i;
                    i++;
                }
            }
        }

        public SourceChoiceForm(IEnumerable<ISiteWrapper> wrappers) {
            InitializeComponent();

            foreach (var w in wrappers) {
                comboBox1.Items.Add(w);
            }
        }

        private void btnOkay_Click(object sender, EventArgs e) {
            if (this.SelectedWrapper != null) {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
