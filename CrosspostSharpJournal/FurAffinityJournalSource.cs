using ArtSourceWrapper;
using FAExportLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharpJournal {
	public class FurAffinityJournalSource : JournalSource<FurAffinityJournalWrapper, int>, IJournalDestination {
		private readonly FAUserClient _client;
		
		public override int BatchSize { get; set; } = int.MaxValue;
		public override int MinBatchSize => int.MaxValue;
		public override int MaxBatchSize => int.MaxValue;

		public string SiteName => "FurAffinity";

		public FurAffinityJournalSource(string a, string b) {
			_client = new FAUserClient(a, b);
		}

		private string _cachedUserName;

		public async Task<string> WhoamiAsync() {
			if (_cachedUserName == null) {
				_cachedUserName = await _client.WhoamiAsync();
			}
			return _cachedUserName;
		}

		protected async override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			var js = await _client.GetJournalsAsync(await WhoamiAsync());
			return new InternalFetchResult(
				js.Select(j => new FurAffinityJournalWrapper(j)),
				js.Count(),
				true);
		}

		public async Task PostAsync(string title, string text, string teaser) {
			await _client.PostJournalAsync(title, text);
		}
	}

	public class FurAffinityJournalWrapper : IJournalWrapper {
		private readonly FAJournal _journal;

		public FurAffinityJournalWrapper(FAJournal journal) {
			_journal = journal;
		}

		public string Title => _journal.title;
		public string HTMLDescription => _journal.description;
		public DateTime Timestamp => _journal.posted_at;
		public string ViewURL => _journal.link;
	}
}
