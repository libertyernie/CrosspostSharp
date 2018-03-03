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
	public partial class AccountSelectionForm<T> : Form where T : Settings.AccountCredentials {
		private Func<Task<T>> OnAdd;
		private Func<T, Task> OnRemove;

		public IEnumerable<T> CurrentList {
			get {
				foreach (var item in listBox1.Items) {
					if (item is T o) {
						yield return o;
					}
				}
			}
		}

		public AccountSelectionForm(
			IEnumerable<T> initialList,
			Func<Task<T>> onAdd,
			Func<T, Task> onRemove = null
		) {
			InitializeComponent();
			foreach (var o in initialList) {
				listBox1.Items.Add(o);
			}
			OnAdd = onAdd;
			OnRemove = onRemove;
		}

		private async void Remove_Click(object sender, EventArgs e) {
			btnAdd.Enabled = btnRemove.Enabled = false;
			try {
				var obj = listBox1.SelectedItem as T;
				if (obj != null) {
					if (MessageBox.Show(this, $"Are you sure you want to remove {obj.Username} form your list of accounts?", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
						if (OnRemove != null) await OnRemove(obj);
						listBox1.Items.Remove(obj);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btnAdd.Enabled = btnRemove.Enabled = true;
		}

		private async void btnAdd_Click(object sender, EventArgs e) {
			btnAdd.Enabled = btnRemove.Enabled = false;
			try {
				var o = await OnAdd();
				if (o != null) listBox1.Items.Add(o);
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btnAdd.Enabled = btnRemove.Enabled = true;
		}
	}
}
