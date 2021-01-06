using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ArtworkSourceSpecification {
	public interface IAuthor {
		string Name { get; }
		string IconUrl { get; }
	}

	public interface IPostBase {
		string Title { get; }
		string HTMLDescription { get; }
		bool Mature { get; }
		bool Adult { get; }
		IEnumerable<string> Tags { get; }
		DateTime Timestamp { get; }
		string ViewURL { get; }
	}

	public interface IThumbnailPost : IPostBase {
		string ThumbnailURL { get; }
	}

	public interface IRemotePhotoPost : IThumbnailPost {
		string ImageURL { get; }
	}

	public interface IRemoteVideoPost : IThumbnailPost {
		string VideoURL { get; }
	}

	public interface IDownloadedData {
		byte[] Data { get; }
		string ContentType { get; }
		string Filename { get; }
	}

	public interface IArtworkSource {
		string Name { get; }
		Task<IAuthor> GetUserAsync();
		IAsyncEnumerable<IPostBase> GetPostsAsync();
    }
}
