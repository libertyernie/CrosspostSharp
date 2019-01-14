namespace SourceWrappers

open FSharp.Control
open DeviantArtFs
open System

type DeviantArtPostWrapper(deviation: Deviation, metadata: DeviationMetadata option) =
    let src =
        deviation.Content
        |> Option.map (fun c -> c.Src)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    interface IRemotePhotoPost with
        member this.Title =
            deviation.Title
            |> Option.defaultValue (deviation.Deviationid.ToString())
        member this.HTMLDescription =
            match metadata with
                | Some m -> m.Description
                | None -> null
        member this.Mature =
            deviation.IsMature
            |> Option.defaultValue false
        member this.Adult = false
        member this.Tags = 
            match metadata with
                | Some m -> m.Tags |> Seq.map (fun t -> t.TagName)
                | None -> Seq.empty
        member this.Timestamp =
            deviation.PublishedTime
            |> Option.map (fun d -> d.UtcDateTime)
            |> Option.defaultValue DateTime.UtcNow
        member this.ViewURL =
            deviation.Url
            |> Option.defaultValue "https://www.example.com"
        member this.ImageURL = src
        member this.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue src

type DeviantArtDeferredPostWrapper(deviation: Deviation, client: IDeviantArtAccessToken) =
    inherit DeferredPhotoPost()

    let src =
        deviation.Content
        |> Option.map (fun c -> c.Src)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    override __.Title =
        deviation.Title
        |> Option.defaultValue (deviation.Deviationid.ToString())
    override __.ThumbnailURL =
        deviation.Thumbs
        |> Seq.map (fun d -> d.Src)
        |> Seq.tryHead
        |> Option.defaultValue src
    override __.ViewURL =
        deviation.Url
        |> Option.defaultValue "https://www.example.com"
    override __.Timestamp = 
        deviation.PublishedTime
        |> Option.map (fun d -> d.UtcDateTime)
    override __.AsyncGetActual() = async {
        let! resp =
            Seq.singleton deviation.Deviationid
            |> DeviantArtFs.Requests.Deviation.MetadataRequest
            |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute client
        return new DeviantArtPostWrapper(deviation, Seq.tryHead resp) :> IRemotePhotoPost
    }

type DeviantArtSourceWrapper(client: IDeviantArtAccessToken, includeLiterature: bool) =
    inherit AsyncSeqWrapper()

    override __.Name = "DeviantArt"

    override __.FetchSubmissionsInternal() =
        let req = new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest()
        DeviantArtFs.Requests.Gallery.GalleryAllView.ToAsyncSeq client req 0
        |> AsyncSeq.filter (fun d -> includeLiterature || Option.isSome d.Content)
        |> AsyncSeq.map (fun d -> new DeviantArtDeferredPostWrapper(d, client) :> IPostBase)

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }