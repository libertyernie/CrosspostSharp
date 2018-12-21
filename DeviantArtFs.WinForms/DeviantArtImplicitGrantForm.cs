using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DeviantArtFs.WinForms {
	public class DeviantArtImplicitGrantForm : Form {
		[DllImport("wininet.dll", SetLastError = true)]
		private static extern bool InternetSetOption(IntPtr hInternet, int dwOption, IntPtr lpBuffer, int lpdwBufferLength);

		private readonly string _state;

		public string AccessToken { get; private set; }
		public DateTimeOffset? ExpiresAt { get; private set; }

		public DeviantArtImplicitGrantForm(string clientId, Uri callbackUrl, IEnumerable<string> scopes = null) {
			_state = Guid.NewGuid().ToString();

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
				sb.Append($"response_type=token&");
				sb.Append($"state={WebUtility.UrlEncode(_state)}&");
				sb.Append($"client_id={clientId}&");
				sb.Append($"redirect_uri={callbackUrl}");
				if (scopes != null) {
					sb.Append($"&scope={WebUtility.UrlEncode(string.Join(" ", scopes))}");
				}
				webBrowser1.Navigate("https://www.deviantart.com/oauth2/authorize?" + sb);
			};

			webBrowser1.Navigated += (o, e) => {
				if (e.Url.Authority == callbackUrl.Authority && e.Url.AbsolutePath == callbackUrl.AbsolutePath) {
					var psd = QueryHelpers.ParseQuery(e.Url.Fragment.Substring(1));
					if (!psd.TryGetValue("access_token", out StringValues access_token)) return;
					if (!psd.TryGetValue("token_type", out StringValues token_type)) return;
					if (!psd.TryGetValue("state", out StringValues state)) return;
					if (state == _state && token_type == "bearer") {
						AccessToken = access_token;
						if (psd.TryGetValue("expires_in", out StringValues expires_in)) {
							if (double.TryParse(expires_in, out double expsec)) {
								ExpiresAt = DateTimeOffset.Now.AddSeconds(expsec);
							}
						}
						webBrowser1.Navigate("about:blank");
					}
				} else if (e.Url.AbsoluteUri == "about:blank") {
					DialogResult = DialogResult.OK;
				}
			};

			webBrowser1.NewWindow += (o, e) => {

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
