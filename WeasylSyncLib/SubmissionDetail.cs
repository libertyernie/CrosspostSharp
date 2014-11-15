using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib {
	public class SubmissionDetail : Submission {
		public int comments { get; set; }
		public string description { get; set; }
		public string embedlink { get; set; }
		public bool favorited { get; set; }
		public bool favorites { get; set; }
		public string folder_name { get; set; }
		public int? folderid { get; set; }
		public bool friends_only { get; set; }
		public int views { get; set; }
	}
}
