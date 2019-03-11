namespace FurAffinityFs.Requests

module Gallery =
    open FSharp.Data
    open FurAffinityFs
    open FurAffinityFs.Models

    type Folder = Gallery = 0 | Scraps = 1 | Favorites = 2

    type internal GalleryHtml = HtmlProvider<"https://www.furaffinity.net/gallery/lizard-socks/">

    let GetFirstPageHref (folder: Folder) (username: string) =
        let folder_path =
            match folder with
            | Folder.Gallery -> "gallery"
            | Folder.Scraps -> "scraps"
            | Folder.Favorites -> "favorites"
            | _ -> invalidArg "folder" "Invalid folder type"
        sprintf "/%s/%s/" folder_path username

    let AsyncExecute (credentials: IFurAffinityCredentials) (page_href: string) = async {
        let! html = Shared.AsyncGetHtml credentials page_href
        let page = GalleryHtml.Parse html
        return {
            submissions = seq {
                let figures = page.Html.CssSelect("figure")
                for f in figures do
                    match f.AttributeValue "id" with
                    | Shared.HasPrefix "sid-" (Shared.CanParseInt sid) ->
                        let t =
                            (f.CssSelect "img" |> Seq.tryExactlyOne,
                             f.CssSelect "figcaption" |> Seq.tryExactlyOne)
                        match t with
                        | ( Some img, Some caption) ->
                            yield {
                                sid = sid
                                title = caption.InnerText()
                                href = sprintf "/view/%d" sid |> Shared.ToUri
                                thumbnail = img.AttributeValue "src" |> Shared.ToUri
                            }
                        | _ -> ()
                    | _ -> ()
            }
            next_page_href =
                page.Html.CssSelect "a.button-link.right"
                |> Seq.map (fun e -> e.AttributeValue "href")
                |> Seq.tryHead
        }
    }

    let ExecuteAsync credentials page_href =
        AsyncExecute credentials page_href
        |> Async.StartAsTask