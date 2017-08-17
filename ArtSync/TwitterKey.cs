using ISchemm.WinFormsOAuth;
using Tweetinvi;
using Tweetinvi.Models;

namespace ArtSync {
	public static class TwitterKey {
		public static TwitterCredentials Obtain(string consumerKey, string consumerSecret) {
			var oauth = new OAuthTwitter(consumerKey, consumerSecret);
			oauth.getRequestToken();
			string verifier = oauth.authorizeToken(); // display WebBrowser
			if (verifier == null) return null;

			string accessToken = oauth.getAccessToken();
			string accessTokenSecret = oauth.TokenSecret;
			return new TwitterCredentials(consumerKey, consumerSecret, accessToken, accessTokenSecret);
		}

		public static string GetScreenName(this ITwitterCredentials credentials) {
			return credentials == null
				? null
				: Auth.ExecuteOperationWithCredentials(credentials, () => User.GetAuthenticatedUser())?.ScreenName;
		}
	}
}
