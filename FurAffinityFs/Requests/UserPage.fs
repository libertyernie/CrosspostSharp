namespace FurAffinityFs.Requests

module UserPage =
    open FSharp.Data
    open FurAffinityFs
    open FurAffinityFs.Models

    type internal UserPageHtml = HtmlProvider<"https://www.furaffinity.net/user/lizard-socks/">

    let AsyncExecute (credentials: IFurAffinityCredentials) (username: string) = async {
        let! html = Shared.AsyncGetHtml credentials (sprintf "/user/%s" username)
        let page = UserPageHtml.Parse html
        let avatars = seq {
            let regex = System.Text.RegularExpressions.Regex """<img class="user-nav-avatar" alt="[^"]+" src="([^"]+)"""
            let m = regex.Match html
            if m.Success then
                yield m.Groups.[1].Value

            yield!
                page.Html.CssSelect("img.avatar")
                |> Seq.map (fun e -> e.AttributeValue "src")
        }
        return {
            avatar =
                avatars
                |> Seq.head
                |> Shared.ToUri
        }
    }

    let ExecuteAsync credentials username =
        AsyncExecute credentials username
        |> Async.StartAsTask