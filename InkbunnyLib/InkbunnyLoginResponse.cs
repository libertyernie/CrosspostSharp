using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	public class InkbunnyLoginResponse : InkbunnyResponse {
		public string sid { get; set; }
		public int user_id { get; set; }
		public string ratingsmask { get; set; }
	}
}
