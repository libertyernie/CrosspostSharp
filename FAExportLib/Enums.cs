using System;

namespace FAExportLib {
	public enum FAFolder {
		gallery, scraps, favorites
	}

	public enum FAOrder {
		relevancy, date, popularity
	}

	public enum FAOrderDirection {
		asc, desc
	}

	public enum FARange {
		day, week, month, all
	}

	public enum FASearchMode {
		all, any, extended
	}

	[Flags]
	public enum FARating {
		general = 1,
		mature = 2,
		adult = 4
	}

	[Flags]
	public enum FAType {
		art = 1,
		flash = 2,
		photo = 4,
		music = 8,
		story = 16,
		poetry = 32
	}
}
