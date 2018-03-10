using ArtSourceWrapper;
using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3.Search {
	internal class DeviantArtSearchWrapper : AsynchronousCachedEnumerable<Deviation, uint> {
		public override int BatchSize { get; set; } = 120;
		public override int MinBatchSize => 1;
		public override int MaxBatchSize => 120;

		private readonly string _query;
		public DeviantArtSearchWrapper(string query) {
			_query = query;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
			uint maxCount = (uint)Math.Max(MinBatchSize, Math.Min(MaxBatchSize, count));
			uint position = startPosition ?? 0;

			var galleryResponse = await new DeviantartApi.Requests.Browse.NewestRequest() {
				Query = _query,
				Limit = maxCount,
				Offset = startPosition
			}.GetNextPageAsync();

			if (galleryResponse.IsError) {
				throw new DeviantArtException(galleryResponse.ErrorText);
			}
			if (!string.IsNullOrEmpty(galleryResponse.Result.Error)) {
				throw new DeviantArtException(galleryResponse.Result.ErrorDescription);
			}

			return new InternalFetchResult(
				galleryResponse.Result.Results,
				position + (uint)galleryResponse.Result.Results.Count,
				!galleryResponse.Result.HasMore);
		}
	}
}
