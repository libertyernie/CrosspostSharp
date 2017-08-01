using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class DeviantArtException : Exception {
        public DeviantArtException(string message) : base(message) { }
    }

    public class DeviantArtWrapper : IWrapper {
        private string _clientId, _clientSecret;
        private bool _initialLogin;

        public DeviantArtWrapper(string clientId, string clientSecret) {
            _clientId = clientId;
            _clientSecret = clientSecret;
            _initialLogin = false;
        }

        /// <summary>
        /// Uses the refresh token to "log in" and get a new set of tokens. If the refresh token is null, invalid, or not provided, a login window will be shown.
        /// </summary>
        /// <param name="refreshToken">An existing refresh token (if any)</param>
        /// <returns>A new refresh token</returns>
        public async Task<string> UpdateTokens(string refreshToken = null) {
            _initialLogin = true;
            var result = await DeviantartApi.Login.SetAccessTokenByRefreshAsync(_clientId, _clientSecret, "https://www.example.com", refreshToken ?? "", null);
            if (result.IsLoginError) {
                throw new DeviantArtException(result.LoginErrorText);
            }
            return result.RefreshToken;
        }

        public async Task<string> Whoami() {
            if (!_initialLogin) await UpdateTokens();

            var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
            if (result.IsError) {
                throw new DeviantArtException(result.ErrorText);
            }
            return result.Object.Username;
        }

        public async Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            if (!_initialLogin) await UpdateTokens();

            return new UpdateGalleryResult {
                Submissions = new List<ISubmissionWrapper>(0)
            };
        }

        public Task<UpdateGalleryResult> NextPage() {
            throw new NotImplementedException();
        }

        public Task<UpdateGalleryResult> PreviousPage() {
            throw new NotImplementedException();
        }
    }
}
