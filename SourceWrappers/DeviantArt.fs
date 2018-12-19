namespace SourceWrappers

open System
open FSharp.Control
open DeviantArtFs

type DeviantArtPostWrapper(deviation: DeviantArtFs.Gallery.GalleryResponse.Result, metadata: DeviantArtFs.Deviation.MetadataResponse.Metadata option) =
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
            |> Option.defaultValue 0
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

type DeviantArtDeferredPostWrapper(deviation: DeviantArtFs.Gallery.GalleryResponse.Result, client: DeviantArtClient) =
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
        |> Option.defaultValue 0
        |> Swu.fromUnixTime
        |> Some
    override __.AsyncGetActual() = async {
        let! resp =
            Seq.singleton deviation.Deviationid
            |> DeviantArtFs.Deviation.MetadataRequest
            |> DeviantArtFs.Deviation.Metadata.AsyncDeviationMetadata client
        return new DeviantArtPostWrapper(deviation, Seq.tryHead resp.Metadata) :> IRemotePhotoPost
    }

type DeviantArtSourceWrapper(client: DeviantArtClient, loadAll: bool, includeLiterature: bool) =
    inherit AsyncSeqWrapper()

    let asyncGetMetadata (list: seq<DeviantArtFs.Gallery.GalleryResponse.Result>) = async {
        if Seq.isEmpty list then
            return Seq.empty
        else
            let! response =
                list
                |> Seq.map (fun d -> d.Deviationid)
                |> DeviantArtFs.Deviation.MetadataRequest
                |> DeviantArtFs.Deviation.Metadata.AsyncDeviationMetadata client
            return seq {
                yield! response.Metadata
            }
    }
    
    override __.Name = "DeviantArt"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = 0
        let mutable more = true

        while more do
            let! gallery =
                new DeviantArtFs.Gallery.AllRequest(Offset = cursor, Limit = 24)
                |> DeviantArtFs.Gallery.All.AsyncExecute client
            if loadAll then
                let! metadata = asyncGetMetadata gallery.Results
                for d in gallery.Results do
                    if includeLiterature || not (Option.isNone d.Content) then
                        let m =
                            metadata
                            |> Seq.filter (fun m -> m.Deviationid = d.Deviationid)
                            |> Seq.tryHead
                        yield DeviantArtPostWrapper(d, m) :> IPostBase
            else
                for d in gallery.Results do
                    if includeLiterature || not (Option.isNone d.Content) then
                        yield DeviantArtDeferredPostWrapper(d, client) :> IPostBase
            
            cursor <- gallery.NextOffset
                |> Option.defaultValue 0
            more <- gallery.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }