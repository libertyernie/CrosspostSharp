namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

type IdResponse = JsonProvider<"""[{
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
}]""", SampleIsList=true>

module Id =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return IdResponse.Parse json
    }