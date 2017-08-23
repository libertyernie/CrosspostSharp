using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class LocalDirectoryWrapper : SiteWrapper<LocalFileSubmissionWrapper, int> {
        private string _directory;
        private Stack<string> _fileStack;

        public override string SiteName => "Local";
        public override string WrapperName => _directory ?? "Local folder";

        public override int BatchSize { get; set; } = 1;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => int.MaxValue;

        public LocalDirectoryWrapper(string directory = null) {
            _directory = directory;
        }

        public override async Task<string> WhoamiAsync() {
            return Environment.UserName;
        }

        public virtual string SelectDirectory() {
            throw new NotImplementedException();
        }

        private IEnumerable<LocalFileSubmissionWrapper> Wrap() {
            if (_fileStack == null) {
                if (_directory == null) {
                    _directory = SelectDirectory();
                    if (_directory == null) {
                        yield break;
                    }
                }

                var files = new DirectoryInfo(_directory)
                    .EnumerateFiles()
                    .OrderBy(f => f.CreationTime)
                    .Select(f => f.FullName);
                _fileStack = new Stack<string>(files);
            }

            while (_fileStack.Any()) {
                string path = _fileStack.Pop();
                System.Diagnostics.Debug.WriteLine(path);
                bool ok = true;
                try {
                    using (Image i = Image.FromFile(path)) { }
                } catch (ArgumentException) {
                    ok = false;
                }
                if (ok) yield return new LocalFileSubmissionWrapper(path);
            }
        }

        protected override async Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            var wrappers = Wrap().Take(count).ToList();
            var isEnded = !_fileStack.Any();
            return new InternalFetchResult(wrappers, 0, isEnded);
        }
    }

    public class LocalFileSubmissionWrapper : ISubmissionWrapper {
        private readonly string _path;
        public LocalFileSubmissionWrapper(string path) {
            _path = path;
        }

        public string Title => Path.GetFileNameWithoutExtension(_path);
        public string HTMLDescription => "";
        public bool PotentiallySensitive => false;
        public IEnumerable<string> Tags => Enumerable.Empty<string>();
        public string GeneratedUniqueTag => $"#testtest";
        public DateTime Timestamp => File.GetCreationTime(_path);
        public string ViewURL => null;
        public string ImageURL => "file:///" + _path.Replace('\\', '/');
        public string ThumbnailURL => "file:///" + _path.Replace('\\', '/');
        public Color? BorderColor => null;
        public bool OwnWork => true;
    }
}
