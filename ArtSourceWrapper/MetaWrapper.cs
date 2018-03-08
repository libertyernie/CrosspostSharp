using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class MetaWrapper : SiteWrapper<ISubmissionWrapper, DateTime> {
		private readonly IEnumerable<ISiteWrapper> _wrappers;

		public MetaWrapper(IEnumerable<ISiteWrapper> wrappers) {
			_wrappers = wrappers.ToList();
		}

		public override string SiteName => "All";

		public override string WrapperName => "All";

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 1;

		public override Task<string> GetUserIconAsync(int size) {
			return Task.FromResult<string>(null);
		}

		public override Task<string> WhoamiAsync() {
			return Task.FromResult("All");
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(DateTime? startPosition, int count) {
			DateTime start = startPosition ?? DateTime.MaxValue;

			var found = new List<ISubmissionWrapper>();
			foreach (var w in _wrappers) {
				while (true) {
					var newest = w.Cache.Where(s => s.Timestamp <= start).FirstOrDefault();
					if (newest != null) {
						found.Add(newest);
						break;
					}
					if (w.IsEnded) {
						break;
					}
					await w.FetchAsync();
				}
			}

			var ts = found
				.OrderByDescending(s => s.Timestamp)
				.Select(s => s.Timestamp)
				.DefaultIfEmpty(start)
				.First();
			var nextPosition = ts.AddTicks(-1);

			var items = found
				.Where(s => s.Timestamp == ts);
			return new InternalFetchResult(items, nextPosition, _wrappers.All(w => w.IsEnded));
		}
	}
}
