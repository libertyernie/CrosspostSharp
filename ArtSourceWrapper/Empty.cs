using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class EmptyWrapper : SiteWrapper<ISubmissionWrapper, int> {
        public override string SiteName => "No site";

        public EmptyWrapper() { }

        public override Task<string> WhoamiAsync() {
            return Task.FromResult<string>(null);
        }

        protected override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, ushort? maxCount) {
            return Task.FromResult(new InternalFetchResult(0, true));
        }
    }
}
