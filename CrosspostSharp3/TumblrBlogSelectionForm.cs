using DontPanic.TumblrSharp.Client;
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
	public partial class TumblrBlogSelectionForm : Form {
		public BlogInfo SelectedItem => listBox1.SelectedItem as BlogInfo;

		public TumblrBlogSelectionForm(
			IEnumerable<UserBlogInfo> list
		) {
			InitializeComponent();
			foreach (var o in list) {
				listBox1.Items.Add(o);
			}
			if (listBox1.Items.Count > 0) {
				listBox1.SelectedIndex = 0;
			}
		}
	}
}
