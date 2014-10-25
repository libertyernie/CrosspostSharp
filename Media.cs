using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LWeasyl {
	public class MediaFile {
		public int mediaid { get; set; }
		public string url { get; set; }
		public SubmissionMedia links { get; set; }
	}

	public class SubmissionMedia {
		public MediaFile[] submission { get; set; }
		public MediaFile[] thumbnail { get; set; }
		public MediaFile[] cover { get; set; }
	}
}
