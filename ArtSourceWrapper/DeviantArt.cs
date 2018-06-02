using DeviantartApi.Objects;
using DeviantartApi.Objects.SubObjects.DeviationMetadata;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class DeviantArtException : Exception {
        public DeviantArtException(string message) : base(message) { }
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
                throw new DeviantArtException(result.LoginErrorText);
            }
            return result.RefreshToken;
        }
    }

    public class DeviantArtSubmissionWrapper : ISubmissionWrapper {
        public bool Mature => Deviation.IsMature == true;
		public bool Adult => false;
        
        public string HTMLDescription {
            get {
                string html = Metadata?.Description;
                if (html == null) return null;

                if (html.IndexOf("<p>", StringComparison.CurrentCultureIgnoreCase) == -1) {
                    html = $"<p>{html}</p>";
                }

                return html;
            }
        }
        public IEnumerable<string> Tags => Metadata?.Tags?.Select(t => t.TagName) ?? Enumerable.Empty<string>();
        public DateTime Timestamp => Deviation.PublishedTime ?? DateTime.Now;
        public string Title => Deviation.Title;

        public string ViewURL => Deviation.Url.AbsoluteUri;
        public string ImageURL => Deviation.Content.Src;
        public string ThumbnailURL => Deviation.Thumbs.FirstOrDefault()?.Src;

        public Color? BorderColor => Deviation.IsMature == true
            ? Color.FromArgb(225, 141, 67)
            : (Color?)null;
		
		public Deviation Deviation { get; private set; }
        public Metadata Metadata { get; private set; }

        public DeviantArtSubmissionWrapper(Deviation deviation, Metadata metadata = null) {
            if (deviation.DeviationId != metadata.DeviationId) throw new ArgumentException("DeviationId must be the same in both arguments");
            Deviation = deviation;
            Metadata = metadata;
        }
    }
}
