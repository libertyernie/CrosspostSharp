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
	public partial class FurAffinityLoginForm : Form {
		public string ACookie => textBox1.Text;
		public string BCookie => textBox2.Text;

		public FurAffinityLoginForm() {
			InitializeComponent();
		}
	}
}
