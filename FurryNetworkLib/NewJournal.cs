using System;
using System.Collections.Generic;
using System.Text;

namespace FurryNetworkLib {
	public class NewJournal {
		public Dictionary<string, object> Collections { get; set; } = new Dictionary<string, object>();
		public bool Community_tags_allowed { get; set; } = true;
		public string Content { get; set; }
		public string Description { get; set; }
		public int Rating { get; set; } = 0;
		public string Status { get; set; } = "public";
		public string Subtitle { get; set; }
		public Dictionary<int, string> Tags { get; set; } = new Dictionary<int, string>();
		public string Title { get; set; }
	}
}
