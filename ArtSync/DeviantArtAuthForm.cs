using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ArtSync {
    public partial class DeviantArtAuthForm : Form {
        public string RedirectUri { get; set; }
        public string Code { get; private set; }
        public string AccessToken { get; private set; }

        public DeviantArtAuthForm() {
            InitializeComponent();
            RedirectUri = "https://www.example.com";

            this.Shown += (o, e) => {
                webBrowser1.Navigate($"https://www.deviantart.com/oauth2/authorize?response_type=code&client_id={OAuthConsumer.DeviantArt.CLIENT_ID}&redirect_uri={WebUtility.UrlEncode(RedirectUri)}");
            };

            Uri ret = new Uri(RedirectUri);
            webBrowser1.Navigated += (o, e) => {
                if (e.Url.Authority == ret.Authority && e.Url.AbsolutePath == ret.AbsolutePath) {
                    int index = e.Url.Query.IndexOf("code=");
                    if (index > -1) {
                        string code = e.Url.Query.Substring(index + 5);
                        if (code.Contains("&")) code = code.Substring(0, code.IndexOf("&"));
                        Code = code;

                        StringBuilder sb = new StringBuilder();
                        sb.Append($"client_id={OAuthConsumer.DeviantArt.CLIENT_ID}");
                        sb.Append('&');
                        sb.Append($"client_secret={OAuthConsumer.DeviantArt.CLIENT_SECRET}");
                        sb.Append('&');
                        sb.Append($"grant_type=authorization_code");
                        sb.Append('&');
                        sb.Append($"code={Code}");
                        sb.Append('&');
                        sb.Append($"redirect_uri={WebUtility.UrlEncode(RedirectUri)}");
                        var req = WebRequest.CreateHttp("https://www.deviantart.com/oauth2/token?" + sb);
                        req.Method = "GET";
                        req.UserAgent = "DASync/0.1 (https://github.com/libertyernie/WeasylSync)";
                        var resp = req.GetResponse();
                        using (var sr = new StreamReader(resp.GetResponseStream())) {
                            Dictionary<string, string> result = JsonConvert.DeserializeObject<Dictionary<string, string>>(sr.ReadToEnd());
                            AccessToken = result["access_token"];
                        }

                        this.Close();
                    }
                }
            };

            webBrowser1.DocumentTitleChanged += (o, e) => {
                this.Text = webBrowser1.DocumentTitle;
            };

            webBrowser1.Navigating += (o, e) => {
                Console.WriteLine(e.Url);
                if (e.Url.OriginalString.StartsWith("javascript:void")) e.Cancel = true;
            };
            webBrowser1.ScriptErrorsSuppressed = false;
        }
    }
}
