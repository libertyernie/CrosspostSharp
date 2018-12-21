namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data

type internal PublishCategoryTreeResponse = JsonProvider<"""{
    "categories": [
        {
            "catpath": "anthro",
            "title": "Anthro",
            "has_subcategory": true,
            "parent_catpath": "/"
        }
    ]
}""">

type PublishCategoryTreeResult = {
    Catpath: string
    Title: string
    HasSubcategory: bool
    ParentCatpath: string
}

type PublishCategoryTreeRequest() = 
    member val Catpath = "/" with get, set
    member val Filetype = null with get, set
    member val Frequent = false with get, set

module PublishCategoryTree =
    let AsyncExecute token (req: PublishCategoryTreeRequest) = async {
        let query = seq {
            yield sprintf "catpath=%s" (dafs.urlEncode req.Catpath)
            yield sprintf "filetype=%s" (dafs.urlEncode req.Filetype)
            yield sprintf "frequent=%b" req.Frequent
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/stash/publish/categorytree?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        let resp = PublishCategoryTreeResponse.Parse json
        return resp.Categories
            |> Seq.map (fun c -> {
                Catpath = c.Catpath
                Title = c.Title
                HasSubcategory = c.HasSubcategory
                ParentCatpath = c.ParentCatpath
            })
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask