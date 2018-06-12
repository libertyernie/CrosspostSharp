using DeviantartApi.Objects;
using DeviantartApi.Objects.SubObjects.DeviationMetadata;
using DeviantartApi.Requests.User;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class DeviantArtStatusWrapper : SiteWrapper<DeviantArtStatusSubmissionWrapper, uint> {
        public override string WrapperName => "DeviantArt (statuses)";
		public override bool SubmissionsFiltered => true;

		public override int BatchSize { get; set; } = 10;
        public override int MinBatchSize => 1;
		public override int MaxBatchSize => 50;
		
		public static async Task<User> GetUserAsync() {
			var result = await new WhoAmIRequest().ExecuteAsync();
			if (result.IsError) {
				throw new Exception(result.ErrorText);
			}
			if (!string.IsNullOrEmpty(result.Result.Error)) {
				throw new Exception(result.Result.ErrorDescription);
			}
			return result.Result;
		}

		private User _user;

		public override async Task<string> WhoamiAsync() {
			if (_user == null) _user = await GetUserAsync();
			return _user.Username;
		}

		public override async Task<string> GetUserIconAsync(int size) {
			if (_user == null) _user = await GetUserAsync();
			return _user.UserIconUrl.AbsoluteUri;
		}

		private IEnumerable<DeviantArtStatusSubmissionWrapper> Wrap(IEnumerable<Status> statuses) {
			foreach (var s in statuses) {
				yield return new DeviantArtStatusSubmissionWrapper(s);
			}
		}

        protected async override Task<InternalFetchResult> InternalFetchAsync(uint? startPosition, int count) {
            uint skip = startPosition ?? 0;
            uint take = (uint)Math.Max(MinBatchSize, Math.Min(MaxBatchSize, BatchSize));

			var req = new StatusesRequest(await WhoamiAsync());
			req.Limit = take;
			req.Offset = skip;
			var resp = await req.ExecuteAsync();
			if (resp.IsError) {
				throw new Exception(resp.ErrorText);
			}
			if (!string.IsNullOrEmpty(resp.Result.Error)) {
				throw new Exception(resp.Result.ErrorDescription);
			}
			return new InternalFetchResult(Wrap(resp.Result.Results), skip + take, !resp.Result.HasMore);
		}

		public static async Task<bool> LogoutAsync() {
			bool success = true;
			foreach (string token in new[] { DeviantartApi.Requester.AccessToken, DeviantartApi.Requester.RefreshToken }) {
				success = success && await DeviantartApi.Login.LogoutAsync(token);
			}
			return success;
		}
	}

    public class DeviantArtStatusSubmissionWrapper : ISubmissionWrapper, IStatusUpdate {
		private Status _status;
		public DeviantArtStatusSubmissionWrapper(Status status) {
			_status = status;
		}

		public IEnumerable<Deviation> Deviations => _status.Items
			.Select(i => i.Deviation)
			.Where(d => d != null);
		public Deviation MainDeviation => Deviations
			.Where(d => d.Author.UserId == _status.Author.UserId)
			.FirstOrDefault();
		public IEnumerable<Deviation> OtherDeviations => Deviations.Except(new[] { MainDeviation });
		public string Icon => _status.Author.UserIconUrl.AbsoluteUri;

		public string Title => "";
		public string HTMLDescription => _status.Body;
		public bool Mature => _status.Items.Any(i => i.Deviation?.IsMature == true);
		public bool Adult => false;
		public IEnumerable<string> Tags => Enumerable.Empty<string>();
		public DateTime Timestamp => _status.TimeStamp;
		public string ViewURL => _status.Url.AbsoluteUri;
		public string ImageURL => MainDeviation?.Content?.Src ?? Icon;
		public string ThumbnailURL => MainDeviation?.Thumbs?.FirstOrDefault()?.Src ?? Icon;
		public Color? BorderColor => Mature
			? Color.FromArgb(225, 141, 67)
			: (Color?)null;

		public string FullHTML => HTMLDescription;
		public bool PotentiallySensitive => Mature || Adult;
		public bool HasPhoto => MainDeviation != null;
		public IEnumerable<string> AdditionalLinks => OtherDeviations.Select(d => d.Url.AbsoluteUri);
	}
}
