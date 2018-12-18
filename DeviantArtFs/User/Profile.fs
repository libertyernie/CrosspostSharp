namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data

type ProfileResponse = JsonProvider<"""[{
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

type ProfileRequest(username: string) =
    member __.Username = username
    member val ExtCollections = false with get, set
    member val ExtGalleries = false with get, set

module Profile =
    let AsyncExecute token (req: ProfileRequest) = async {
        let query = seq {
            yield sprintf "ext_collections=%b" req.ExtCollections
            yield sprintf "ext_galleries=%b" req.ExtGalleries
        }
        let qs = String.concat "&" query
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/profile/%s?%s" (dafs.urlEncode req.Username) qs
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return ProfileResponse.Parse json
    }