namespace DeviantArtFs

open DeviantArtFs.RequestTypes
open FSharp.Data
open System.Net
open System.IO
open System

type DeviantArtBaseResponse = JsonProvider<"""{"status":"error"}""">

type DeviantArtErrorResponse = JsonProvider<"""{"error":"invalid_request","error_description":"Must provide an access_token to access this resource.","status":"error"}""">

type DeviantArtException(resp: WebResponse, body: DeviantArtErrorResponse.Root) =
    inherit Exception(body.ErrorDescription)

    member __.ResponseBody = body
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type DeviantArtGalleryFoldersResponse = JsonProvider<"""[{
    "results": [
        {
            "folderid": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
            "parent": null,
            "name": "Featured",
            "size": 2
        },
        {
            "folderid": "E431BAFB-7A00-7EA1-EED7-2EF9FA0F04CE",
            "parent": "47D47436-5683-8DF2-EEBF-2A6760BE1336",
            "name": "My New Gallery"
        }
    ],
    "has_more": true,
    "next_offset": 2
}, {
    "results": [],
    "has_more": false,
    "next_offset": null
}]""", SampleIsList=true>

type StashSubmitResponse = JsonProvider<"""{
    "status": "success",
    "itemid": 123456789876,
    "stack": "Stash Uploads 1",
    "stackid": 12345678987
}""">

module internal dafs =
    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

    let urlEncode = WebUtility.UrlEncode

    let userAgent = "DeviantArtFs/0.1 (https://github.com/libertyernie/CrosspostSharp)"
    let createRequest (token: IDeviantArtAccessToken) (url: string) =
        let req = WebRequest.CreateHttp url
        req.UserAgent <- userAgent
        req.Headers.["Authorization"] <- sprintf "Bearer %s" token.AccessToken
        req
    let asyncRead (req: WebRequest) = async {
        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = DeviantArtBaseResponse.Parse json
            if obj.Status = "error" then
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
            else
                return json
        with
            | :? WebException as ex ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
    }

type DeviantArtClient(token: IDeviantArtAccessToken) =
    let current_token = token

    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

    let userAgent = "DeviantArtFs/0.1 (https://github.com/libertyernie/CrosspostSharp)"
    let createRequest (url: string) =
        let req = WebRequest.CreateHttp url
        req.UserAgent <- userAgent
        req.Headers.["Authorization"] <- sprintf "Bearer %s" current_token.AccessToken
        req
    let asyncRead (req: WebRequest) = async {
        try
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())
            let! json = sr.ReadToEndAsync() |> Async.AwaitTask
            let obj = DeviantArtBaseResponse.Parse json
            if obj.Status = "error" then
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
            else
                return json
        with
            | :? WebException as ex ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
    }

    member __.AsyncGalleryFolders (ps: GalleryFoldersRequest) = async {
        let query = seq {
            match Option.ofObj ps.Username with
            | Some s -> yield sprintf "username=%s" (WebUtility.UrlEncode s)
            | None -> ()
            match Option.ofNullable ps.CalculateSize with
            | Some s -> yield sprintf "calculate_size=%b" s
            | None -> ()
            match Option.ofNullable ps.Offset with
            | Some s -> yield sprintf "offset=%d" s
            | None -> ()
            match Option.ofNullable ps.Limit with
            | Some s -> yield sprintf "limit=%d" s
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/folders?%s"
            |> createRequest
        let! json = asyncRead req
        return DeviantArtGalleryFoldersResponse.Parse json
    }

    member __.AsyncStashSubmit (ps: StashSubmitRequest) = async {
        // multipart separators
        let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
        let h2 = sprintf "--%s" h1
        let h3 = sprintf "--%s--" h1

        let req = createRequest "https://www.deviantart.com/api/v1/oauth2/stash/submit"
        req.Method <- "POST"
        req.ContentType <- sprintf "multipart/form-data; boundary=%s" h1

        do! async {
            use ms = new MemoryStream()
            let w (s: string) =
                let bytes = System.Text.Encoding.UTF8.GetBytes(sprintf "%s\n" s)
                ms.Write(bytes, 0, bytes.Length)
            
            match Option.ofObj ps.Title with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"title\""
                w ""
                w s
            | None -> ()

            match Option.ofObj ps.ArtistComments with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"artist_comments\""
                w ""
                w s
            | None -> ()

            match Option.ofObj ps.Tags with
            | Some s ->
                let mutable index = 0
                for t in s do
                    w h2
                    w (sprintf "Content-Disposition: form-data; name=\"tags[%d]\"" index)
                    w ""
                    w t
                    index <- index + 1
            | None -> ()

            match Option.ofObj ps.OriginalUrl with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"original_url\""
                w ""
                w s
            | None -> ()

            match Option.ofNullable ps.IsDirty with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"is_dirty\""
                w ""
                w (sprintf "%b" s)
            | None -> ()

            match Option.ofNullable ps.ItemId with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"itemid\""
                w ""
                w (sprintf "%d" s)
            | None -> ()

            match Option.ofObj ps.Stack with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"stack\""
                w ""
                w s
            | None -> ()

            match Option.ofNullable ps.StackId with
            | Some s ->
                w h2
                w "Content-Disposition: form-data; name=\"stackid\""
                w ""
                w (sprintf "%d" s)
            | None -> ()

            w h2
            w (sprintf "Content-Disposition: form-data; name=\"submission\"; filename=\"%s\"" ps.Filename)
            w (sprintf "Content-Type: %s" ps.ContentType)
            w ""
            ms.Flush()
            ms.Write(ps.Data, 0, ps.Data.Length)
            ms.Flush()
            w ""
            w h3

            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            ms.Position <- 0L
            do! ms.CopyToAsync(reqStream) |> Async.AwaitTask

            System.IO.File.WriteAllBytes("C:/Users/admin/out.txt", ms.ToArray())
        }

        let! json = asyncRead req
        let o = StashSubmitResponse.Parse json
        return o.Itemid
    }

    member this.GalleryFoldersAsync(ps) = this.AsyncGalleryFolders ps |> whenDone (fun r -> r.Results |> Seq.map (fun f -> (f.Folderid, f.Name)) |> dict) |> Async.StartAsTask
    member this.StashSubmitAsync(ps) = this.AsyncStashSubmit ps |> Async.StartAsTask

    interface IDeviantArtAccessToken with
        member this.AccessToken = current_token.AccessToken