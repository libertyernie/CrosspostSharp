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

type DeviantArtStatusPostResponse = JsonProvider<"""{
    "statusid": "85B05A5E-7773-9AAF-CEC7-88F522A5AF5B"
}""">

type DeviantArtMetadataResponse = JsonProvider<"""{
    "metadata": [
        {
            "deviationid": "928DC00B-9A4E-AC29-BDEE-D985BD2CF16F",
            "printid": null,
            "author": {
                "userid": "899C73B5-347B-72C1-2F63-289173EEC881",
                "username": "chris",
                "usericon": "https://a.deviantart.net/avatars/c/h/chris.jpg?3",
                "type": "regular"
            },
            "is_watching": false,
            "title": "deviantART Server",
            "description": "A deviantART server that we added today.",
            "license": "No License",
            "allows_comments": true,
            "tags": [
                {
                    "tag_name": "server",
                    "sponsored": false,
                    "sponsor": ""
                }
            ],
            "is_mature": false,
            "is_favourited": false
        },
        {
            "deviationid": "0E366EC1-95C5-23DB-DD08-A0EDF900B28E",
            "printid": "DDADA407-0DC6-879C-B7F8-21E225B2BD4B",
            "author": {
                "userid": "FC908DE8-78A8-4CEE-A92D-C84D7305F4AA",
                "username": "micahgoulart",
                "usericon": "https://a.deviantart.net/avatars/m/i/micahgoulart.jpg?1",
                "type": "regular"
            },
            "is_watching": false,
            "title": "Back Tomorrow",
            "description": "if you miss a sunset, looking on the  bright side of it (pun not  intentional), there is always another  one tomorrow, especially if you live in  central Brazil, where the sunsets  almost make you stay for the sunrise.<br />\n<br />\nthe silhoutte is of some banana trees  in my backyard at home in Brazil.",
            "license": "No License",
            "allows_comments": true,
            "tags": [
                {
                    "tag_name": "brasil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "brazil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "color",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorful",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colors",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "dusk",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "dust",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "orange",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "red",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "set",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sky",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sun",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorred",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorssky",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "redorange",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "setsunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sunsetsunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "orangedust",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "brazilbrasil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorfulcolor",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "skycolorful",
                    "sponsored": false,
                    "sponsor": ""
                }
            ],
            "is_mature": false,
            "is_favourited": false
        }, {
            "deviationid": "928DC00B-9A4E-AC29-BDEE-D985BD2CF16F",
            "printid": null,
            "author": {
                "userid": "899C73B5-347B-72C1-2F63-289173EEC881",
                "username": "chris",
                "usericon": "https://a.deviantart.net/avatars/c/h/chris.jpg?3",
                "type": "regular"
            },
            "is_watching": false,
            "title": "deviantART Server",
            "description": "A deviantART server that we added today.",
            "license": "No License",
            "allows_comments": true,
            "tags": [
                {
                    "tag_name": "server",
                    "sponsored": false,
                    "sponsor": ""
                }
            ],
            "is_mature": false,
            "is_favourited": false,
            "submission": {
                "creation_time": "2004-02-24T11:42:47-0800",
                "category": "photography/civilization/industrial",
                "file_size": "166 KB",
                "resolution": "1024x768",
                "submitted_with": {
                    "app": "Unknown App",
                    "url": ""
                }
            },
            "stats": {
                "views": 15966,
                "views_today": 3,
                "favourites": 51,
                "comments": 196,
                "downloads": 2125,
                "downloads_today": 0
            },
            "camera": {
                "make": "Canon",
                "model": "Canon PowerShot G5",
                "shutter_speed": "1/60 second",
                "aperture": "F/2.0",
                "focal_length": "7 mm",
                "date_taken": "Feb 18, 2004, 12:45:46 AM"
            },
            "collections": []
        },
        {
            "deviationid": "0E366EC1-95C5-23DB-DD08-A0EDF900B28E",
            "printid": "DDADA407-0DC6-879C-B7F8-21E225B2BD4B",
            "author": {
                "userid": "FC908DE8-78A8-4CEE-A92D-C84D7305F4AA",
                "username": "micahgoulart",
                "usericon": "https://a.deviantart.net/avatars/m/i/micahgoulart.jpg?1",
                "type": "regular"
            },
            "is_watching": false,
            "title": "Back Tomorrow",
            "description": "if you miss a sunset, looking on the  bright side of it (pun not  intentional), there is always another  one tomorrow, especially if you live in  central Brazil, where the sunsets  almost make you stay for the sunrise.<br />\n<br />\nthe silhoutte is of some banana trees  in my backyard at home in Brazil.",
            "license": "No License",
            "allows_comments": true,
            "tags": [
                {
                    "tag_name": "brasil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "brazil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "color",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorful",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colors",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "dusk",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "dust",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "orange",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "red",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "set",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sky",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sun",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorred",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorssky",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "redorange",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "setsunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "sunsetsunset",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "orangedust",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "brazilbrasil",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "colorfulcolor",
                    "sponsored": false,
                    "sponsor": ""
                },
                {
                    "tag_name": "skycolorful",
                    "sponsored": false,
                    "sponsor": ""
                }
            ],
            "is_mature": false,
            "is_favourited": false,
            "submission": {
                "creation_time": "2004-01-10T13:39:24-0800",
                "category": "photography/nature/sky",
                "file_size": "194 KB",
                "resolution": "768x1024",
                "submitted_with": {
                    "app": "Unknown App",
                    "url": ""
                }
            },
            "stats": {
                "views": 3950,
                "views_today": 2,
                "favourites": 133,
                "comments": 119,
                "downloads": 998,
                "downloads_today": 0
            },
            "collections": []
        }
    ]
}""">

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

type DeviantArtStatusPostParameters = {
    Body: string
    StatusId: Nullable<Guid>
    ParentId: Nullable<Guid>
    StashId: Nullable<int64>
}

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

    member __.AsyncUserStatusesPost (ps: DeviantArtStatusPostParameters) = async {
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
        let req = createRequest "https://www.deviantart.com/api/v1/oauth2/user/statuses/post"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }
        let! json = asyncRead req
        let result = DeviantArtStatusPostResponse.Parse json
        return result.Statusid
    }

    member __.AsyncDeviationMetadata ext_submission ext_camera ext_stats ext_collection (deviationids: seq<Guid>) = async {
        let query = seq {
            match ext_submission with
            | Some s -> yield sprintf "username=%b" s
            | None -> ()
            match ext_camera with
            | Some s -> yield sprintf "ext_camera=%b" s
            | None -> ()
            match ext_stats with
            | Some s -> yield sprintf "ext_stats=%b" s
            | None -> ()
            match ext_collection with
            | Some s -> yield sprintf "ext_collection=%b" s
            | None -> ()
            yield deviationids |> Seq.map (fun o -> o.ToString()) |> String.concat "," |> sprintf "deviationids[]=%s"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/metadata?%s"
            |> createRequest
        let! json = asyncRead req
        return DeviantArtMetadataResponse.Parse json
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

    member this.UserStatusesPostAsync(ps) = this.AsyncUserStatusesPost ps |> Async.StartAsTask
    member this.GalleryFoldersAsync(ps) = this.AsyncGalleryFolders ps |> whenDone (fun r -> r.Results |> Seq.map (fun f -> (f.Folderid, f.Name)) |> dict) |> Async.StartAsTask
    member this.StashSubmitAsync(ps) = this.AsyncStashSubmit ps |> Async.StartAsTask

    interface IDeviantArtAccessToken with
        member this.AccessToken = current_token.AccessToken