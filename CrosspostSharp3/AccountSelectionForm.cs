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
		private Func<Task<IEnumerable<T>>> OnAdd;
		private Action<T> OnRemove;

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
			Func<Task<IEnumerable<T>>> onAdd,
			Action<T> onRemove = null
		) {
			InitializeComponent();
			foreach (var o in initialList) {
				listBox1.Items.Add(o);
			}
			OnAdd = onAdd;
			OnRemove = onRemove;
		}

		public AccountSelectionForm(
			IEnumerable<T> initialList,
			Func<IEnumerable<T>> onAdd,
			Action<T> onRemove = null
		) : this(initialList, async () => onAdd(), onRemove) { }

		private void Remove_Click(object sender, EventArgs e) {
			btnAdd.Enabled = btnRemove.Enabled = btnOk.Enabled = false;
			try {
				var obj = listBox1.SelectedItem as T;
				if (obj != null) {
					if (MessageBox.Show(this, $"Are you sure you want to remove {obj.Username} form your list of accounts?", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
						OnRemove?.Invoke(obj);
						listBox1.Items.Remove(obj);
					}
				}
			} catch (Exception ex) {
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btnAdd.Enabled = btnRemove.Enabled = btnOk.Enabled = true;
		}

		private async void btnAdd_Click(object sender, EventArgs e) {
			btnAdd.Enabled = btnRemove.Enabled = btnOk.Enabled = false;
			try {
				var list = await OnAdd();
				foreach (var o in list) listBox1.Items.Add(o);
			} catch (Exception ex) {
				if (ex is System.Net.WebException w) {
					using (var s = w.Response.GetResponseStream())
					using (var sr = new System.IO.StreamReader(s)) {
						Console.WriteLine(await sr.ReadToEndAsync());
					}
				}
				MessageBox.Show(this, ex.Message, ex.GetType().Name, MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			btnAdd.Enabled = btnRemove.Enabled = btnOk.Enabled = true;
		}
	}
}
