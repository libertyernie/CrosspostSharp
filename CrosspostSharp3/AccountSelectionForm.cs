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
	public partial class AccountSelectionForm<T> : Form where T : Settings.IAccountCredentials {
		private class MenuItem {
			public string Username => Account.Username ?? Account.ToString();
			public T Account;

			public MenuItem(T account) {
				Account = account;
			}
		}

		private Func<Task<IEnumerable<T>>> OnAdd;
		private Action<T> OnRemove;

		public IEnumerable<T> CurrentList {
			get {
				foreach (var item in listBox1.Items) {
					if (item is MenuItem o) {
						yield return o.Account;
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
				listBox1.Items.Add(new MenuItem(o));
			}
			OnAdd = onAdd;
			OnRemove = onRemove;
		}

		public AccountSelectionForm(
			IEnumerable<T> initialList,
			Func<IEnumerable<T>> onAdd,
			Action<T> onRemove = null
		) : this(initialList, () => Task.FromResult(onAdd()), onRemove) { }

		private void Remove_Click(object sender, EventArgs e) {
			btnAdd.Enabled = btnRemove.Enabled = btnOk.Enabled = false;
			try {
				var obj = listBox1.SelectedItem as MenuItem;
				if (obj != null) {
					if (MessageBox.Show(this, $"Are you sure you want to remove {obj.Username} form your list of accounts?", Text, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK) {
						try {
							OnRemove?.Invoke(obj.Account);
						} catch (Exception ex) {
							Console.Error.WriteLine(ex);
						}
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
				foreach (var o in list) listBox1.Items.Add(new MenuItem(o));
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
