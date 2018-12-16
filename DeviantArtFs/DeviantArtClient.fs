namespace DeviantArtFs

open FSharp.Data
open System.Net
open System.IO
open System

type DeviantArtErrorResponse = JsonProvider<"""{"error":"invalid_request","error_description":"Must provide an access_token to access this resource.","status":"error"}""">

type DeviantArtException(resp: WebResponse, body: DeviantArtErrorResponse.Root) =
    inherit Exception(body.ErrorDescription)

    member __.ResponseBody = body
    member __.StatusCode =
        match resp with
        | :? HttpWebResponse as h -> Nullable h.StatusCode
        | _ -> Nullable()

type DeviantArtUser = JsonProvider<"""[
{
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular",
    "details": {
        "sex": "m",
        "age": 29,
        "joindate": "2014-10-17T08:01:00-0700"
    },
    "geo": {
        "country": "Australia",
        "countryid": 15,
        "timezone": "America/Los_Angeles"
    },
    "profile": {
        "user_is_artist": true,
        "artist_level": "Professional",
        "artist_speciality": "Film & Animation",
        "real_name": "John Doe",
        "tagline": "My tagline",
        "website": "http://example.com",
        "cover_photo": "TODO",
        "profile_pic": {
            "deviationid": "891F8FA9-780C-03B8-56C1-577FE599E118",
            "printid": null,
            "url": "https://verycoolusername.deviantart.lan/art/Profile-picture-488579198",
            "title": "Profile picture",
            "category": "Uncategorized",
            "category_path": "",
            "is_favourited": false,
            "is_deleted": false,
            "author": {
                "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
                "username": "verycoolusername",
                "usericon": "https://a.deviantart.net/avatars/default.gif",
                "type": "regular"
            },
            "stats": {
                "comments": 0,
                "favourites": 0
            },
            "published_time": 1413558297,
            "allows_comments": true,
            "content": {
                "src": "https://fc07.deviantart.net/fs70/f/2014/290/8/2/profile_picture_by_verycoolusername-d82vy4e.jpg",
                "height": 224,
                "width": 300
            },
            "thumbs": [
                {
                    "src": "https://th03.deviantart.net/fs70/150/f/2014/290/8/2/profile_picture_by_verycoolusername-d82vy4e.jpg",
                    "height": 112,
                    "width": 150
                },
                {
                    "src": "https://th08.deviantart.net/fs70/200H/f/2014/290/8/2/profile_picture_by_verycoolusername-d82vy4e.jpg",
                    "height": 200,
                    "width": 268
                }
            ]
        }
    },
    "stats": {
        "watchers": 0,
        "friends": 0
    }
}, {
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular"
}]""", SampleIsList=true>

type DeviantArtDeviation = JsonProvider<"""[
{
    "deviationid": "F9921FC4-3A0F-90A9-3625-AA8E105747AD",
    "printid": null,
    "url": "https://justgalym.deviantart.com/journal/Another-post-written-in-stash-446384730",
    "title": "Another post written in stash",
    "category": "Personal Journal",
    "category_path": "personaljournal",
    "is_favourited": false,
    "is_deleted": false,
    "author": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "admin"
    },
    "stats": {
        "comments": 0,
        "favourites": 0
    },
    "published_time": 1397059526,
    "allows_comments": true,
    "excerpt": "I'm checking out stash writer",
    "thumbs": []
}, {
    "deviationid": "52BAFA97-9DB9-0E5F-FF2D-C39083F89817",
    "printid": null,
    "url": "https://justgalym.deviantart.com/art/Astana-Hotel-487117484",
    "title": "Astana Hotel",
    "category": "City Life",
    "category_path": "photography/civilization/city",
    "is_favourited": false,
    "is_deleted": false,
    "author": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "admin"
    },
    "stats": {
        "comments": 0,
        "favourites": 1
    },
    "published_time": 1412745603,
    "allows_comments": true,
    "content": {
        "src": "https://fc01.deviantart.net/fs70/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
        "height": 575,
        "width": 1024
    },
    "thumbs": [
        {
            "src": "https://th03.deviantart.net/fs70/150/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
            "height": 84,
            "width": 150
        },
        {
            "src": "https://th02.deviantart.net/fs70/200H/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
            "height": 169,
            "width": 300
        }
    ]
}
]""", SampleIsList=true>

type DeviantArtGalleryResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 2,
    "results": [
    {
        "deviationid": "F9921FC4-3A0F-90A9-3625-AA8E105747AD",
        "printid": null,
        "url": "https://justgalym.deviantart.com/journal/Another-post-written-in-stash-446384730",
        "title": "Another post written in stash",
        "category": "Personal Journal",
        "category_path": "personaljournal",
        "is_favourited": false,
        "is_deleted": false,
        "author": {
            "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "admin"
        },
        "stats": {
            "comments": 0,
            "favourites": 0
        },
        "published_time": 1397059526,
        "allows_comments": true,
        "excerpt": "I'm checking out stash writer",
        "thumbs": []
    }, {
        "deviationid": "52BAFA97-9DB9-0E5F-FF2D-C39083F89817",
        "printid": null,
        "url": "https://justgalym.deviantart.com/art/Astana-Hotel-487117484",
        "title": "Astana Hotel",
        "category": "City Life",
        "category_path": "photography/civilization/city",
        "is_favourited": false,
        "is_deleted": false,
        "author": {
            "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "admin"
        },
        "stats": {
            "comments": 0,
            "favourites": 1
        },
        "published_time": 1412745603,
        "allows_comments": true,
        "content": {
            "src": "https://fc01.deviantart.net/fs70/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
            "height": 575,
            "width": 1024
        },
        "thumbs": [
            {
                "src": "https://th03.deviantart.net/fs70/150/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
                "height": 84,
                "width": 150
            },
            {
                "src": "https://th02.deviantart.net/fs70/200H/i/2014/280/8/7/astana_hotel_by_justgalym-d820m98.jpg",
                "height": 169,
                "width": 300
            }
        ]
    }]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

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
            return! sr.ReadToEndAsync() |> Async.AwaitTask
        with
            | :? WebException as ex ->
                use resp = ex.Response
                use sr = new StreamReader(resp.GetResponseStream())
                let! json = sr.ReadToEndAsync() |> Async.AwaitTask
                let error_obj = DeviantArtErrorResponse.Parse json
                return raise (new DeviantArtException(resp, error_obj))
    }

    member __.AsyncUserWhoami () = async {
        let req = createRequest "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = asyncRead req
        return DeviantArtUser.Parse json
    }

    member __.AsyncGalleryAll (?username: string, ?offset: int, ?limit: int) = async {
        let query = seq {
            match username with
            | Some s -> yield sprintf "username=%s" (WebUtility.UrlEncode s)
            | None -> ()
            match offset with
            | Some s -> yield sprintf "offset=%d" s
            | None -> ()
            match limit with
            | Some s -> yield sprintf "limit=%d" s
            | None -> ()
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/gallery/all?%s"
            |> createRequest
        let! json = asyncRead req
        return DeviantArtGalleryResponse.Parse json
    }

    member this.GetUsernameAsync() = this.AsyncUserWhoami() |> whenDone (fun u -> u.Username) |> Async.StartAsTask
    member this.GetGalleryThumbnailsAsync() = this.AsyncGalleryAll() |> whenDone (fun g -> g.Results |> Seq.map (fun d -> d.Thumbs |> Seq.map (fun t -> t.Src) |> Seq.head)) |> Async.StartAsTask