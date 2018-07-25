using SourceWrappers;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CrosspostSharp3.Imgur {
	public class ImgurPostWrapper : IRemotePhotoPost, IDeletable {
		public string ImageURL { get; set; }
		public string ThumbnailURL => ImageURL;
		public string Title => "";
		public string HTMLDescription => "";
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Enumerable.Empty<string>();
		public DateTime Timestamp => DateTime.UtcNow;
		public string ViewURL => ImageURL;

		public string SiteName => "Imgur";
		public Task DeleteAsync() {
			// Actual deletion will be done through Imgur post-delete handler
			return Task.CompletedTask;
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
					yield return new ImgurPostWrapper { ImageURL = split[0] };
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
