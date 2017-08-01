using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public interface IWrapper {
        string SiteName { get; }
        Task<string> WhoamiAsync();
        Task<UpdateGalleryResult> UpdateGalleryAsync(UpdateGalleryParameters p);
        Task<UpdateGalleryResult> NextPageAsync();
        Task<UpdateGalleryResult> PreviousPageAsync();
    }

    public class UpdateGalleryParameters {
        public int Count;
        public bool Weasyl_LoadCharacters;

        public IProgress<double> Progress;

        public UpdateGalleryParameters() {
            Count = 4;
        }
    }

    public class UpdateGalleryResult {
        public IList<ISubmissionWrapper> Submissions;
        public bool HasLess;
        public bool HasMore;

        internal UpdateGalleryResult() { }
    }

    public interface ISubmissionWrapper {
        string Title { get; }
        string HTMLDescription { get; }
        string URL { get; }
        bool PotentiallySensitive { get; }
        IEnumerable<string> Tags { get; }
        string GeneratedUniqueTag { get; }
        DateTime Timestamp { get; }

        string ImageURL { get; }
        string ThumbnailURL { get; }
        Color? BorderColor { get; }
    }
}
