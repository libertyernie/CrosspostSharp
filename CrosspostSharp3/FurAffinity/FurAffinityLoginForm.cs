using System;
using System.Linq;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class FurAffinityLoginForm : Form {
		public string ACookie { get; private set; } = null;
		public string BCookie { get; private set; } = null;

		public FurAffinityLoginForm() {
			InitializeComponent();
		}

		private void FurAffinityLoginForm_Shown(object sender, EventArgs e) {
			webBrowser1.Navigate("https://www.furaffinity.net/msg/others/");
		}

		private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
			Text = webBrowser1.Document?.Title ?? "Log In";

			var cks = CppCookieTools.Cookies.GetCookies("https://www.furaffinity.net");
			var dict = cks.ToDictionary(x => x.Name, x => x.Value);
			if (dict.ContainsKey("a") && dict.ContainsKey("b")) {
				ACookie = dict["a"];
				BCookie = dict["b"];
				DialogResult = DialogResult.OK;
			}
		}
	}
}
