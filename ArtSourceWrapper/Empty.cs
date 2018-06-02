using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class EmptyWrapper : SiteWrapper<ISubmissionWrapper, int> {
        public override string WrapperName => "None";
		public override bool SubmissionsFiltered => false;

		public override int BatchSize { get; set; } = 0;
        public override int MinBatchSize => 0;
        public override int MaxBatchSize => 0;

        public EmptyWrapper() { }

        public override Task<string> WhoamiAsync() {
            return Task.FromResult<string>(null);
        }

        public override Task<string> GetUserIconAsync(int size) {
            return Task.FromResult<string>(null);
        }

        protected override Task<InternalFetchResult<ISubmissionWrapper, int>> InternalFetchAsync(int? startPosition, int count) {
            return Task.FromResult(new InternalFetchResult<ISubmissionWrapper, int>(0, true));
        }
    }
}
