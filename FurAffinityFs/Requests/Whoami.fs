namespace FurAffinityFs.Requests

module Whoami =
    open FSharp.Data
    open FurAffinityFs

    type internal HomePageHtml = HtmlProvider<"https://www.furaffinity.net/">

    let AsyncExecute (credentials: IFurAffinityCredentials) = async {
        let! html = Shared.AsyncGetHtml credentials "/"
        let page = HomePageHtml.Parse html
        let username =
            page.Html.CssSelect("#my-username")
            |> Seq.map (fun e -> e.InnerText())
            |> Seq.tryHead
        return
            match username with
            | Some u -> Shared.StripTilde u
            | None -> failwithf "Username not found on page (not logged in?)"
    }

    let ExecuteAsync credentials =
        AsyncExecute credentials
        |> Async.StartAsTask