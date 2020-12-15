using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CrosspostSharp3 {
	public partial class FurAffinityLoginForm : Form {
		public string ACookie { get; private set; } = null;
		public string BCookie { get; private set; } = null;

		public FurAffinityLoginForm() {
			InitializeComponent();
		}

		[DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetGetCookieEx(
			string url,
			string cookieName,
			StringBuilder cookieData,
			ref int size,
			int flags,
			IntPtr pReserved);

		private const int INTERNET_COOKIE_HTTPONLY = 0x00002000;

		public static string GetCookie(string url, string name) {
			int size = 512;
			StringBuilder sb = new StringBuilder(size);
			if (!InternetGetCookieEx(url, name, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero)) {
				if (size < 0) {
					return null;
				}
				sb = new StringBuilder(size);
				if (!InternetGetCookieEx(url, name, sb, ref size, INTERNET_COOKIE_HTTPONLY, IntPtr.Zero)) {
					return null;
				}
			}
			return sb.ToString();
		}

		private void FurAffinityLoginForm_Shown(object sender, EventArgs e) {
			webBrowser1.Navigate("https://www.furaffinity.net/msg/others/");
		}

		private void webBrowser1_Navigated(object sender, WebBrowserNavigatedEventArgs e) {
			Text = webBrowser1.Document?.Title ?? "Log In";

			string a = GetCookie("https://www.furaffinity.net", "a");
			string b = GetCookie("https://www.furaffinity.net", "b");
			if (a?.StartsWith("a=") == true && b?.StartsWith("b=") == true) {
				ACookie = a.Split('=')[1];
				BCookie = b.Split('=')[1];
				DialogResult = DialogResult.OK;
			}
		}
	}
}
