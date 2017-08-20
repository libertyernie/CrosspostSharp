using InkbunnyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using InkbunnyLib.Responses;

namespace ArtSourceWrapper {
	public class InkbunnyWrapper : SiteWrapper<InkbunnySubmissionWrapper, int> {
		private InkbunnyClient _client;
        private string _rid;

        public InkbunnyWrapper(InkbunnyClient client) {
			_client = client;
		}

		public override string SiteName => "Inkbunny";

        public override int BatchSize { get; set; } = 30;
        public override int IndividualRequestsPerInvocation { get; set; } = 0;

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition) {
            int maxCount = Math.Max(0, Math.Min(BatchSize, 100));

            var response = startPosition == null
                ? await _client.SearchAsync(
                    new InkbunnySearchParameters { UserId = _client.UserId, DaysLimit = 30 },
                    maxCount)
                : await _client.SearchAsync(_rid,
                    startPosition.Value + 1,
                    maxCount);
            
            if (response.pages_count < (startPosition ?? 1)) {
                return new InternalFetchResult(response.page + 1, isEnded: true);
            }

            _rid = response.rid;
            var details = await _client.GetSubmissionsAsync(response.submissions.Select(s => s.submission_id));
            var wrappers = details.submissions
                .Where(s => s.@public)
                .OrderByDescending(s => s.create_datetime)
                .Select(s => new InkbunnySubmissionWrapper(s));
            return new InternalFetchResult(wrappers, response.page + 1);
        }

        public override Task<string> WhoamiAsync() {
			return _client.GetUsernameAsync();
		}
    }

	public class InkbunnySubmissionWrapper : ISubmissionWrapper {
		public readonly InkbunnySubmissionDetail Submission;

		public Color? BorderColor {
			get {
				switch (Submission.rating_id) {
					case InkbunnyRating.Mature:
						return Color.FromArgb(170, 187, 34);
					case InkbunnyRating.Adult:
						return Color.FromArgb(185, 30, 35);
					default:
						return null;
				}
			}
		}

		public string GeneratedUniqueTag => "#inkbunny" + Submission.submission_id;
		public string HTMLDescription => Submission.description_bbcode_parsed;
        public string ImageURL => Submission.file_url_full;
		public bool PotentiallySensitive => Submission.rating_id != InkbunnyRating.General;
		public IEnumerable<string> Tags => Submission.keywords.Select(k => k.keyword_name);
        public string ThumbnailURL => Submission.thumbnail_url_medium ?? Submission.thumbnail_url_medium_noncustom;
		public DateTime Timestamp => Submission.create_datetime.ToLocalTime().LocalDateTime;
		public string Title => Submission.title;
		public string ViewURL => "https://inkbunny.net/submissionview.php?id=" + Submission.submission_id;

		public bool OwnWork => true;

        public InkbunnySubmissionWrapper(InkbunnySubmissionDetail submission) {
			Submission = submission;
        }
	}
}
