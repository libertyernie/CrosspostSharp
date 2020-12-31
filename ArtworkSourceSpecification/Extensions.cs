using System.Threading.Tasks;

namespace ArtworkSourceSpecification {
	public static class ArtworkCacheExtensions {
		public static async Task<string> WhoamiAsync(this IArtworkSource source) {
			var user = await source.GetUserAsync();
			return user.Name;
		}

		public static async Task<string> GetUserIconAsync(this IArtworkSource source) {
			var user = await source.GetUserAsync();
			return user.IconUrl;
		}
	}
}
