namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

type MetadataResponse = JsonProvider<"""{
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

type MetadataRequest(deviationids: seq<Guid>) =
    member __.DeviationIds = deviationids
    member val ExtSubmission = false with get, set
    member val ExtCamera = false with get, set
    member val ExtStats = false with get, set
    member val ExtCollection = false with get, set

module Metadata =
    let AsyncExecute token (req: MetadataRequest) = async {
        let query = seq {
            yield sprintf "ext_submission=%b" req.ExtSubmission
            yield sprintf "ext_camera=%b" req.ExtCamera
            yield sprintf "ext_stats=%b" req.ExtStats
            yield sprintf "ext_collection=%b" req.ExtCollection
            yield req.DeviationIds |> Seq.map (fun o -> o.ToString()) |> String.concat "," |> sprintf "deviationids[]=%s"
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/metadata?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return MetadataResponse.Parse json
    }