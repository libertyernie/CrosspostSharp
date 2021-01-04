using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrosspostSharp3.Inkbunny.Responses {
    public class InkbunnySubmissionDetailsResponse : InkbunnyResponse {
        public string sid;
        public string user_location;
        public int results_count;
        public IEnumerable<InkbunnySubmissionDetail> submissions;
    }
}
