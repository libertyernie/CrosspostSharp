namespace DeviantArtFs.Gallery

open DeviantArtFs
open FSharp.Data

type GalleryFoldersResponse = JsonProvider<"""[{
    "results": [
        {
            "folderid": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
            "parent": null,
            "name": "Featured",
            "size": 2
        },
        {
            "folderid": "E431BAFB-7A00-7EA1-EED7-2EF9FA0F04CE",
            "parent": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
            "name": "My New Gallery"
        }
    ],
    "has_more": true,
    "next_offset": 2
}, {
    "results": [],
    "has_more": false,
    "next_offset": null
}]""", SampleIsList=true>

type GalleryFoldersRequest() =
    member val Username = null with get, set
    member val CalculateSize = false with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Folders =
    let AsyncExecute token (ps: GalleryFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (dafs.urlEncode s)
            | None -> ()
            yield sprintf "calculate_size=%b" ps.CalculateSize
            yield sprintf "offset=%d" ps.Offset
            yield sprintf "limit=%d" ps.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return GalleryFoldersResponse.Parse json
    }

    let ExecuteAsync token ps =
        AsyncExecute token ps
        |> dafs.whenDone (fun r -> r.Results |> Seq.map (fun f -> (f.Folderid, f.Name)) |> dict)
        |> Async.StartAsTask