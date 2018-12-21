using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviantArtFs.WinForms {
	public class DeviantArtAuthorizationCodeForm : Form {
		[DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

		public string Code { get; private set; }

		public DeviantArtAuthorizationCodeForm(int clientId, Uri callbackUrl, IEnumerable<string> scopes = null) {
			this.Width = 322;
			this.Height = 573;

			InternetSetOption(IntPtr.Zero, 42, IntPtr.Zero, 0);

			var webBrowser1 = new WebBrowser {
				Dock = DockStyle.Fill,
				ScriptErrorsSuppressed = true
			};
			this.Controls.Add(webBrowser1);

			this.Shown += (o, e) => {
				StringBuilder sb = new StringBuilder();
				sb.Append($"response_type=code&");
				sb.Append($"client_id={clientId}&");
				sb.Append($"redirect_uri={callbackUrl}");
				if (scopes != null) {
					sb.Append($"&scope={WebUtility.UrlEncode(string.Join(" ", scopes))}");
				}
				webBrowser1.Navigate("https://www.deviantart.com/oauth2/authorize?" + sb);
			};

			webBrowser1.Navigated += (o, e) => {
				if (e.Url.Authority == callbackUrl.Authority && e.Url.AbsolutePath == callbackUrl.AbsolutePath) {
					int codeIndex = e.Url.Query.IndexOf("code=");
					if (codeIndex > -1) {
						string code = e.Url.Query.Substring(codeIndex + 5);
						if (code.Contains("&")) code = code.Substring(0, code.IndexOf("&"));
						Code = code;
						DialogResult = DialogResult.OK;
					}
				}
			};

			webBrowser1.DocumentTitleChanged += (o, e) => {
				this.Text = webBrowser1.DocumentTitle;
			};

			webBrowser1.Navigating += (o, e) => {
				if (e.Url.OriginalString.StartsWith("javascript:void")) e.Cancel = true;
			};
		}
	}
}
