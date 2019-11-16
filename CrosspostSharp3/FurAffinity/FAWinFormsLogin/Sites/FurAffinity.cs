using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace FAWinFormsLogin.Sites
{
    public class FurAffinity
    {
        private const string FABase = "https://www.furaffinity.net/";
        private const string FALoginPage = "https://www.furaffinity.net/login/";

        private WebHandler webHandler = new WebHandler();

        public string getCookies() {
            Uri fa = new Uri(FABase);
            string cookies = webHandler.getCookies(fa);
            return cookies;
        }

        public async Task<Image> GetCaptchaAsync()
        {
            string html = webHandler.getPage(FALoginPage);

            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            doc.LoadHtml(html);
            string captchaLink = doc.GetElementbyId("captcha_img").Attributes["src"].Value;

            return await webHandler.getImageAsync(FABase + captchaLink);
        }

        public struct FACookies {
            public string b, a;
        }

        public async Task<FACookies?> loginAsync(string username, string password, string captcha)
        {
            string postData = string.Format("action=login&name={0}&pass={1}&g-recaptcha-response=&use_old_captcha=1&captcha={2}&login={3}",
                username,
                password,
                captcha,
                "Login to%C2%A0FurAffinity");

            await webHandler.getPageAsync(FALoginPage + "?ref=https://furaffinity.net/", postData);

            if (await isLoggedInAsync())
            {
                Uri uri = new Uri(FABase);
                return new FACookies {
                    b = webHandler.getCookie(uri, "b"),
                    a = webHandler.getCookie(uri, "a")
                };
            }
            return null;
        }

        private async Task<bool> isLoggedInAsync()
        {
            HtmlAgilityPack.HtmlDocument doc = new HtmlAgilityPack.HtmlDocument();
            string web = await webHandler.getPageAsync(FABase);
            doc.LoadHtml(web);
            HtmlNode node = doc.DocumentNode.SelectSingleNode("//a[@href='/submit/']");

            if (node == null)
            {
                return false;
            }

            string cookies = getCookies();

            return true;
        }
    }
}
