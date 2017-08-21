using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class EmptyWrapper : SiteWrapper<ISubmissionWrapper, int> {
        public override string SiteName => "No site";

        public override int BatchSize { get; set; } = 0;
        public override int MinBatchSize => 0;
        public override int MaxBatchSize => 0;

        public EmptyWrapper() { }

        public override Task<string> WhoamiAsync() {
            return Task.FromResult<string>(null);
        }

        protected override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            return Task.FromResult(new InternalFetchResult(0, true));
        }
    }
}
