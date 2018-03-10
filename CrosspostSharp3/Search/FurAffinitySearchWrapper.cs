using ArtSourceWrapper;
using FAExportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3.Search {
	public class FurAffinitySearchWrapper : FurAffinityIdWrapper {
		private readonly string _q;
		private readonly FARating _rating;

		public override int BatchSize { get; set; } = 60;
		public override int MinBatchSize => 60;
		public override int MaxBatchSize => 60;

		public override string WrapperName => $"FurAffinity ({_q})";

		public FurAffinitySearchWrapper(string a, string b, string q, FARating rating = FARating.general) : base(a, b) {
			_q = q;
			_rating = rating;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			string username = await WhoamiAsync();

			int pos = startPosition ?? 1;
			var result = await _client.SearchSubmissionIdsAsync(_q, pos, rating: _rating, type: FAType.art | FAType.photo);
			return new InternalFetchResult(result, pos + 1, isEnded: !result.Any());
		}
	}
}
