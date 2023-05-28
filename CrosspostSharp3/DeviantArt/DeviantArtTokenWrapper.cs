using DeviantArtFs;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtTokenWrapper : IDeviantArtRefreshableAccessToken {
		private readonly Settings _parent;
		private Settings.DeviantArtAccountSettings _current;

		public DeviantArtTokenWrapper(Settings parent, Settings.DeviantArtAccountSettings current) {
			_parent = parent;
			_current = current;
		}

		public DeviantArtApp App =>
			new DeviantArtApp(OAuthConsumer.DeviantArt.CLIENT_ID.ToString(), OAuthConsumer.DeviantArt.CLIENT_SECRET);

		public string RefreshToken => _current.RefreshToken;
		public string AccessToken => _current.AccessToken;
		public string Username => _current.Username;

		private class AccessTokenOnly : IDeviantArtAccessToken {
			public string AccessToken { get; private set; }

			public AccessTokenOnly(string token) {
				AccessToken = token;
			}
		}

		async Task IDeviantArtRefreshableAccessToken.RefreshAccessTokenAsync() {
			var resp = await DeviantArtAuth.RefreshAsync(App, RefreshToken);
			_current.AccessToken = resp.access_token;
			_current.RefreshToken = resp.refresh_token;
			_parent.Save();
		}
	}
}
