using SourceWrappers;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class PostWrapperWrapper : ISubmissionWrapper {
		private IPostWrapper _post;

		public PostWrapperWrapper(IPostWrapper post) {
			_post = post ?? throw new ArgumentNullException(nameof(post));
		}

		public string Title => _post.Title;
		public string HTMLDescription => _post.HTMLDescription;
		public bool Mature => _post.Mature;
		public bool Adult => _post.Adult;
		public IEnumerable<string> Tags => _post.Tags;
		public DateTime Timestamp => _post.Timestamp;
		public string ViewURL => _post.ViewURL;
		public string ImageURL => _post.ImageURL;
		public string ThumbnailURL => _post.ThumbnailURL;
		public Color? BorderColor => null;
	}

	public class SourceWrapperWrapper : SiteWrapper<PostWrapperWrapper, int> {
		private readonly SourceWrapper _source;

		public SourceWrapperWrapper(SourceWrapper source) {
			_source = source ?? throw new ArgumentNullException(nameof(source));
		}

		public override string WrapperName => _source.Name;
		public override bool SubmissionsFiltered => _source.SubmissionsFiltered;

		public override int BatchSize { get; set; } = 5;

		public override int MinBatchSize => 1;
		public override int MaxBatchSize => 200;

		public override Task<string> WhoamiAsync() {
			return _source.WhoamiAsync();
		}

		public override Task<string> GetUserIconAsync(int size) {
			return _source.GetUserIconAsync(size);
		}

		protected async override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			int startPos = startPosition ?? 0;
			var got = await _source.FetchAsync(startPos, count);
			return new InternalFetchResult(got.Select(w => new PostWrapperWrapper(w)), startPos + count, false);
		}
	}
}
