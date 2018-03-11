using ArtSourceWrapper;
using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharpJournal {
	public class DeviantArtJournalSource : JournalSource<DeviantArtJournalWrapper, uint> {
		public override int BatchSize { get; set; } = 10;
		public override int MinBatchSize => 1;
		public override int MaxBatchSize => 50;

		private User _user;

		private async Task<User> GetUserAsync() {
			if (_user == null) {
				var result = await new DeviantartApi.Requests.User.WhoAmIRequest().ExecuteAsync();
				if (result.IsError) {
					throw new DeviantArtException(result.ErrorText);
				}
				if (!string.IsNullOrEmpty(result.Result.Error)) {
					throw new DeviantArtException(result.Result.ErrorDescription);
				}
				_user = result.Result;
			}
			return _user;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
			var user = await GetUserAsync();

			uint maxCount = (uint)Math.Max(MinBatchSize, Math.Min(MaxBatchSize, count));
			uint position = startPosition ?? 0;

			var request = new DeviantartApi.Requests.Browse.User.JournalsRequest(user.Username) {
				Limit = maxCount,
				Offset = position
			};
			var response = await request.GetNextPageAsync();

			if (response.IsError) {
				throw new DeviantArtException(response.ErrorText);
			}
			if (!string.IsNullOrEmpty(response.Result.Error)) {
				throw new DeviantArtException(response.Result.ErrorDescription);
			}

			return new InternalFetchResult(
				response.Result.Results.Select(j => new DeviantArtJournalWrapper(j)),
				position + (uint)response.Result.Results.Count,
				!response.Result.HasMore);
		}
	}

	public class DeviantArtJournalWrapper : IJournalWrapper {
		private readonly Deviation _deviation;

		public DeviantArtJournalWrapper(Deviation deviation) {
			_deviation = deviation;
		}

		public string Title => _deviation.Title;
		public string HTMLDescription => _deviation.Excerpt;
		public DateTime Timestamp => _deviation.PublishedTime ?? DateTime.UtcNow;
		public string ViewURL => _deviation.Url.AbsoluteUri;
	}
}
