namespace FurAffinityFs.Requests

module FurAffinityWhoamiRequest =
    open FurAffinityFs
    open System.IO
    open System.Text.RegularExpressions

    let AsyncExecute (credentials: IFurAffinityCredentials) = async {
        let req = Shared.CreateRequest credentials "/"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        let! html = sr.ReadToEndAsync() |> Async.AwaitTask
        let m = Regex.Match(html, """id="my-username"[^>]*>~([^<]+)""")
        return if m.Success
            then m.Groups.[1].Value
            else failwithf "Username not found on page (not logged in?)"
    }

    let ExecuteAsync credentials =
        AsyncExecute credentials
        |> Async.StartAsTask