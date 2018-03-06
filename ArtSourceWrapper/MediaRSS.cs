using MediaRss;
using MediaRss.Primary;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.ServiceModel.Syndication;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ArtSourceWrapper {
	public class MediaRSSWrapper : SiteWrapper<MediaRSSItemWrapper, int> {
		private readonly Uri _initialUrl;
		private readonly string _name;

		private List<Uri> _urls;

		public MediaRSSWrapper(Uri url, string name = null) {
			_initialUrl = url;
			_name = name ?? url.AbsoluteUri;
			_urls = new List<Uri> { url };
		}

		public override string SiteName => _initialUrl.AbsoluteUri;

		public override string WrapperName => _initialUrl.AbsoluteUri;

		public override int BatchSize { get; set; }

		public override int MinBatchSize => 1;

		public override int MaxBatchSize => 1;

		public override Task<string> GetUserIconAsync(int size) {
			return Task.FromResult<string>(null);
		}

		public override Task<string> WhoamiAsync() {
			return Task.FromResult(_name);
		}

		protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
			Uri url = _urls.Skip(startPosition ?? 0).FirstOrDefault();
			if (url == null) throw new NotImplementedException("Cannot jump ahead here - please start at 0");

			var req = WebRequest.Create(url);
			req.Method = "GET";
			if (req is HttpWebRequest h) h.UserAgent = "CrosspostSharp/3.0 (https://github.com/libertyernie/CrosspostSharp)";
			using (var resp = await req.GetResponseAsync())
			using (var stream = resp.GetResponseStream())
			using (var ms = new MemoryStream()) {
				await stream.CopyToAsync(ms);
				ms.Position = 0;
				var reader = XmlReader.Create(ms);
				var feed = SyndicationFeed.Load<MediaRssFeed>(reader);

				var wrappers = feed.Items.Select(i => new MediaRSSItemWrapper((MediaRssItem)i));

				int nextPosition = (startPosition ?? 0) + 1;
				if (nextPosition == _urls.Count) {
					var q = feed.Links.Where(l => l.RelationshipType == "next").Select(l => l.Uri);
					if (q.Any()) _urls.Add(q.First());
				}

				return new InternalFetchResult(
					wrappers,
					nextPosition,
					!_urls.Skip(nextPosition).Any());
			}
		}
	}

	public class MediaRSSItemWrapper : ISubmissionWrapper {
		private readonly MediaRssItem _item;

		public MediaRSSItemWrapper(MediaRssItem item) {
			_item = item;
		}

		public string Title => _item.Title.Text;

		public string HTMLDescription => _item.OptionalElements.DescriptionNode.DescriptionText;

		public bool Mature => false;

		public bool Adult => false;

		public IEnumerable<string> Tags => Enumerable.Empty<string>();

		public DateTime Timestamp => _item.PublishDate.UtcDateTime;

		public string ViewURL => _item.Links.Where(l => l.RelationshipType == "alternate").Select(l => l.Uri.AbsoluteUri).FirstOrDefault();

		public string ImageURL => _item.ContentNodes.OrderByDescending(c => c.Width * c.Height).Select(c => c.Url.AbsoluteUri).FirstOrDefault();

		public string ThumbnailURL => _item.OptionalElements.ThumbnailNode.Url.AbsoluteUri;

		public Color? BorderColor => null;
	}
}
