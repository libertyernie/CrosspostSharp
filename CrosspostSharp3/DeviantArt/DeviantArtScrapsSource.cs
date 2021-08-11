using ArtworkSourceSpecification;
using DeviantArtFs;
using DeviantArtFs.Extensions;
using DeviantArtFs.ParameterTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace CrosspostSharp3.DeviantArt {
	public class DeviantArtScrapsSource : DeviantArtSource {
		public DeviantArtScrapsSource(IDeviantArtAccessToken token) : base(token) { }

		public override string Name => "DeviantArt (scraps)";

		private record EclipseGalleryContents {
			public bool HasMore { get; init; }
			public int? NextOffset { get; init; }
			public IEnumerable<long> DeviationIds { get; init; }
		}

		private static async Task<EclipseGalleryContents> FetchBatchAsync(string username, int offset, int limit) {
			var req = WebRequest.CreateHttp($"https://www.deviantart.com/_napi/da-user-profile/api/gallery/contents?username={Uri.EscapeDataString(username)}&offset={offset}&limit={limit}&scraps_folder=true&mode=newest");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Accept = "application/json";

			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);

			string json = await sr.ReadToEndAsync();
			var obj = JsonConvert.DeserializeAnonymousType(json, new {
				hasMore = false,
				nextOffset = (int?)null,
				results = new[] {
					new {
						deviation = new {
							deviationId = 0L
						}
					}
				}
			});
			return new EclipseGalleryContents {
				HasMore = obj.hasMore,
				NextOffset = obj.nextOffset,
				DeviationIds = obj.results.Select(x => x.deviation.deviationId)
			};
		}

		private static async Task<Guid> GetApiIdAsync(long deviationId) {
			var req = WebRequest.CreateHttp($"https://www.deviantart.com/_napi/shared_api/deviation/extended_fetch?deviationid={deviationId}&type=art&include_session=false");
			req.UserAgent = "CrosspostSharp/4.0 (https://github.com/libertyernie/CrosspostSharp)";
			req.Accept = "application/json";

			using var resp = await req.GetResponseAsync();
			using var stream = resp.GetResponseStream();
			using var sr = new StreamReader(stream);

			string json = await sr.ReadToEndAsync();
			var obj = JsonConvert.DeserializeAnonymousType(json, new {
				deviation = new {
					extended = new {
						deviationUuid = Guid.Empty
					}
				}
			});
			return obj.deviation.extended.deviationUuid;
		}

		public override async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var user = await GetUserAsync();

			int offset = 0;
			while (true) {
				var batch = await FetchBatchAsync(user.Name, offset, 24);
				foreach (var d in batch.DeviationIds) {
					Guid deviationId = await GetApiIdAsync(d);

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

				if (!batch.HasMore)
					break;

				offset = batch.NextOffset.Value;
			}
		}
	}
}
