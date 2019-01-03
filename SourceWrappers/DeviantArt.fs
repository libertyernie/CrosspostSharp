namespace SourceWrappers

open FSharp.Control
open DeviantArtFs

type DeviantArtPostWrapper(deviation_obj: Deviation, metadata_obj: Metadata option) =
    let deviation = deviation_obj.Original
    let metadata = metadata_obj |> Option.map (fun m -> m.Original)

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
            |> Option.defaultValue 0L
            |> Swu.fromUnixTime
        member this.ViewURL =
            deviation.Url
            |> Option.defaultValue "https://www.example.com"
        member this.ImageURL = src
        member this.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue src

type DeviantArtDeferredPostWrapper(deviation_obj: Deviation, client: IDeviantArtAccessToken) =
    inherit DeferredPhotoPost()
    
    let deviation = deviation_obj.Original

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
        |> Option.defaultValue 0L
        |> Swu.fromUnixTime
        |> Some
    override __.AsyncGetActual() = async {
        let! resp =
            Seq.singleton deviation.Deviationid
            |> DeviantArtFs.Requests.Deviation.MetadataRequest
            |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute client
        return new DeviantArtPostWrapper(deviation_obj, Seq.tryHead resp) :> IRemotePhotoPost
    }

type DeviantArtSourceWrapper(client: IDeviantArtAccessToken, loadAll: bool, includeLiterature: bool) =
    inherit AsyncSeqWrapper()

    let asyncGetMetadata (list: seq<Deviation>) = async {
        if Seq.isEmpty list then
            return Seq.empty
        else
            return!
                list
                |> Seq.map (fun d -> d.Deviationid)
                |> DeviantArtFs.Requests.Deviation.MetadataRequest
                |> DeviantArtFs.Requests.Deviation.MetadataById.AsyncExecute client
    }
    
    override __.Name = "DeviantArt"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = 0
        let mutable more = true

        while more do
            let! gallery =
                new DeviantArtFs.Requests.Gallery.AllRequest(Offset = cursor, Limit = 24)
                |> DeviantArtFs.Requests.Gallery.All.AsyncExecute client

            if loadAll then
                let! metadata = asyncGetMetadata gallery.Results
                for d in gallery.Results do
                    if includeLiterature || not (isNull d.Content) then
                        let m =
                            metadata
                            |> Seq.filter (fun m -> m.Deviationid = d.Deviationid)
                            |> Seq.tryHead
                        yield DeviantArtPostWrapper(d, m) :> IPostBase
            else
                for d in gallery.Results do
                    if includeLiterature || not (isNull d.Content) then
                        yield DeviantArtDeferredPostWrapper(d, client) :> IPostBase
            
            cursor <- gallery.NextOffset
                |> Option.defaultValue 0
            more <- gallery.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }