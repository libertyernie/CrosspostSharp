using DontPanic.TumblrSharp.Client;
using FurryNetworkLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3.FurryNetwork {
	public partial class FurryNetworkCharacterSelectionForm : Form {
		public IEnumerable<Character> SelectedItems {
			get {
				foreach (object o in listBox1.SelectedItems) {
					if (o is Character i) yield return i;
				}
			}
		}

		public FurryNetworkCharacterSelectionForm(
			IEnumerable<Character> list
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
