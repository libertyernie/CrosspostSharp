using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading.Tasks;

namespace ArtworkSourceSpecification {
	public record ArtworkList : IArtworkSource {
		public string name;
		public Lazy<Task<IAuthor>> userTask;
		public Lazy<Task<IEnumerable<IPostBase>>> postsTask;

		public string Name => name;

		public static ArtworkList Create(IArtworkSource source, int max = int.MaxValue) {
			return new ArtworkList {
				name = source.Name,
				userTask = new Lazy<Task<IAuthor>>(() => source.GetUserAsync()),
				postsTask = new Lazy<Task<IEnumerable<IPostBase>>>(async () => {
					var list = await source.GetPostsAsync().Take(max).ToListAsync();
					if (list.Count == max)
						throw new Exception($"Maximum of {max} items reached");
					return list.OrderBy(p => p.Timestamp);
				})
			};
		}

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			var list = await postsTask.Value;
			foreach (var p in list) yield return p;
		}

		public Task<IAuthor> GetUserAsync() => userTask.Value;
	}
}
