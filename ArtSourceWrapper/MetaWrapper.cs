using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class MetaWrapper : SiteWrapper<ISubmissionWrapper, DateTime> {
		private readonly string _name;
		private readonly IEnumerable<ISiteWrapper> _wrappers;

		public MetaWrapper(string name, IEnumerable<ISiteWrapper> wrappers) {
			_name = name;
			_wrappers = wrappers.ToList();
		}

		public override string SiteName => _name;

		public override string WrapperName => _name;

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 1;

		public override Task<string> GetUserIconAsync(int size) {
			return Task.FromResult<string>(null);
		}

		public override async Task<string> WhoamiAsync() {
			return string.Join(", ", (await Task.WhenAll(_wrappers.Select(w => w.WhoamiAsync()))).Distinct());
		}

		private async Task<IEnumerable<ISubmissionWrapper>> FetchIfNeeded(ISiteWrapper w, DateTime start) {
			while (true) {
				var newest = w.Cache.Where(s => s.Timestamp <= start).FirstOrDefault();
				if (newest != null) {
					return new[] { newest };
				}
				if (w.IsEnded) {
					return Enumerable.Empty<ISubmissionWrapper>();
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
