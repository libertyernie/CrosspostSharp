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

type DeviantArtStatusesResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 3,
    "results": [
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
                "type": "admin"
            },
            "items": []
        }
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": [
        {
            "statusid": "23B744A7-3842-043A-1AA4-71234113A7E7",
            "body": "I was gonna go to bed, but then Backstreet Boys stared playing on the radio",
            "ts": "2018-11-11T18:47:39-0800",
            "url": "https://www.deviantart.com/lizard-socks/status/15474448",
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
        }
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": [
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
    ]
}, {
    "has_more": false,
    "next_offset": null,
    "results": []
}
]""", SampleIsList=true>

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

    member __.AsyncUserStatuses offset limit username = async {
        let query = seq {
            yield sprintf "username=%s" (WebUtility.UrlEncode username)
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
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/user/statuses?%s"
            |> createRequest
        let! json = asyncRead req
        return DeviantArtStatusesResponse.Parse json
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

    member this.GetGalleryThumbnailsAsync() = this.AsyncGalleryAll None None None |> whenDone (fun g -> g.Results |> Seq.map (fun d -> d.Thumbs |> Seq.map (fun t -> t.Src) |> Seq.head)) |> Async.StartAsTask
    member this.UserStatusesPostAsync(ps) = this.AsyncUserStatusesPost ps |> Async.StartAsTask
    member this.GalleryFoldersAsync(ps) = this.AsyncGalleryFolders ps |> whenDone (fun r -> r.Results |> Seq.map (fun f -> (f.Folderid, f.Name)) |> dict) |> Async.StartAsTask
    member this.StashSubmitAsync(ps) = this.AsyncStashSubmit ps |> Async.StartAsTask

    interface IDeviantArtAccessToken with
        member this.AccessToken = current_token.AccessToken