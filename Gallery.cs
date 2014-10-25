using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylReadOnly {
	public class Gallery {
		public Submission[] submissions { get; set; }
		public int? backid { get; set; }
		public int? nextid { get; set; }
	}
}
