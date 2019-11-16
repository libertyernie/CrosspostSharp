using System;
using System.Text;
using System.Net;
using System.IO;
using System.Drawing;
using System.Threading.Tasks;

namespace FAWinFormsLogin
{
    class WebHandler
    {
        private CookieContainer cookiesContainer;
        private const string useragent = "Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/51.0.2704.103 Safari/537.36";

        private string refer = "";
        public WebHandler()
        {
            cookiesContainer = new CookieContainer();
        }
        public WebHandler(string cookie)
        {
            cookiesContainer = new CookieContainer();
            setCookies(cookie);
        }

        public void setCookies(string cookie)
        {
            if (cookie != null)
            {
                CookieCollection cookies = new CookieCollection();
                string[] cook = cookie.Split('\n');
                foreach (string cooki in cook)
                {
                    string[] cock = cooki.Split('?');
                    Cookie newcookie = new Cookie(cock[0], cock[1], cock[2], cock[3]);
                    newcookie.Expires = Convert.ToDateTime(cock[4]);

                    cookies.Add(newcookie);
                }
                cookiesContainer.Add(cookies);
            }
        }

        public string getCookies(Uri uri)
        {
            string cookie = "";
            CookieCollection cookies = cookiesContainer.GetCookies(uri);
            foreach (Cookie cock in cookies)
            {
                string domain = cock.Domain;
                string path = cock.Path;
                string name = cock.Name;
                string value = cock.Value;
                string expires = cock.Expires.ToString();
                cookie += name + "?" + value + "?" + path + "?" + domain + "?" + expires + "\n";
            }
            cookie = cookie.TrimEnd('\n');
            return cookie;
        }

        public string getCookie(Uri uri, string name)
        {
            CookieCollection cookies = cookiesContainer.GetCookies(uri);
            return cookies[name].Value;
        }

        public string getPage(string URL, bool decode=true)
        {
            Uri URI = new Uri(URL);
            string result;
            using (WebResponse resp = GET(URI))
            {
                if(resp == null)
                {
                    return "";
                }
                refer = URL;

                var stream = resp.GetResponseStream();
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();

                if(decode)
                    result = WebUtility.HtmlDecode(result);

            }
            return result;
        }

        public async Task<string> getPageAsync(string URL)
        {
            Uri URI = new Uri(URL);
            string result;
            using (WebResponse resp = await GETAsync(URI))
            {
                if(resp == null)
                {
                    return "";
                }
                refer = URL;

                using (var reader = new StreamReader(resp.GetResponseStream()))
                {
                    result = await reader.ReadToEndAsync();
                }

                result = WebUtility.HtmlDecode(result);

            }
            return result;
        }

        public string getPage(string URL, string postData)
        {
            Uri URI = new Uri(URL);
            string result;
            using (WebResponse resp = POST(URI, postData))
            {
                if(resp == null)
                {
                    return "";
                }

                refer = URL;

                var stream = resp.GetResponseStream();
                var reader = new StreamReader(stream);
                result = reader.ReadToEnd();

                result = WebUtility.HtmlDecode(result);
            }
            return result;
        }

        public async Task<string> getPageAsync(string URL, string postData)
        {
            Uri URI = new Uri(URL);
            string result;
            using (WebResponse resp = await POSTAsync(URI, postData))
            {
                if(resp == null)
                {
                    return "";
                }

                refer = URL;
                
                using (var reader = new StreamReader(resp.GetResponseStream()))
                {
                    result = await reader.ReadToEndAsync();
                }

                result = WebUtility.HtmlDecode(result);
            }
            return result;
        }

        public Image getImage(string URL)
        {
            Uri URI = new Uri(URL);
            Image image;
            using (WebResponse resp = GET(URI))
            {
                image = Image.FromStream(resp.GetResponseStream());
            }
            return image;
        }

        public async Task<Image> getImageAsync(string URL)
        {
            Uri URI = new Uri(URL);
            Image image;
            using (WebResponse resp = await GETAsync(URI))
            {
                using (var stream = resp.GetResponseStream())
                {
                    DateTime dt = DateTime.Now;
                    image = Image.FromStream(stream);
                    Console.WriteLine(DateTime.Now - dt);
                }
            }
            return image;
        }

