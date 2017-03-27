using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeasylSyncLib {
	public class Gallery {
		public GallerySubmission[] submissions { get; set; }
		public int? backid { get; set; }
		public int? nextid { get; set; }
	}
}
