using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ArtSourceWrapper {
    public class LocalPathWrapper : SiteWrapper<LocalFileSubmissionWrapper, int> {
        private string _path;
        private Stack<string> _fileStack;
		
        public override string WrapperName => _path ?? "Local folder";
		public override bool SubmissionsFiltered => false;

		public override int BatchSize { get; set; } = 1;
        public override int MinBatchSize => 1;
        public override int MaxBatchSize => int.MaxValue;

        public LocalPathWrapper(string path = null) {
            _path = path;
        }

        public override Task<string> WhoamiAsync() {
            return Task.FromResult(Environment.UserName);
        }

        public override Task<string> GetUserIconAsync(int size) {
            return null;
        }

        public virtual string SelectDirectory() {
            throw new NotImplementedException();
        }

        private static ImageFormat DetectFormat(string path) {
            var map = new Dictionary<byte[], ImageFormat> {
                [new byte[] { 0xFF, 0xD8 }] = ImageFormat.Jpeg,
                [new byte[] { 0x89, 0x50, 0x4E, 0x47, 0x0D, 0x0A, 0x1A, 0x0A }] = ImageFormat.Png,
                [new byte[] { 0x47, 0x49, 0x46 }] = ImageFormat.Gif,
            };

            byte[] header = new byte[8];
            using (var fs = new FileStream(path, FileMode.Open, FileAccess.Read)) {
                fs.Read(header, 0, 8);
            }
            foreach (var pair in map) {
                if (header.Take(pair.Key.Length).SequenceEqual(pair.Key)) {
                    return pair.Value;
                }
            }
            return null;
        }

        private IEnumerable<LocalFileSubmissionWrapper> Wrap() {
            if (_fileStack == null) {
                if (_path == null) {
                    _path = SelectDirectory();
                    if (_path == null) {
                        yield break;
                    }
                }

                if (Directory.Exists(_path)) {
                    var files = new DirectoryInfo(_path)
                        .EnumerateFiles()
                        .OrderBy(f => f.CreationTime)
                        .Select(f => f.FullName);
                    _fileStack = new Stack<string>(files);
                } else if (File.Exists(_path)) {
                    _fileStack = new Stack<string>();
                    _fileStack.Push(_path);
                }
            }

            while (_fileStack.Any()) {
                string path = _fileStack.Pop();
                System.Diagnostics.Debug.WriteLine(path);
                ImageFormat format = DetectFormat(path);
                if (format != null) yield return new LocalFileSubmissionWrapper(path);
            }
        }

        protected override Task<InternalFetchResult> InternalFetchAsync(int? startPosition, int count) {
            var wrappers = Wrap().Take(count).ToList();
            var isEnded = _fileStack?.Any() != true;
            return Task.FromResult(new InternalFetchResult(wrappers, 0, isEnded));
        }
    }

    public class LocalFileSubmissionWrapper : ISubmissionWrapper {
        private readonly string _path;
        public LocalFileSubmissionWrapper(string path) {
            _path = path;
        }

        public string Title => Path.GetFileNameWithoutExtension(_path);
        public string HTMLDescription => "";
        public bool Mature => false;
		public bool Adult => false;
        public IEnumerable<string> Tags => Enumerable.Empty<string>();
        public DateTime Timestamp => File.GetCreationTime(_path);
        public string ViewURL => null;
        public string ImageURL => "file:///" + _path.Replace('\\', '/');
        public string ThumbnailURL => "file:///" + _path.Replace('\\', '/');
        public Color? BorderColor => null;
    }
}
