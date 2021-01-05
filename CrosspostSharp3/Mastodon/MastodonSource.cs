using ArtworkSourceSpecification;
using Pleronet;
using Pleronet.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CrosspostSharp3.Mastodon {
	public class MastodonSource : IArtworkSource {
		private readonly MastodonClient _client;

		public MastodonSource(MastodonClient client) {
			_client = client;
		}

		public string Name => _client.AppRegistration.Instance;

		private class MastodonPostWrapper : IPostBase {
			private readonly Status _status;

			public MastodonPostWrapper(Status status) {
				_status = status;
			}

			public string Title => "";
			public string HTMLDescription => _status.Content;
			public bool Mature => _status.Sensitive;
			public bool Adult => _status.Sensitive;
			public IEnumerable<string> Tags => _status.Tags.Select(x => x.Name);
			public DateTime Timestamp => _status.CreatedAt;
			public string ViewURL => _status.Url;
		}

		private class MastodonPhotoPostWrapper : MastodonPostWrapper, IRemotePhotoPost {
			private readonly Attachment _attachment;

			public MastodonPhotoPostWrapper(Status status, Attachment attachment) : base(status) {
				_attachment = attachment;
			}

			public string ImageURL => _attachment.Url;
			public string ThumbnailURL => _attachment.PreviewUrl;
		}

		private class MastodonAnimatedGifPostWrapper : MastodonPostWrapper, IRemoteVideoPost {
			private readonly Attachment _attachment;

			public MastodonAnimatedGifPostWrapper(Status status, Attachment attachment) : base(status) {
				_attachment = attachment;
			}

			public string VideoURL => _attachment.Url;
			public string ThumbnailURL => _attachment.PreviewUrl;
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await _client.GetCurrentUser();

			var statuses = await _client.GetAccountStatuses(user.Id, excludeReblogs: true);

			while (true) {
				foreach (var s in statuses) {
					var photos = s.MediaAttachments.Where(m => m.Type == "image");
					foreach (var m in photos)
						yield return new MastodonPhotoPostWrapper(s, m);

					var gifs = s.MediaAttachments.Where(m => m.Type == "gifv");
					foreach (var m in gifs)
						yield return new MastodonAnimatedGifPostWrapper(s, m);

					if (!photos.Any() && !gifs.Any())
						yield return new MastodonPostWrapper(s);
				}

				if (statuses.NextPageMaxId == null)
					break;

				statuses = await _client.GetAccountStatuses(user.Id, excludeReblogs: true, maxId: statuses.NextPageMaxId);
			}
		}

		private record Author : IAuthor {
			public string Name { get; init; }
			public string IconUrl { get; init; }
		}

		public async Task<IAuthor> GetUserAsync() {
			var user = await _client.GetCurrentUser();
			return new Author {
				Name = user.UserName,
				IconUrl = user.AvatarUrl
			};
		}
	}
}
