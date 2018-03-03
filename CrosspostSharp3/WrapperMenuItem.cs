using ArtSourceWrapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3 {
	public class WrapperMenuItem {
		public readonly ISiteWrapper BaseWrapper;
		public readonly string DisplayName;

		public WrapperMenuItem(ISiteWrapper baseWrapper, string displayName) {
			BaseWrapper = baseWrapper;
			DisplayName = displayName;
		}

		public override string ToString() {
			return DisplayName;
		}
	}
}
