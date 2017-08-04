using DeviantartApi.Objects;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class EmptyWrapper : IWrapper {
        public string SiteName => "No site";

        public EmptyWrapper() { }

        public Task<string> WhoamiAsync() {
            return Task.FromResult<string>(null);
        }

        public Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p) {
            return Task.FromResult(new UpdateGalleryResult());
        }

        public Task<UpdateGalleryResult> NextPageAsync() {
            return Task.FromResult(new UpdateGalleryResult());
        }

        public Task<UpdateGalleryResult> PreviousPageAsync() {
            return Task.FromResult(new UpdateGalleryResult());
        }
    }
}
