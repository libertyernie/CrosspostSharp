using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;

namespace FurryNetworkLib {
    public class SearchResults {
        public IEnumerable<Hit> Hits { get; set; }
        public IEnumerable<object> Tags { get; set; }
        public int Total { get; set; }

        public class Hit {
            public string _index { get; set; }
            public string _type { get; set; }
            public int _id { get; set; }
            public JObject _source { get; set; }
            public IEnumerable<object> _sort { get; set; }
            
            /// <summary>
            /// Information about the submission. (This object might be a specific subtype of Submission, such as Artwork or Journal.)
            /// </summary>
            public Submission Submission =>
                _type == "artwork" ? JsonConvert.DeserializeObject<Artwork>(_source.ToString())
                : _type == "photo" ? JsonConvert.DeserializeObject<Photo>(_source.ToString())
                : _type == "journal" ? JsonConvert.DeserializeObject<Journal>(_source.ToString())
                : _type == "multimedia" ? JsonConvert.DeserializeObject<Multimedia>(_source.ToString())
                : _type == "story" ? JsonConvert.DeserializeObject<Story>(_source.ToString())
                : JsonConvert.DeserializeObject<Submission>(_source.ToString());
        }
    }
}