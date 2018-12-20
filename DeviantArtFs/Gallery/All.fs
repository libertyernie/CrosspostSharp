namespace DeviantArtFs.Gallery

open DeviantArtFs
open FSharp.Data

type internal GalleryResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "results": []
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type AllRequest() =
    member val Username = null with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module All =
    let AsyncExecute token (req: AllRequest) = async {
        let query = seq {
            match Option.ofObj req.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/all?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = GalleryResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield DeviantArtFs.Deviation.IdResponse.Parse json
            }
        }
    }