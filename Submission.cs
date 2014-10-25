using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylReadOnly {
	public class Submission {
		public SubmissionMedia media { get; set; }
		public string owner { get; set; }
		public string owner_login { get; set; }
		public DateTime posted_at { get; set; }
		public string rating { get; set; }
		public int submitid { get; set; }
		public string subtype { get; set; }
		public string[] tags { get; set; }
		public string title { get; set; }
		public string type { get; set; }

		public override string ToString() {
			return posted_at + " " + title;
		}
	}
}
