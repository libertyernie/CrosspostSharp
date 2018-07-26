using Imgur.API.Authentication.Impl;
using Imgur.API.Endpoints.Impl;
using SourceWrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrosspostSharp3.Imgur {
	public class ImgurPostWrapper : IRemotePhotoPost, IDeletable {
		private readonly string _imageUrl;
		private readonly string _deleteHash;

		public ImgurPostWrapper(string imageUrl, string deleteHash) {
			this._imageUrl = imageUrl ?? throw new ArgumentNullException(nameof(imageUrl));
			this._deleteHash = deleteHash ?? throw new ArgumentNullException(nameof(deleteHash));
		}

		public string ImageURL => _imageUrl;
		public string ThumbnailURL => _imageUrl;
		public string Title => "";
		public string HTMLDescription => "";
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Enumerable.Empty<string>();
		public DateTime Timestamp => DateTime.UtcNow;
		public string ViewURL => _imageUrl;

		public string SiteName => "Imgur";
		public async Task DeleteAsync() {
			var imgur = new ImgurClient(OAuthConsumer.Imgur.CLIENT_ID, OAuthConsumer.Imgur.CLIENT_SECRET);
			var endpoint = new ImageEndpoint(imgur);
			await endpoint.DeleteImageAsync(_deleteHash);
		}
	}

	public class PreviousImgurUploadsWrapper : ISourceWrapper<int> {
		public string Name => "Previous Imgur uploads";

		public int SuggestedBatchSize => 1;

		public IEnumerable<IPostBase> FetchAllOldestFirst() {
			using (var fs = new FileStream("imgur-uploads.txt", FileMode.Open, FileAccess.Read))
			using (var sr = new StreamReader(fs)) {
				string line;
				while ((line = sr.ReadLine()) != null) {
					string[] split = line.Split(' ');
					yield return new ImgurPostWrapper(split[0], split[1]);
				}
			}
		}

		public IEnumerable<IPostBase> FetchAllNewestFirst() {
			return FetchAllOldestFirst().Reverse().ToList();
		}

		public Task<IEnumerable<IPostBase>> FetchAllAsync(int limit) {
			return Task.FromResult(FetchAllNewestFirst().Take(limit));
		}

		public Task<string> GetUserIconAsync(int size) {
			throw new NotImplementedException();
		}

		public Task<FetchResult<int>> MoreAsync(int cursor, int take) {
			var e = FetchAllNewestFirst().Skip(cursor).Take(take);
			return Task.FromResult(new FetchResult<int>(e, take, e.Any()));
		}

		public Task<FetchResult<int>> StartAsync(int take) {
			var e = FetchAllNewestFirst().Take(take);
			return Task.FromResult(new FetchResult<int>(e, take, e.Any()));
		}

		public Task<string> WhoamiAsync() {
			return Task.FromResult("");
		}
	}
}
