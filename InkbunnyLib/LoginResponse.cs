using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
	public class LoginResponse : InkbunnyResponse {
		public string sid { get; set; }
		public long user_id { get; set; }
		public string ratingsmask { get; set; }
	}
}
