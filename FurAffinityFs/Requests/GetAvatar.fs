namespace FurAffinityFs.Requests

module GetAvatar =
    open FurAffinityFs
    open System
    open System.IO
    open System.Text.RegularExpressions

    let AsyncExecute (credentials: IFurAffinityCredentials) (username: string) = async {
        let req = Shared.CreateRequest credentials (sprintf "/user/%s" username)
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        let! html = sr.ReadToEndAsync() |> Async.AwaitTask
        let m = Regex.Match(html, """img class="avatar"[^>]*src="([^"]+)""")
        return if m.Success
            then Some (Uri (req.RequestUri, m.Groups.[1].Value))
            else None
    }

    let ExecuteAsync credentials username =
        AsyncExecute credentials username
        |> Shared.AsyncOptionDefault null
        |> Async.StartAsTask