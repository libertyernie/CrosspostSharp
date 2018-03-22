using PinSharp;
using PinSharp.Models;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class PinterestWrapper : SiteWrapper<PinterestSubmissionWrapper, PinterestWrapper.PinterestCursor> {
		public struct PinterestCursor {
			public readonly string cursor;

			public PinterestCursor(string cursor) {
				this.cursor = cursor;
			}
		}

		private readonly PinSharpClient _client;
		private readonly string BoardName;

		private Task<IDetailedUser> _userTask;

		private async Task<IDetailedUser> GetUserAsync() {
			if (_userTask == null) {
				_userTask = _client.Me.GetUserAsync();
			}
			return await _userTask;
		}

		public PinterestWrapper(string accessToken, string boardName) {
			_client = new PinSharpClient(accessToken);
			BoardName = boardName;
		}
		
		public override string WrapperName => "Pinterest";
		public override bool SubmissionsFiltered => false;

		public override int BatchSize { get; set; } = 25;

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 100;

		public override async Task<string> GetUserIconAsync(int size) {
			return (await GetUserAsync()).Images.W60?.Url;
		}

		public override Task<string> WhoamiAsync() {
			return Task.FromResult(BoardName);
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(PinterestCursor? startPosition, int count) {
			var pins = startPosition == null
				? await _client.Boards.GetPinsAsync(BoardName, BatchSize)
				: await _client.Boards.GetPinsAsync(BoardName, startPosition.Value.cursor, BatchSize);
			if (pins == null) {
				throw new Exception($"No pins returned. Board name \"{BoardName}\" may be invalid.");
			}
			return new InternalFetchResult(
				pins.Select(p => new PinterestSubmissionWrapper(p)),
				new PinterestCursor(pins.NextPageCursor),
				pins.NextPageCursor == null);
		}
	}

	public class PinterestSubmissionWrapper : ISubmissionWrapper {
		public readonly IPin Pin;

		public PinterestSubmissionWrapper(IPin pin) {
			Pin = pin;
		}

		public string Title => "";
		public string HTMLDescription => WebUtility.HtmlEncode(Pin.Note);
		public bool Mature => false;
		public bool Adult => false;
		public IEnumerable<string> Tags => Enumerable.Empty<string>();
		public DateTime Timestamp => Pin.CreatedAt;
		public string ViewURL => Pin.Url;
		public string ImageURL => Pin.Images.Original?.Url;
		public string ThumbnailURL => ImageURL;
		public Color? BorderColor => null;
	}
}
