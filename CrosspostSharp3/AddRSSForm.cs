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
	public partial class AddRSSForm : Form {
		public string FeedLabel => txtLabel.Text;
		public string FeedUrl => txtUrl.Text;

		public AddRSSForm() {
			InitializeComponent();
		}
	}
}
