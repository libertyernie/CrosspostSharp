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

		private static SemaphoreSlim _refreshLock = new SemaphoreSlim(1, 1);

		public async Task UpdateTokenAsync(IDeviantArtRefreshToken value) {
			await _refreshLock.WaitAsync();

			try {
				if (!await DeviantArtFs.Requests.Util.Placebo.IsValidAsync(new AccessTokenOnly(AccessToken))) {
					var newCredentials = await DeviantArtAuth.RefreshAsync(_current.RefreshToken);
					_current.AccessToken = newCredentials.AccessToken;
					_current.RefreshToken = newCredentials.RefreshToken;
					_parent.Save();
				}
			} catch (Exception e) {
				Console.Error.WriteLine(e);
			}

			_refreshLock.Release();
		}
	}
}
