namespace DeviantArtFs.Stash

open DeviantArtFs

module Delete =
    open System.IO
    open System.Threading.Tasks

    let AsyncExecute token (itemid: int64) = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/delete"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! sprintf "itemid=%d" itemid |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        return ignore json
    }

    let ExecuteAsync token itemid = AsyncExecute token itemid |> Async.StartAsTask :> Task