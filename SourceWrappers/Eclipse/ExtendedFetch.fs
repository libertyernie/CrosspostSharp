namespace SourceWrappers.Eclipse

open System.Net
open System.IO

module ExtendedFetch =
    let AsyncGet (deviation_id: int) = async {
        let query = seq {
            yield deviation_id |> sprintf "deviationid=%d"
            yield "type=art"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/_napi/da-user-profile/shared_api/deviation/extended_fetch?%s"
            |> WebRequest.CreateHttp
        req.UserAgent <- "SourceWrapper/0.0 (https://github.com/libertyernie/CrosspostSharp)"
        req.Accept <- "application/json"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let data = ExtendedFetchResponse.Parse json
        return data
    }