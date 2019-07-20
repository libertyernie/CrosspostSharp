namespace SourceWrappers.Eclipse

open System
open System.IO
open System.Net
open FSharp.Control

type GalleryContentsRequest(username: string, ?offset: int) =
    member __.Username = username
    member __.Offset = offset |> Option.defaultValue 0
    member val Limit: int option = None with get, set
    member val FolderId: int option = None with get, set
    member val ScrapsFolder = false with get, set

module GalleryContents =
    let AsyncGet (req: GalleryContentsRequest) = async {
        let query = seq {
            yield req.Username |> Uri.EscapeDataString |> sprintf "username=%s"
            yield req.Offset |> sprintf "offset=%d"
            match req.Limit with
            | Some l -> yield l |> sprintf "limit=%d"
            | None -> ()
            match req.FolderId with
            | Some f -> yield f |> sprintf "folderid=%d"
            | None -> ()
            match req.ScrapsFolder with
            | true -> yield "scraps_folder=true"
            | false -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/_napi/da-user-profile/api/gallery/contents?%s"
            |> WebRequest.CreateHttp
        req.UserAgent <- "SourceWrapper/0.0 (https://github.com/libertyernie/CrosspostSharp)"
        req.Accept <- "application/json"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let data = GalleryContentsResponse.Parse json
        return data
    }

    let AsyncGetAllResults (req: GalleryContentsRequest) = asyncSeq {
        let mutable next = req.Offset
        let mutable finished = false
        let mutable errors = 0
        while not finished do
            try
                let r = new GalleryContentsRequest(req.Username, next)
                r.Limit <- req.Limit
                r.FolderId <- req.FolderId
                r.ScrapsFolder <- req.ScrapsFolder
                let! data = AsyncGet r
                for x in data.Results do
                    yield x.Deviation
                if data.HasMore then
                    next <- data.NextOffset
                else
                    finished <- true
                errors <- 0
            with
                | :? WebException as ex ->
                    errors <- errors + 1
                    if errors >= 3 then
                        raise (new Exception("Could not load gallery contents from DeviantArt", ex))
                    else
                        do! Async.Sleep 2000
    }