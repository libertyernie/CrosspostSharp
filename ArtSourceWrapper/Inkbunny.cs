using InkbunnyLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using InkbunnyLib.Responses;

namespace ArtSourceWrapper {
	public class InkbunnyWrapper : IWrapper {
		private InkbunnyClient _client;
		private InkbunnySearchResponse _lastResponse;
        private UpdateGalleryParameters _lastUpdateGalleryParameters;

        public InkbunnyWrapper(InkbunnyClient client) {
			_client = client;
		}

		public string SiteName => "Inkbunny";

		private async Task<UpdateGalleryResult> Wrap(InkbunnySearchResponse response) {
            _lastUpdateGalleryParameters.Progress?.Report(0.5f);
            var details = await _client.GetSubmissionsAsync(
				response.submissions.Select(s => s.submission_id),
				show_description_bbcode_parsed: true);
            _lastUpdateGalleryParameters.Progress?.Report(1);
            return new UpdateGalleryResult {
				Submissions = details.submissions
					.OrderByDescending(s => s.submission_id)
					.Where(s => !s.hidden)
					.Select(s => {
						ISubmissionWrapper w = new InkbunnySubmissionWrapper(s, _client.Sid);
						return w;
					})
					.ToList(),
				HasLess = response.page > 1,
				HasMore = response.page < response.pages_count
			};
		}

		public async Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
			_lastResponse = await _client.SearchAsync(new InkbunnySearchParameters {
				UserId = _client.UserId
			}, submissions_per_page: p.Count);
            _lastUpdateGalleryParameters = p;
			return await Wrap(_lastResponse);
		}

		public async Task<UpdateGalleryResult> NextPageAsync() {
			_lastResponse = await _client.NextPageAsync(_lastResponse, submissions_per_page: _lastUpdateGalleryParameters.Count);
			return await Wrap(_lastResponse);
		}

		public async Task<UpdateGalleryResult> PreviousPageAsync() {
			_lastResponse = await _client.PrevPageAsync(_lastResponse, submissions_per_page: _lastUpdateGalleryParameters.Count);
			return await Wrap(_lastResponse);
		}

		public Task<string> WhoamiAsync() {
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
        public string ImageURL => Public
            ? Submission.file_url_full
            : null;
		public bool PotentiallySensitive => Submission.rating_id != InkbunnyRating.General;
		public IEnumerable<string> Tags => Submission.keywords.Select(k => k.keyword_name);
        public string ThumbnailURL => Public
            ? Submission.thumbnail_url_medium ?? Submission.thumbnail_url_medium_noncustom
            : null;
		public DateTime Timestamp => Submission.create_datetime.ToLocalTime().LocalDateTime;
		public string Title => Submission.title;
		public string ViewURL => "https://inkbunny.net/submissionview.php?id=" + Submission.submission_id;

		public bool OwnWork => true;
        public bool Public => Submission.@public;

        public InkbunnySubmissionWrapper(InkbunnySubmissionDetail submission, string sid) {
			Submission = submission;
        }
	}
}
