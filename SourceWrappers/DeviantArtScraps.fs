namespace SourceWrappers

open FSharp.Control
open DeviantArtFs
open System
open System.Net
open System.IO
open Newtonsoft.Json

type DeviantArtScrapsWrapper(client: IDeviantArtAccessToken, username: string, includeLiterature: bool) =
    inherit AsyncSeqWrapper()

    let fetch_batch offset limit = async {
        let req =
            sprintf "https://www.deviantart.com/_napi/da-user-profile/api/gallery/contents?username=%s&offset=%d&limit=%d&scraps_folder=true&mode=newest" (Uri.EscapeDataString username) offset limit
            |> WebRequest.CreateHttp
        req.UserAgent <- "SourceWrapper/0.0 (https://github.com/libertyernie/CrosspostSharp)"
        req.Accept <- "application/json"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let sample =
            {|
                hasMore = true
                nextOffset = Nullable 1
                results = [
                    {|
                        deviation = {| deviationid = 0L |}
                    |}
                ]
            |}
        return JsonConvert.DeserializeAnonymousType(json, sample)
    }

    let rec fetch_batches offset limit = asyncSeq {
        let! batch = fetch_batch offset limit
        for r in batch.results do
            yield r.deviation.deviationid
        if batch.hasMore then
            yield! fetch_batches batch.nextOffset.Value limit
    }

    let fetch_api_id internal_id = async {
        let req =
            sprintf "https://www.deviantart.com/_napi/shared_api/deviation/extended_fetch?deviationid=%d&username=%s&type=art&include_session=false" internal_id (Uri.EscapeDataString username)
            |> WebRequest.CreateHttp
        req.UserAgent <- "CrosspostSharp/0.0 (https://github.com/libertyernie/CrosspostSharp)"
        req.Accept <- "application/json"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let sample =
            {|
                deviation =
                    {|
                        extended = {| deviationUuid = Guid.Empty |}
                    |}
            |}
        let obj = JsonConvert.DeserializeAnonymousType(json, sample)
        return obj.deviation.extended.deviationUuid
    }

    override __.Name = "DeviantArt (scraps)"

    override __.FetchSubmissionsInternal() =
        fetch_batches 0 24
        |> AsyncSeq.mapAsync fetch_api_id
        |> AsyncSeq.mapAsync (DeviantArtFs.Requests.Deviation.DeviationById.AsyncExecute client)
        |> AsyncSeq.filter (fun d -> includeLiterature || Option.isSome d.content)
        |> AsyncSeq.map (fun d -> new DeviantArtDeferredPostWrapper(d, client) :> IPostBase)

    override __.FetchUserInternal() = async {
        let! u =
            new DeviantArtFs.Requests.User.ProfileByNameRequest(username)
            |> DeviantArtFs.Requests.User.ProfileByName.AsyncExecute client
        return {
            username = u.user.username
            icon_url = Some u.user.usericon
        }
    }