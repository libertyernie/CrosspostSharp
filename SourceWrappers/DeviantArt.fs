namespace SourceWrappers

open FSharp.Control
open DeviantArtFs
open System

type DeviantArtPostWrapper(deviation: Deviation, metadata: DeviationMetadata option) =
    let src =
        deviation.content
        |> Option.map (fun c -> c.src)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    interface IRemotePhotoPost with
        member this.Title =
            deviation.title
            |> Option.defaultValue (deviation.deviationid.ToString())
        member this.HTMLDescription =
            match metadata with
                | Some m -> m.description.Replace("https://www.deviantart.com/users/outgoing?", "")
                | None -> null
        member this.Mature =
            deviation.is_mature
            |> Option.defaultValue false
        member this.Adult = false
        member this.Tags = 
            match metadata with
                | Some m -> m.tags |> Seq.map (fun t -> t.tag_name)
                | None -> Seq.empty
        member this.Timestamp =
            deviation.published_time
            |> Option.map (fun d -> d.UtcDateTime)
            |> Option.defaultValue DateTime.UtcNow
        member this.ViewURL =
            deviation.url
            |> Option.defaultValue "https://www.example.com"
        member this.ImageURL = src
        member this.ThumbnailURL =
            deviation.thumbs
            |> Option.defaultValue List.empty
            |> Seq.map (fun d -> d.src)
            |> Seq.tryHead
            |> Option.defaultValue src

type DeviantArtDeferredPostWrapper(deviation: Deviation, client: IDeviantArtAccessToken) =
    inherit DeferredPhotoPost()

    let src =
        deviation.content
        |> Option.map (fun c -> c.src)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    override __.Title =
        deviation.title
        |> Option.defaultValue (deviation.deviationid.ToString())
    override __.ThumbnailURL =
        deviation.thumbs
        |> Option.defaultValue List.empty
        |> Seq.map (fun d -> d.src)
        |> Seq.tryHead
        |> Option.defaultValue src
    override __.ViewURL =
        deviation.url
        |> Option.defaultValue "https://www.example.com"
    override __.Timestamp = 
        deviation.published_time
        |> Option.map (fun d -> d.UtcDateTime)
    override __.AsyncGetActual() = async {
        let! resp =
            Seq.singleton deviation.deviationid
            |> DeviantArtFs.Requests.Deviation.MetadataRequest
            |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute client
        return new DeviantArtPostWrapper(deviation, Seq.tryHead resp) :> IRemotePhotoPost
    }

type DeviantArtSourceWrapper(client: IDeviantArtAccessToken, includeLiterature: bool) =
    inherit AsyncSeqWrapper()

    override __.Name = "DeviantArt"

    override __.FetchSubmissionsInternal() =
        new DeviantArtFs.Requests.Gallery.GalleryAllViewRequest()
        |> DeviantArtFs.Requests.Gallery.GalleryAllView.ToAsyncSeq client 0
        |> AsyncSeq.filter (fun d -> includeLiterature || Option.isSome d.content)
        |> AsyncSeq.map (fun d -> new DeviantArtDeferredPostWrapper(d, client) :> IPostBase)

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.username
            icon_url = Some u.usericon
        }
    }