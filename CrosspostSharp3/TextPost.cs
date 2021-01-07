using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;

namespace CrosspostSharp3 {
	public record TextPost : IPostBase {
		public string Title { get; init; }
		public string HTMLDescription { get; init; }
		public bool Mature { get; init; }
		public bool Adult { get; init; }
		public IEnumerable<string> Tags { get; init; }

		DateTime IPostBase.Timestamp => DateTime.UtcNow;
		string IPostBase.ViewURL => null;
	}
}
