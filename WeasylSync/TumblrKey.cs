using DontPanic.TumblrSharp.OAuth;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WinFormsOAuth;

namespace DASync {
	public static class TumblrKey {
		public static Token Obtain(string consumerKey, string consumerSecret) {
			var oauth = new OAuthTumblr(consumerKey, consumerSecret);
			oauth.getRequestToken();
			string verifier = oauth.authorizeToken(); // display WebBrowser
			if (verifier == null) return null;

			string accessToken = oauth.getAccessToken();
			string accessTokenSecret = oauth.TokenSecret;
			return new Token(accessToken, accessTokenSecret);
		}
	}
}
