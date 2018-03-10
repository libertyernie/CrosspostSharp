using DontPanic.TumblrSharp;
using DontPanic.TumblrSharp.Client;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class TumblrSearchWrapper : SiteWrapper<TumblrSubmissionWrapper, DateTime> {
        private readonly TumblrClient _client;
		private readonly string _query;

        public TumblrSearchWrapper(TumblrClient client, string query) {
            _client = client;
			_query = query;
        }

        public override string SiteName => "Tumblr";
        public override string WrapperName => "Tumblr";

        public override int BatchSize { get; set; } = 20;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => 20;

        protected override async Task<InternalFetchResult> InternalFetchAsync(DateTime? startPosition, int maxCount) {
			var posts = await _client.GetTaggedPostsAsync(_query, startPosition ?? DateTime.UtcNow, BatchSize);
			
            var list = posts
                .Select(post => post as PhotoPost)
                .Where(post => post != null)
                .Select(post => new DeletableTumblrSubmissionWrapper(_client, post));

            return new InternalFetchResult(list, posts.Select(p => p.Timestamp).Min(), !posts.Any());
        }

        public async override Task<string> WhoamiAsync() {
			var user = await _client.GetUserInfoAsync();
			return user.Name;
		}

        public async override Task<string> GetUserIconAsync(int size) {
            return $"https://api.tumblr.com/v2/blog/{await WhoamiAsync()}.tumblr.com/avatar/{size}";
        }
    }
}
