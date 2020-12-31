using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace ArtworkSourceSpecification {
	public class ArtworkCache : IArtworkSource {
		public string Name { get; }

		private readonly Lazy<Task<IAuthor>> _userTask;

		private readonly List<IPostBase> _cache;
		private readonly IAsyncEnumerator<IPostBase> _enumerator;
		private readonly SemaphoreSlim _sem;

		public ArtworkCache(IArtworkSource source) {
			Name = source.Name;

			_userTask = new Lazy<Task<IAuthor>>(() => source.GetUserAsync());

			_cache = new List<IPostBase>();
			_enumerator = source.GetPostsAsync().GetAsyncEnumerator();
			_sem = new SemaphoreSlim(1, 1);
		}

		public Task<IAuthor> GetUserAsync() => _userTask.Value;

		public async IAsyncEnumerable<IPostBase> GetPostsAsync() {
			for (int i = 0; ; i++) {
				await _sem.WaitAsync();
				try {
					if (_cache.Count <= i) {
						if (!await _enumerator.MoveNextAsync()) {
							yield break;
						}
						_cache.Add(_enumerator.Current);
					}
					yield return _cache[i];
				} finally {
					_sem.Release();
				}
			}
		}
	}
}
