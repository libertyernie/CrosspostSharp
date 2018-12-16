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

type DeviantArtWhoamiRepsonse = JsonProvider<"""{
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular"
}""">

type DeviantArtUserProfileResponse = JsonProvider<"""[{
    "user": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "regular"
    },
    "is_watching": false,
    "profile_url": "https://justgalym.deviantart.com",
    "user_is_artist": false,
    "artist_level": "Professional",
    "artist_specialty": "Traditional Art",
    "real_name": "Galymzhan",
    "tagline": "my tagline",
    "countryid": 110,
    "country": "Kazakhstan",
    "website": "http://bytespree.com",
    "bio": "my bio",
    "cover_photo": "",
    "profile_pic": {
        "deviationid": "74B294D1-5B80-8C51-A98A-5E0ABB54F4EB",
        "printid": null,
        "url": "https://justgalym.deviantart.com/art/Profile-picture-479816543",
        "title": "Profile picture",
        "category": "Uncategorized",
        "category_path": "",
        "is_downloadable": true,
        "is_mature": false,
        "is_favourited": false,
        "is_deleted": false,
        "author": {
            "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "regular"
        },
        "stats": {
            "comments": 0,
            "favourites": 0
        },
        "published_time": 1409666205,
        "allows_comments": true,
        "content": {
            "src": "https://fc00.deviantart.net/fs70/f/2014/245/a/b/profile_picture_by_justgalym-d7xo4tb.png",
            "filesize": 1000,
            "height": 300,
            "width": 300,
            "transparency": false
        },
        "thumbs": [
            {
                "src": "https://th09.deviantart.net/fs70/150/f/2014/245/a/b/profile_picture_by_justgalym-d7xo4tb.png",
                "height": 150,
                "width": 150,
                "transparency": false
            },
            {
                "src": "https://th03.deviantart.net/fs70/200H/f/2014/245/a/b/profile_picture_by_justgalym-d7xo4tb.png",
                "height": 200,
                "width": 200,
                "transparency": false
            }
        ]
    },
    "last_status": {
        "statusid": "C2967EDD-B16C-760C-9A53-263DEC8F0A36",
        "body": "<a class=\"discoverytag\" href=\"https://www.deviantart.com/tag/foundonnewest\" data-canonical-tag=\"foundonnewest\">#FoundOnNewest</a>&nbsp;<br /><br /><span class=\"shadow-holder\" data-embed-type=\"deviation\" data-embed-id=\"487110297\" data-embed-format=\"thumb\"><span class=\"tt-fh-tc\" style=\"width: 133px;\"><span class=\"tt-bb\" style=\"width: 133px;\"></span><span class=\"shadow mild\" style=\"width: 133px;\"><a class=\"thumb\" href=\"https://debyauliaa.deviantart.com/art/Autumn-487110297\" title=\"Autumn by debyauliaa, Oct 8, 2014 in Photography &gt; People &amp; Portraits &gt; Classic Portraits\" data-super-img=\"https://th04.deviantart.net/fs71/PRE/i/2014/280/5/3/autumn_by_debyauliaa-d820gpl.jpg\" data-super-width=\"730\" data-super-height=\"1095\" data-super-transparent=\"false\" data-super-full-img=\"https://fc03.deviantart.net/fs71/i/2014/280/5/3/autumn_by_debyauliaa-d820gpl.jpg\" data-super-full-width=\"1024\" data-super-full-height=\"1536\"><i></i><img   width=\"133\" height=\"200\" alt=\"Autumn by debyauliaa\" src=\"https://th06.deviantart.net/fs71/200H/i/2014/280/5/3/autumn_by_debyauliaa-d820gpl.jpg\" data-src=\"https://th06.deviantart.net/fs71/200H/i/2014/280/5/3/autumn_by_debyauliaa-d820gpl.jpg\"></a></span><!-- ^TTT --></span><span class=\"details\" ></span><!-- TTT$ --></span> ",
        "ts": "2014-10-07T21:26:42-0700",
        "url": "https://justgalym.deviantart.com/status/13434",
        "comments_count": 0,
        "is_share": false,
        "is_deleted": false,
        "author": {
            "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "regular"
        },
        "items": []
    },
    "stats": {
        "user_deviations": 5,
        "user_favourites": 2,
        "user_comments": 2,
        "profile_pageviews": 3096,
        "profile_comments": 3
    },
    "collections": [
        {
            "folderid": "47B028B2-8D67-C927-3F4F-650E4FABFD6C",
            "name": "Featured"
        },
        {
            "folderid": "7F894824-5609-2C6B-2838-F5E704A27222",
            "name": "Nature"
        },
        {
            "folderid": "38B62271-19EF-403B-0DC4-AE138695B9B6",
            "name": "My Awesome collection"
        }
    ],
    "galleries": [
        {
            "folderid": "C8B0B604-0D86-2076-6207-7D254E175DEA",
            "parent": null,
            "name": "Featured"
        }
    ]
}, {
    "user": {
        "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
        "username": "justgalym",
        "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
        "type": "regular"
    },
    "is_watching": false,
    "profile_url": "https://justgalym.deviantart.com",
    "user_is_artist": false,
    "artist_level": "Professional",
    "artist_specialty": null,
    "real_name": "Galymzhan",
    "tagline": "my tagline",
    "countryid": 110,
    "country": "Kazakhstan",
    "website": "http://bytespree.com",
    "bio": "my bio",
    "cover_photo": null,
    "profile_pic": null,
    "last_status": null,
    "stats": {
        "user_deviations": 5,
        "user_favourites": 2,
        "user_comments": 2,
        "profile_pageviews": 3096,
        "profile_comments": 3
    }
}]""", SampleIsList=true>

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
        ],
        "is_mature": false,
        "is_downloadable": true,
        "download_filesize": 0
    }, {
        "deviationid": "00000000-0000-0000-0000-000000000000",
        "printid": "00000000-0000-0000-0000-000000000000",
        "is_deleted": true
    }]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

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
        return DeviantArtWhoamiRepsonse.Parse json
    }

    member __.AsyncUserProfile ext_collections ext_galleries username = async {
        let query = seq {
            match ext_collections with
            | Some s -> yield sprintf "ext_collections=%b" s
            | None -> ()
            match ext_galleries with
            | Some s -> yield sprintf "ext_galleries=%b" s
            | None -> ()
        }
        let qs = String.concat "&" query
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s?%s" (WebUtility.UrlEncode username) qs
            |> createRequest
        let! json = asyncRead req
        return DeviantArtUserProfileResponse.Parse json
    }

    member __.AsyncGalleryAll offset limit username = async {
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

    member __.AsyncDeviation (deviationid: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" deviationid
            |> createRequest
        let! json = asyncRead req
        return json
        |> sprintf """{ "has_more": false, "next_offset": null, "results": [%s] }"""
        |> DeviantArtGalleryResponse.Parse
        |> (fun r -> r.Results)
        |> Seq.head
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

    member this.GetUsernameAsync() = this.AsyncUserWhoami() |> whenDone (fun u -> u.Username) |> Async.StartAsTask
    member this.GetGalleryThumbnailsAsync() = this.AsyncGalleryAll None None None |> whenDone (fun g -> g.Results |> Seq.map (fun d -> d.Thumbs |> Seq.map (fun t -> t.Src) |> Seq.head)) |> Async.StartAsTask