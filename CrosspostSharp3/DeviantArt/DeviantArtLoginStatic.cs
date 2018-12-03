using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public class DeviantArtLoginException : Exception {
		public DeviantArtLoginException(string message) : base(message) { }
	}

	public static class DeviantArtLoginStatic {
		/// <summary>
		/// Uses the refresh token to "log in" and get a new set of tokens.
		/// </summary>
		/// <param name="refreshToken">An existing refresh token (if any)</param>
		/// <returns>A new refresh token</returns>
		public static async Task<string> UpdateTokens(string clientId, string clientSecret, string refreshToken = null) {
			var result = await DeviantartApi.Login.SetAccessTokenByRefreshAsync(
				clientId,
				clientSecret,
				new Uri("https://www.example.com"),
				refreshToken ?? "",
				null,
				new[] { DeviantartApi.Login.Scope.Browse, DeviantartApi.Login.Scope.User, DeviantartApi.Login.Scope.Stash, DeviantartApi.Login.Scope.Publish, DeviantartApi.Login.Scope.UserManage });

			if (result.IsLoginError) {
				throw new DeviantArtLoginException(result.LoginErrorText);
			}
			return result.RefreshToken;
		}

		public static async Task<bool> LogoutAsync() {
			bool success = true;
			foreach (string token in new[] { DeviantartApi.Requester.AccessToken, DeviantartApi.Requester.RefreshToken }) {
				success = success && await DeviantartApi.Login.LogoutAsync(token);
			}
			return success;
		}
	}
}
