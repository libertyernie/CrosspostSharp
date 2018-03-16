using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class PinterestAppForm : Form {
		public string AppId => txtAppId.Text;
		public string AppSecret => txtAppSecret.Text;

		public PinterestAppForm() {
			InitializeComponent();
		}
	}
}
