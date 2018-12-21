namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System

type StatusPostResponse = JsonProvider<"""{
    "statusid": "85B05A5E-7773-9AAF-CEC7-88F522A5AF5B"
}""">

type StatusPostRequest(body: string) =
    member __.Body = body
    member val StatusId = Nullable<Guid>() with get, set
    member val ParentId = Nullable<Guid>() with get, set
    member val StashId = Nullable<int64>() with get, set

module StatusPost =
    open System.IO

    let AsyncExecute token (ps: StatusPostRequest) = async {
        let query = seq {
            match Option.ofObj ps.Body with
            | Some s -> yield sprintf "body=%s" s
            | None -> ()
            match Option.ofNullable ps.ParentId with
            | Some s -> yield sprintf "parentid=%O" s
            | None -> ()
            match Option.ofNullable ps.StatusId with
            | Some s -> yield sprintf "id=%O" s
            | None -> ()
            match Option.ofNullable ps.StashId with
            | Some s -> yield sprintf "stashid=%O" s
            | None -> ()
        }
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = dafs.asyncRead req
        let result = StatusPostResponse.Parse json
        return result.Statusid
    }

    let ExecuteAsync token ps = AsyncExecute token ps |> Async.StartAsTask