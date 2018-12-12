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
	public partial class MastodonLoginDialog : Form {
		public string Instance => comboBox1.Text;
		public string Email => txtEmail.Text;
		public string Password => txtPassword.Text;

		//public Uri Uri =>
		//	Uri.TryCreate(Instance, UriKind.Absolute, out Uri u1) ? u1
		//	: Uri.TryCreate("https://" + Instance, UriKind.Absolute, out Uri u2) ? u2
		//	: throw new FormatException("The instance entered is not a valid URI.");

		public MastodonLoginDialog() {
			InitializeComponent();
		}
	}
}
