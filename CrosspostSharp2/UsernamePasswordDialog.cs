using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp {
	public partial class UsernamePasswordDialog : Form {
		public string Username => txtUsername.Text;
		public string Password => txtPassword.Text;

		public string UsernameLabel {
			get {
				return txtUsername.Text;
			}
			set {
				txtUsername.Text = value;
			}
		}

		public UsernamePasswordDialog() {
			InitializeComponent();
		}
	}
}
