using InkbunnyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using InkbunnyLib.Responses;

namespace ArtSourceWrapper {
	public class InkbunnySearchWrapper : SiteWrapper<InkbunnySubmissionWrapper, int> {
		private InkbunnyClient _client;
		private string _query;
		private string _rid;

		public InkbunnySearchWrapper(InkbunnyClient client, string query) {
			_client = client;
			_query = query;
		}

		public override string SiteName => "Inkbunny";
		public override string WrapperName => "Inkbunny";

		public override int BatchSize { get; set; } = 30;
		public override int MinBatchSize => 1;
		public override int MaxBatchSize => 100;

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int maxCount) {
			var response = startPosition == null
				? await _client.SearchAsync(
					new InkbunnySearchParameters { Text = _query, Keywords = true, Title = true, Description = true },
					maxCount)
				: await _client.SearchAsync(_rid,
					startPosition.Value + 1,
					maxCount);

			if (response.pages_count < (startPosition ?? 1)) {
				return new InternalFetchResult(response.page + 1, isEnded: true);
			}

			_rid = response.rid;
			var details = await _client.GetSubmissionsAsync(response.submissions.Select(s => s.submission_id), show_description_bbcode_parsed: true);
			var wrappers = details.submissions
				.Where(s => s.@public)
				.OrderByDescending(s => s.create_datetime)
				.Select(s => new DeletableInkbunnySubmissionWrapper(_client, s));
			return new InternalFetchResult(wrappers, response.page + 1);
		}

		public override async Task<string> WhoamiAsync() {
			AsynchronousCachedEnumerable<InkbunnySubmissionWrapper, int> wrapper = this;
			if (!wrapper.Cache.Any()) {
				await wrapper.FetchAsync();
			}
			if (!wrapper.Cache.Any()) {
				throw new Exception("No Inkbunny submissions - cannot determine username");
			}
			return wrapper.Cache.First().Username;
		}

		public override async Task<string> GetUserIconAsync(int size) {
			AsynchronousCachedEnumerable<InkbunnySubmissionWrapper, int> wrapper = this;
			if (!wrapper.Cache.Any()) {
				await wrapper.FetchAsync();
			}
			if (!wrapper.Cache.Any()) {
				throw new Exception("No Inkbunny submissions - cannot determine username");
			}
			return wrapper.Cache.First().UserIcon;
		}
	}
}