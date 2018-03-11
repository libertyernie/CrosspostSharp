using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharpJournal {
	public class MetaJournalSource : JournalSource<IJournalWrapper, DateTime> {
		private readonly IEnumerable<IJournalSource> _wrappers;

		public MetaJournalSource(IEnumerable<IJournalSource> wrappers) {
			_wrappers = wrappers.ToList();
		}
		
		public override int BatchSize { get; set; }

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 1;
		
		private async Task<IEnumerable<IJournalWrapper>> FetchIfNeeded(IJournalSource w, DateTime start) {
			while (true) {
				var newest = w.Cache.Where(s => s.Timestamp <= start).FirstOrDefault();
				if (newest != null) {
					return new[] { newest };
				}
				if (w.IsEnded) {
					return Enumerable.Empty<IJournalWrapper>();
				}
				await w.FetchAsync();
			}
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(DateTime? startPosition, int count) {
			DateTime start = startPosition ?? DateTime.MaxValue;

			var found = (await Task.WhenAll(_wrappers.Select(w => FetchIfNeeded(w, start)))).SelectMany(s => s);

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
