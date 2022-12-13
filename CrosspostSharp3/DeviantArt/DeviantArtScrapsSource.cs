using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtScrapsSource : DeviantArtSource {
		private static readonly Lazy<HttpClient> _client = new(() => {
			var client = new HttpClient(new HttpClientHandler {
				AutomaticDecompression = System.Net.DecompressionMethods.All
			});
			client.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
			client.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
			client.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
			client.DefaultRequestHeaders.Add("Connection", "keep-alive");
			client.DefaultRequestHeaders.Add("Host", "www.deviantart.com");
			client.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:107.0) Gecko/20100101 Firefox/107.0");
			client.DefaultRequestHeaders.Add("Upgrade-Insecure-Requests", "1");
			return client;
		});

		public DeviantArtScrapsSource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt (scraps)";

		private static async IAsyncEnumerable<Uri> GetLinksAsync(string username) {
			Uri next = new($"https://www.deviantart.com/{Uri.EscapeDataString(username)}/gallery/scraps");

			while (next != null) {
				string html = await _client.Value.GetStringAsync(next);

				next = null;

				var document = new HtmlDocument();
				document.LoadHtml(html);

				foreach (var node in document.DocumentNode.Descendants("a"))
					if (node.GetAttributeValue("data-hook", null) == "deviation_link")
						if (node.GetAttributeValue("href", null) is string str)
							if (Uri.TryCreate(str, UriKind.Absolute, out Uri uri))
								yield return uri;

				foreach (var node in document.DocumentNode.Descendants("link"))
					if (node.GetAttributeValue("rel", null) == "next")
						if (node.GetAttributeValue("href", null) is string str)
							if (Uri.TryCreate(str, UriKind.Absolute, out Uri uri))
								next = uri;
			}
		}

		private static readonly Regex DeviationUriRegex = new("^DeviantArt://deviation/([^/]+)$", RegexOptions.IgnoreCase);

		private static Guid? ExtractId(string da_app_uri) {
			var match = DeviationUriRegex.Match(da_app_uri);
			if (match.Success && Guid.TryParse(match.Groups[1].Value, out Guid g)) return g;
			return null;
		}

		private static async Task<Guid> GetApiIdAsync(Uri url) {
			string html = await _client.Value.GetStringAsync(url);
			var document = new HtmlDocument();
			document.LoadHtml(html);

			foreach (var node in document.DocumentNode.Descendants("meta"))
				if (node.GetAttributeValue("property", null) == "da:appurl")
					if (node.GetAttributeValue("content", null) is string str)
						if (ExtractId(str) is Guid g)
							return g;

			throw new Exception("Could not find appropriate meta tag");
		}

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();

			await foreach (Uri link in GetLinksAsync(user.Name).Distinct()) {
				Guid deviationId = await GetApiIdAsync(link);

				var deviation = await DeviantArtFs.Api.Deviation.AsyncGet(
					_token,
					ObjectExpansion.None,
					deviationId).StartAsTask();
				var metadata = await DeviantArtFs.Api.Deviation.AsyncGetMetadata(
					_token,
					ExtParams.None,
					new[] { deviationId }).StartAsTask();

				if (!deviation.is_deleted && metadata.metadata.Any())
					yield return new DeviantArtPostWrapper(deviation, metadata.metadata.Single());
			}
		}
	}
}
