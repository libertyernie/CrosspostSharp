using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylLib {
	public class WeasylMediaFile {
        public int? mediaid;
        public string url;
        public WeasylSubmissionMedia links;
	}

	public class WeasylSubmissionMedia {
        public IEnumerable<WeasylMediaFile> submission;
        public IEnumerable<WeasylMediaFile> thumbnail;
        public IEnumerable<WeasylMediaFile> cover;
	}
}
