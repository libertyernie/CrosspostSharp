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

namespace WeasylSync {
	public static class TwitterKey {
		public static Token Obtain(string consumerKey, string consumerSecret) {
			var oauth = new OAuthTwitter(OAuthConsumer.Twitter.CONSUMER_KEY, OAuthConsumer.Twitter.CONSUMER_SECRET);
			oauth.getRequestToken();
			string verifier = oauth.authorizeToken(); // display WebBrowser
			if (verifier == null) return null;

			string accessToken = oauth.getAccessToken();
			string accessTokenSecret = oauth.TokenSecret;
			return new Token(accessToken, accessTokenSecret);
		}
	}
}
