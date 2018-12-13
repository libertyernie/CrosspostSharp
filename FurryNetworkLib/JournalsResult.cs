using System;
using System.Collections.Generic;
using System.Text;

namespace FurryNetworkLib {
    public class JournalsResult {
		public int Page { get; set; }
		public int Page_count { get; set; }
		public int Result_count { get; set; }
		public int Results_per_page { get; set; }
		public IEnumerable<Journal> Results { get; set; }
    }
}
