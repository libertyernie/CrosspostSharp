namespace SourceWrappers.Eclipse

open FSharp.Data

type GalleryContentsResponse = JsonProvider<"Eclipse/gallery-contents.json", SampleIsList=true>
type ExtendedFetchResponse = JsonProvider<"Eclipse/extended-fetch.json", SampleIsList=true>
