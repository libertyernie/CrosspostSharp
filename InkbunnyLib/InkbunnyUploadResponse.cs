using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	public class InkbunnyUploadResponse : InkbunnyResponse {
		public string sid { get; set; }
		public long submission_id { get; set; }
	}
}
