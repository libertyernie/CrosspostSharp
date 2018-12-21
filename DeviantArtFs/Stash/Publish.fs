namespace DeviantArtFs.Stash

open DeviantArtFs
open FSharp.Data
open System

type internal PublishResponse = JsonProvider<"""{
    "status": "success",
    "url": "https://username.deviantart.com/art/My-Great-Artwork-1234567890",
    "deviationid": "DAF48E51-DE0E-B24D-4CEC-A23170DCE888"
}""">

type PublishResult = {
    Url: string
    DeviationId: Guid
}

type LicenseModifyOption = No=0 | Yes=1 | ShareAlike=2

type MatureLevel = None=0 | Strict=1 | Moderate=2

[<FlagsAttribute>]
type MatureClassification = None=0 | Nudity=1 | Sexual=2 | Gore=4 | Language=8 | Ideology=16

type DisplayResolution = Original=0 | Max400Px=1 | Max600px=2 | Max800px=3 | Max900px=4 | Max1024px=5 | Max1280px=6 | Max1600px=7

type Sharing = Allow=0 | HideShareButtons=1 | HideAndMembersOnly=2

type LicenseOptions() =
    member val CreativeCommons = false with get, set
    member val Commercial = false with get, set
    member val Modify = LicenseModifyOption.No with get, set

type PublishRequest(itemId: int64) =
    member val IsMature = false with get, set
    member val MatureLevel = MatureLevel.None with get, set
    member val MatureClassification = MatureClassification.None with get, set
    member val AgreeSubmission = false with get, set
    member val AgreeTos = false with get, set
    member val Catpath = null with get, set
    member val Feature = false with get, set
    member val AllowComments = false with get, set
    member val RequestCritique = false with get, set
    member val DisplayResolution = DisplayResolution.Original with get, set
    member val Sharing = Sharing.Allow with get, set
    member val LicenseOptions = new LicenseOptions() with get, set
    member val GalleryIds = Seq.empty<Guid> with get, set
    member val AllowFreeDownload = false with get, set
    member val AddWatermark = false with get, set
    member val ItemId = itemId

module Publish =
    open System.IO

    let AsyncExecute token (req: PublishRequest) = async {
        let query = seq {
            yield sprintf "is_mature=%b" req.IsMature
            match req.MatureLevel with
            | MatureLevel.Strict -> yield "mature_level=strict"
            | MatureLevel.Moderate -> yield "mature_level=moderate"
            | _ -> ()
            if req.MatureClassification.HasFlag MatureClassification.Nudity then
                yield "mature_classification[]=nudity"
            if req.MatureClassification.HasFlag MatureClassification.Sexual then
                yield "mature_classification[]=sexual"
            if req.MatureClassification.HasFlag MatureClassification.Gore then
                yield "mature_classification[]=gore"
            if req.MatureClassification.HasFlag MatureClassification.Language then
                yield "mature_classification[]=language"
            if req.MatureClassification.HasFlag MatureClassification.Ideology then
                yield "mature_classification[]=ideology"
            yield sprintf "agree_submission=%b" req.AgreeSubmission
            yield sprintf "agree_tos=%b" req.AgreeTos
            yield sprintf "catpath=%s" (dafs.urlEncode req.Catpath)
            yield sprintf "feature=%b" req.Feature
            yield sprintf "allow_comments=%b" req.AllowComments
            yield sprintf "request_critique=%b" req.RequestCritique
            yield sprintf "display_resolution=%d" (int req.DisplayResolution)
            match req.Sharing with
            | Sharing.Allow -> yield sprintf "sharing=allow"
            | Sharing.HideShareButtons -> yield sprintf "sharing=hide_share_buttons"
            | Sharing.HideAndMembersOnly -> yield sprintf "sharing=hide_and_members_only"
            | _ -> ()
            yield sprintf "license_options[creative_commons]=%b" req.LicenseOptions.CreativeCommons
            yield sprintf "license_options[commercial]=%s" (if req.LicenseOptions.Commercial then "yes" else "no")
            match req.LicenseOptions.Modify with
            | LicenseModifyOption.Yes -> yield "license_options[modify]=yes"
            | LicenseModifyOption.No -> yield "license_options[modify]=no"
            | LicenseModifyOption.ShareAlike -> yield "license_options[modify]=share"
            | _ -> ()
            for id in req.GalleryIds do
                yield sprintf "galleryids[]=%O" id
            yield sprintf "allow_free_download=%b" req.AllowFreeDownload
            yield sprintf "add_watermark=%b" req.AddWatermark
            yield sprintf "itemid=%d" req.ItemId
        }

        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/stash/publish"
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"

        do! async {
            use! stream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(stream)
            do! String.concat "&" query |> sw.WriteAsync |> Async.AwaitTask
        }

        let! json = dafs.asyncRead req
        let resp = PublishResponse.Parse json
        return {
            Url = resp.Url
            DeviationId = resp.Deviationid
        }
    }

    let ExecuteAsync token req = AsyncExecute token req |> Async.StartAsTask