using ArtSourceWrapper;
using FurryNetworkLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3.Search {
	public class FurryNetworkSearchWrapper : SiteWrapper<FurryNetworkSubmissionWrapper, int> {
		private FurryNetworkClient _client;
		private string _query;

		public FurryNetworkSearchWrapper(FurryNetworkClient client, string query) {
			_client = client;
			_query = query;
		}
		
		public override string SiteName => "Furry Network";

		public override string WrapperName => "Furry Network";

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 30;

		public override int MaxBatchSize => 30;

		public override Task<string> GetUserIconAsync(int size) {
			return Task.FromResult<string>(null);
		}

		public override async Task<string> WhoamiAsync() {
			var user = await _client.GetUserAsync();
			return user.Email;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			var searchResults = await _client.SearchByTypeAsync("artwork", tags: new[] { _query.Replace(" ", "-") }, from: startPosition ?? 0);
			int nextPosition = (startPosition ?? 0) + searchResults.Hits.Count();
			return new InternalFetchResult(
				await Task.WhenAll(searchResults.Hits
					.Select(h => h.Submission)
					.Where(h => h is Artwork || h is Photo)
					.Select(h => FurryNetworkSubmissionWrapper.CreateAsync((FileSubmission)h, _client, convertMarkdown: false))
					.ToArray()),
				nextPosition,
				nextPosition >= searchResults.Total);
		}
	}
}
