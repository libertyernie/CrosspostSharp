using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InkbunnyLib.Responses {
    public class InkbunnyEditSubmissionResponse : InkbunnyResponse {
        public long submission_id;
        public bool? twitter_authentication_success;
    }
}
