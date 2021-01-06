using ArtworkSourceSpecification;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public class TextPost : IPostBase {
		public string Title { get; set; }
		public string HTMLDescription { get; set; }
		public bool Mature { get; set; }
		public bool Adult { get; set; }
		public IEnumerable<string> Tags { get; set; }

		DateTime IPostBase.Timestamp => DateTime.UtcNow;
		string IPostBase.ViewURL => null;

		public TextPost() { }
	}
}
