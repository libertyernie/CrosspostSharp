using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib {
    public class InkbunnyEditSubmissionResponse : InkbunnyResponse {
        public long submission_id { get; set; }
        public bool? twitter_authentication_success { get; set; }
    }
}
