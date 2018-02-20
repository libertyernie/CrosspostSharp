﻿using Pixeez;
using Pixeez.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
	public class PixivWrapper : SiteWrapper<PixivSubmissionWrapper, int> {
		private Tokens _tokens;
		private User _user;

		public PixivWrapper(AuthResult authResult) {
			_tokens = authResult.Tokens;
			_user = authResult.Authorize.User;
		}

		public override string SiteName => "Pixiv";

		public override string WrapperName => "Pixiv";

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 1;

		public override async Task<string> GetUserIconAsync(int size) {
			return _user.GetAvatarUrl();
		}

		public override async Task<string> WhoamiAsync() {
			return _user.Name;
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			var result = await _tokens.GetUserWorksAsync(_user.Id.Value, offset: startPosition ?? 0);
			return new InternalFetchResult(result.illusts.Select(i => new PixivSubmissionWrapper(i)),
				result.illusts.Length + (startPosition ?? 0),
				result.next_url != null);
		}
	}

	public class PixivSubmissionWrapper : ISubmissionWrapper {
		private IllustWork _work;

		public PixivSubmissionWrapper(IllustWork work) {
			_work = work;
		}

		public string Title => _work.Title;
		public string HTMLDescription => _work.Caption;
		public bool PotentiallySensitive => true;
		public IEnumerable<string> Tags => _work.Tags;
		public DateTime Timestamp => _work.CreatedTime;
		public string ViewURL => $"https://www.pixiv.net/member_illust.php?mode=medium&illust_id={_work.Id}";
		public string ImageURL => _work.meta_single_page?.OriginalImageUrl
			?? _work.ImageUrls?.Original
			?? _work.ImageUrls?.Large;
		public string ThumbnailURL => _work.ImageUrls.Px128x128;
		public Color? BorderColor => null;
		public bool OwnWork => true;
	}
}
