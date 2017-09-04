using ISchemm.WinFormsOAuth;
using static ArtSync.Settings;

namespace ArtSync {
	public static class FlickrKey {
        public static FlickrSettings Obtain(string consumerKey, string consumerSecret) {
            IECompatibility.SetForCurrentProcess();
            
            var oauth = new OAuthFlickr(consumerKey, consumerSecret);
			oauth.getRequestToken();
			string verifier = oauth.authorizeToken(); // display WebBrowser
			if (verifier == null) return null;

			string accessToken = oauth.getAccessToken();
			string accessTokenSecret = oauth.TokenSecret;
            return new FlickrSettings {
                TokenKey = accessToken,
                TokenSecret = accessTokenSecret
            };
        }
	}
}
