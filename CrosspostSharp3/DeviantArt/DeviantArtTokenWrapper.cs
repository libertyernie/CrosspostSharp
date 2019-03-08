using DeviantArtFs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtTokenWrapper : IDeviantArtAutomaticRefreshToken {
		private readonly Settings _parent;
		private Settings.DeviantArtAccountSettings _current;

		public DeviantArtTokenWrapper(Settings parent, Settings.DeviantArtAccountSettings current) {
			_parent = parent;
			_current = current;
		}

		public IDeviantArtAuth DeviantArtAuth =>
			new DeviantArtAuth(OAuthConsumer.DeviantArt.CLIENT_ID, OAuthConsumer.DeviantArt.CLIENT_SECRET);

		public string RefreshToken => _current.RefreshToken;
		public string AccessToken => _current.AccessToken;
		public string Username => _current.Username;

		private class AccessTokenOnly : IDeviantArtAccessToken {
			public string AccessToken { get; private set; }

			public AccessTokenOnly(string token) {
				AccessToken = token;
			}
		}

		public Task UpdateTokenAsync(IDeviantArtRefreshToken value) {
			_current.AccessToken = value.AccessToken;
			_current.RefreshToken = value.RefreshToken;
			_parent.Save();
			return Task.CompletedTask;
		}
	}
}
