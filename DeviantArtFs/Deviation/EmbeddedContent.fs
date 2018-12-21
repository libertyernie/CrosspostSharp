namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

type EmbeddedContentResponse = JsonProvider<"""[
{
    "has_more": true,
    "next_offset": 1,
    "has_less": false,
    "prev_offset": null,
    "results": [
        {
            "deviationid": "7573A2CC-629A-B586-98BC-2ADAB5E316F3",
            "printid": null,
            "url": "https://www.deviantart.com/lizard-socks/art/Jeremy-747930253",
            "title": "Jeremy",
            "category": "Drawings",
            "category_path": "cartoons/digital/cartoons/drawings",
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
                "favourites": 1
            },
            "published_time": "1527990245",
            "allows_comments": true,
            "preview": {
                "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcdaqod-0b5fcc4c-1a50-4f49-8a6b-7152ed6dd192.png",
                "height": 618,
                "width": 316,
                "transparency": true
            },
            "content": {
                "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcdaqod-0b5fcc4c-1a50-4f49-8a6b-7152ed6dd192.png",
                "height": 618,
                "width": 316,
                "transparency": true,
                "filesize": 7732
            },
            "thumbs": [
                {
                    "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcdaqod-0b5fcc4c-1a50-4f49-8a6b-7152ed6dd192.png/v1/fit/w_150,h_150,strp/jeremy_by_lizard_socks_dcdaqod-150.png",
                    "height": 150,
                    "width": 77,
                    "transparency": true
                },
                {
                    "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcdaqod-0b5fcc4c-1a50-4f49-8a6b-7152ed6dd192.png/v1/crop/w_102,h_200,x_0,y_0,scl_0.32594936708861,strp/jeremy_by_lizard_socks_dcdaqod-200h.png",
                    "height": 200,
                    "width": 102,
                    "transparency": true
                },
                {
                    "src": "https://images-wixmp-ed30a86b8c4ca887773594c2.wixmp.com/intermediary/f/a26676ed-5753-4a81-8907-199bd9057960/dcdaqod-0b5fcc4c-1a50-4f49-8a6b-7152ed6dd192.png/v1/fit/w_300,h_618,strp/jeremy_by_lizard_socks_dcdaqod-300w.png",
                    "height": 587,
                    "width": 300,
                    "transparency": true
                }
            ],
            "is_mature": false,
            "is_downloadable": true,
            "download_filesize": 7732
        }
    ]
},
{
    "has_more": false,
    "next_offset": null,
    "has_less": true,
    "prev_offset": 1,
    "results": []
}
]""", SampleIsList=true>

type EmbeddedContentRequest(deviationid: Guid) =
    member __.DeviationId = deviationid
    member val OffsetDeviationId = Nullable<Guid>() with get, set
    member val Offset = 0 with get, set
    member val Limit = 10 with get, set

module EmbeddedContent =
    let AsyncExecute token (req: EmbeddedContentRequest) = async {
        let query = seq {
            yield sprintf "deviationid=%O" req.DeviationId
            match Option.ofNullable req.OffsetDeviationId with
            | Some s -> yield sprintf "offset_deviationid=%O" s
            | None -> ()
            yield sprintf "offset=%d" req.Offset
            yield sprintf "limit=%d" req.Limit
        }
        let req =
            query
            |> String.concat "&"
            |> sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/embeddedcontent?%s"
            |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return EmbeddedContentResponse.Parse json
    }