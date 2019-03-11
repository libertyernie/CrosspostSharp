namespace FurAffinityFs.Requests

module Gallery =
    open FSharp.Data
    open FurAffinityFs

    type Folder = Gallery = 0 | Scraps = 1 | Favorites = 2

    type internal GalleryHtml = HtmlProvider<"https://www.furaffinity.net/gallery/lizard-socks/">

    let AsyncExecute (credentials: IFurAffinityCredentials) (folder: Folder) (username: string) = async {
        let folder_path =
            match folder with
            | Folder.Gallery -> "gallery"
            | Folder.Scraps -> "scraps"
            | Folder.Favorites -> "favorites"
            | _ -> invalidArg "folder" "Invalid folder type"
        let! html =
            sprintf "/%s/%s/" folder_path username
            |> Shared.AsyncGetHtml credentials
        let page = GalleryHtml.Parse html
        let figures = page.Html.CssSelect("figure")
        return seq {
            for f in figures do
                match f.AttributeValue "id" with
                | Shared.HasPrefix "sid-" (Shared.CanParseInt sid) -> yield sid
                | _ -> ()
        }
    }

    let ExecuteAsync credentials folder username =
        AsyncExecute credentials folder username
        |> Async.StartAsTask