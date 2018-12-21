namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data
open System

type StatusesIdResponse = JsonProvider<"""[
    {
        "statusid": "9F680E12-C156-55F2-F026-53B8BC00E291",
        "body": "sharing journals",
        "ts": "2014-10-15T22:03:46-0700",
        "url": "https://justgalym.deviantart.com/status/25704",
        "comments_count": 0,
        "is_share": true,
        "is_deleted": false,
        "author": {
            "userid": "09A4052C-88B2-65CB-CEC5-B1E31C18B940",
            "username": "justgalym",
            "usericon": "https://a.deviantart.net/avatars/j/u/justgalym.jpg?1",
            "type": "admin"
        },
        "items": [
            {
                "type": "deviation",
                "deviation": {
                    "deviationid": "F9921FC4-3A0F-90A9-3625-AA8E105747AD",
                    "printid": null,
                    "url": "https://justgalym.deviantart.com/journal/Another-post-written-in-stash-446384730",
                    "title": "Another post written in stash",
                    "category": "Personal Journal",
                    "category_path": "personaljournal",
                    "is_downloadable": false,
                    "is_mature": false,
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
                }
            }
        ]
    },
    {
        "statusid": "163DB8A7-49CF-657D-FE3A-A877222A6161",
        "body": "<p>Sad monsters?? This is my kind of game </p><br /><br /><a class=\"embedded-deviation embedded-image-deviation\" href=\"https://sta.sh/01o8rj7yawnx\" data-embed-type=\"deviation\" data-embed-id=\"6118323951538653\" data-extension=\"jpg\" data-fullview=\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg\" data-fullview-width=\"1024\" data-fullview-height=\"576\"><img src=\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg\" alt=\"Status Update 10/21/2018 11:11:10 AM by lizard-socks\" title=\"Status Update 10/21/2018 11:11:10 AM by lizard-socks\" style=\"\"></a>",
        "ts": "2018-10-21T09:11:15-0700",
        "url": "https://www.deviantart.com/lizard-socks/status/15268122",
        "comments_count": 0,
        "is_share": false,
        "is_deleted": false,
        "author": {
            "userid": "7EE6490E-FCA7-3129-410A-AA684C3BC7DB",
            "username": "lizard-socks",
            "usericon": "https://a.deviantart.net/avatars/l/i/lizard-socks.png",
            "type": "regular"
        },
        "items": [
            {
                "type": "thumb_background_deviation",
                "deviation": {
                    "deviationid": "10DB0566-619F-FEF7-AF81-FC84BBF39D7B",
                    "printid": null,
                    "url": "https://www.deviantart.com/lizard-socks/art/Status-Update-10-21-2018-11-11-10-AM-769251820",
                    "title": "Status Update 10/21/2018 11:11:10 AM",
                    "category": "Uncategorized",
                    "category_path": "",
                    "is_favourited": false,
                    "is_deleted": false,
                    "author": {
                        "userid": "7EE6490E-FCA7-3129-410A-AA684C3BC7DB",
                        "username": "lizard-socks",
                        "usericon": "https://a.deviantart.net/avatars/l/i/lizard-socks.png",
                        "type": "regular"
                    },
                    "stats": {
                        "comments": 0,
                        "favourites": 0
                    },
                    "published_time": "1540138272",
                    "allows_comments": true,
                    "preview": {
                        "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg",
                        "height": 576,
                        "width": 1024,
                        "transparency": false
                    },
                    "content": {
                        "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg",
                        "height": 576,
                        "width": 1024,
                        "transparency": false,
                        "filesize": 171897
                    },
                    "thumbs": [
                        {
                            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fit/w_150,h_150,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-150.jpg",
                            "height": 84,
                            "width": 150,
                            "transparency": false
                        },
                        {
                            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_300,h_169,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-200h.jpg",
                            "height": 169,
                            "width": 300,
                            "transparency": false
                        },
                        {
                            "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fit/w_300,h_720,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-300w.jpg",
                            "height": 169,
                            "width": 300,
                            "transparency": false
                        }
                    ],
                    "is_mature": false,
                    "is_downloadable": true,
                    "download_filesize": 171897
                }
            }
        ]
    },
    {
        "statusid": "1358FE0E-8A9D-E39F-3BC8-A7ACEC0EEE8C",
        "body": "test",
        "ts": "2018-12-16T15:33:10-0800",
        "url": "https://www.deviantart.com/libertyernie/status/15844513",
        "comments_count": 0,
        "is_share": true,
        "is_deleted": false,
        "author": {
            "userid": "FE89BDF5-90DF-951A-24CD-366353ECC271",
            "username": "libertyernie",
            "usericon": "https://a.deviantart.net/avatars/l/i/libertyernie.jpg?2",
            "type": "regular"
        },
        "items": [
            {
                "type": "status",
                "status": {
                    "statusid": "163DB8A7-49CF-657D-FE3A-A877222A6161",
                    "body": "<p>Sad monsters?? This is my kind of game </p><br /><br /><a class=\"embedded-deviation embedded-image-deviation\" href=\"https://sta.sh/01o8rj7yawnx\" data-embed-type=\"deviation\" data-embed-id=\"6118323951538653\" data-extension=\"jpg\" data-fullview=\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg\" data-fullview-width=\"1024\" data-fullview-height=\"576\"><img src=\"https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcpzqi4-2127463d-51c3-4a3e-b5ed-98c69457015d.jpg/v1/fill/w_1024,h_576,q_70,strp/status_update_10_21_2018_11_11_10_am_by_lizard_socks_dcpzqi4-fullview.jpg\" alt=\"Status Update 10/21/2018 11:11:10 AM by lizard-socks\" title=\"Status Update 10/21/2018 11:11:10 AM by lizard-socks\" style=\"\"></a>",
                    "ts": "2018-10-21T09:11:15-0700",
                    "url": "https://www.deviantart.com/lizard-socks/status/15268122",
                    "comments_count": 0,
                    "is_share": false,
                    "is_deleted": false,
                    "author": {
                        "userid": "7EE6490E-FCA7-3129-410A-AA684C3BC7DB",
                        "username": "lizard-socks",
                        "usericon": "https://a.deviantart.net/avatars/l/i/lizard-socks.png",
                        "type": "regular"
                    },
                    "items": []
                }
            }
        ]
    }
]""", SampleIsList=true>

module StatusesId =
    let AsyncExecute token (id: Guid) = async {
        let req =
            sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%O" id
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return StatusesIdResponse.Parse json
    }