        public byte[] getBinary(string URL)
        {
            Uri URI = new Uri(URL);
            byte[] result;
            byte[] buffer = new byte[4096];
            using (WebResponse resp = GET(URI))
            {

                using (Stream streamReader = resp.GetResponseStream())
                {
                    using (MemoryStream stream = new MemoryStream())
                    {
                        int count = 0;
                        do
                        {
                            count = streamReader.Read(buffer, 0, buffer.Length);
                            stream.Write(buffer, 0, count);
                        } while (count != 0);

                        result = stream.ToArray();
                    }
                }
            }
            return result;
        }



        private WebResponse POST(Uri URL, string postData)
        {

            var request = (HttpWebRequest)WebRequest.Create(URL);

            var data = Encoding.UTF8.GetBytes(postData);

            request.UserAgent = useragent;
            request.CookieContainer = cookiesContainer;


            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.Referer = refer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = request.GetRequestStream())
            {
                stream.Write(data, 0, data.Length);
            }
            HttpWebResponse response = (HttpWebResponse)request.GetResponse();

            if ((int)response.StatusCode == 302)
            {
                // Get login cookies
                string responseHeaderLocation = response.Headers["Location"];
                Uri loc;
                // Handle relative vs absolute paths
                try
                {
                    loc = new Uri(responseHeaderLocation);
                }
                catch (UriFormatException)
                {
                    loc = new Uri(URL, responseHeaderLocation);
                }

                response = GET(loc);

            }

            return response;
        }

        private async Task<WebResponse> POSTAsync(Uri URL, string postData) {

            var request = (HttpWebRequest)WebRequest.Create(URL);

            var data = Encoding.UTF8.GetBytes(postData);

            request.UserAgent = useragent;
            request.CookieContainer = cookiesContainer;


            request.Method = "POST";
            request.AllowAutoRedirect = false;
            request.Referer = refer;
            request.ContentType = "application/x-www-form-urlencoded";
            request.ContentLength = data.Length;

            using (var stream = await request.GetRequestStreamAsync()) {
                await stream.WriteAsync(data, 0, data.Length);
            }
            HttpWebResponse response = (HttpWebResponse)(await request.GetResponseAsync());

            if ((int)response.StatusCode == 302) {
                // Get login cookies
                string responseHeaderLocation = response.Headers["Location"];
                Uri loc;
                // Handle relative vs absolute paths
                try {
                    loc = new Uri(responseHeaderLocation);
                } catch (UriFormatException) {
                    loc = new Uri(URL, responseHeaderLocation);
                }

                response = await GETAsync(loc);

            }

            return response;
        }

        private HttpWebResponse GET(Uri URL)
        {
            HttpWebResponse resp = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.UserAgent = useragent;
                request.CookieContainer = cookiesContainer;
                request.Referer = refer;
                request.KeepAlive = true;
                request.Timeout = 10000;

                // var fi = new FileInfo(URL.AbsolutePath);
                // var ext = fi.Extension;

                // if (!string.IsNullOrWhiteSpace(ext))
                // {
                //     request.Accept = "image/webp,image/*,*/*;q=0.8";
                // }


                resp = (HttpWebResponse)request.GetResponse();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return resp;
        }

        private async Task<HttpWebResponse> GETAsync(Uri URL)
        {
            HttpWebResponse resp = null;
            try
            {
                var request = (HttpWebRequest)WebRequest.Create(URL);
                request.UserAgent = useragent;
                request.CookieContainer = cookiesContainer;
                request.Referer = refer;
                request.KeepAlive = true;
                request.Timeout = 10000;

                // var fi = new FileInfo(URL.AbsolutePath);
                // var ext = fi.Extension;

                // if (!string.IsNullOrWhiteSpace(ext))
                // {
                //     request.Accept = "image/webp,image/*,*/*;q=0.8";
                // }


                resp = (HttpWebResponse)(await request.GetResponseAsync());
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            return resp;
        }



    }
}
