namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data

type internal StatusesResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 3,
    "results": []
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

type StatusesRequest(username: string) =
    member __.Username = username
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module Statuses =
    let AsyncExecute token (req: StatusesRequest) = async {
        let query = seq {
            yield sprintf "username=%s" (dafs.urlEncode req.Username)
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let o = StatusesResponse.Parse json
        return {
            HasMore = o.HasMore
            NextOffset = o.NextOffset
            Results = seq {
                for element in o.Results do
                    let json = element.JsonValue.ToString()
                    yield StatusesIdResponse.Parse json
            }
        }
    }