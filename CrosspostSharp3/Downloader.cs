using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public static class Downloader {
		private record DownloadedData : IDownloadedData {
			public byte[] Data { get; init; }
			public string ContentType { get; init; }
			public string Filename { get; init; }
		}

		public static async Task<IDownloadedData> DownloadAsync(string url) {
			var req = WebRequest.Create(url);
			using var resp = await req.GetResponseAsync();

			using var stream = resp.GetResponseStream();
			using var ms = new MemoryStream();
			await stream.CopyToAsync(ms);

			byte[] data = ms.ToArray();

			string md5 = string.Join(
				"",
				MD5.Create().ComputeHash(data).Select(b => ((int)b).ToString("x2")));
			string contentType = resp.ContentType;
			string ext = contentType.Split('/').Last();

			return new DownloadedData {
				ContentType = contentType,
				Data = data,
				Filename = $"{md5}.{ext}"
			};
		}

		public static async Task<IDownloadedData> DownloadAsync(IPostBase post) {
			if (post is IDownloadedData downloaded)
				return downloaded;
			else if (post is IRemotePhotoPost remotePhoto)
				return await DownloadAsync(remotePhoto.ImageURL);
			else
				return null;
		}
	}
}
