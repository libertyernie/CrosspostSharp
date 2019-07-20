namespace SourceWrappers.Eclipse

open FSharp.Data

type GalleryContentsResponse = JsonProvider<"Eclipse/gallery-contents.json", SampleIsList=true>
