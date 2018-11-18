namespace SourceWrappers

open DeviantartApi.Objects
open DeviantartApi.Objects.SubObjects.DeviationMetadata
open System
open DeviantartApi.Requests.User
open DeviantartApi.Requests.Gallery
open DeviantartApi.Requests.Deviation
open FSharp.Control

type DeviantArtPostWrapper(deviation: Deviation, metadata: Metadata option) =
    interface IRemotePhotoPost with
        member this.Title = deviation.Title
        member this.HTMLDescription =
            match metadata with
                | Some m -> m.Description
                | None -> null
        member this.Mature = deviation.IsMature |> Option.ofNullable |> Option.defaultValue false
        member this.Adult = false
        member this.Tags = 
            match metadata with
                | Some m -> m.Tags |> Seq.map (fun t -> t.TagName)
                | None -> Seq.empty
        member this.Timestamp = deviation.PublishedTime |> Option.ofNullable |> Option.defaultValue DateTime.UtcNow
        member this.ViewURL = deviation.Url.AbsoluteUri
        member this.ImageURL = deviation.Content.Src
        member this.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue deviation.Content.Src

type DeviantArtSourceWrapper() =
    inherit AsyncSeqWrapper()

    let asyncGetMetadata (list: seq<Deviation>) = async {
        if Seq.isEmpty list then
            return Seq.empty
        else
            let! response =
                list
                |> Seq.map (fun d -> d.DeviationId)
                |> MetadataRequest
                |> Swu.executeAsync
            return seq {
                yield! response.Metadata
            }
    }
    
    override __.Name = "DeviantArt"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = uint32 0
        let mutable more = true

        while more do
            let galleryRequest = new AllRequest()
            galleryRequest.Limit <- uint32 24 |> Nullable
            galleryRequest.Offset <- cursor |> Nullable

            let! gallery = Swu.executeAsync galleryRequest
            let! metadata = asyncGetMetadata gallery.Results

            for d in gallery.Results do
                if not (isNull d.Content) then
                    let m =
                        metadata
                        |> Seq.filter (fun m -> m.DeviationId = d.DeviationId)
                        |> Seq.tryHead
                    yield DeviantArtPostWrapper(d, m) :> IPostBase
            
            cursor <- gallery.NextOffset
                |> Option.ofNullable
                |> Option.defaultValue 0
                |> uint32
            more <- gallery.HasMore
    }

    override __.FetchUserInternal() = async {
        let req = new WhoAmIRequest()
        let! u =
            req.ExecuteAsync()
            |> Async.AwaitTask
            |> Swu.whenDone Swu.processDeviantArtError
        return {
            username = u.Username
            icon_url = Some u.UserIconUrl.AbsoluteUri
        }
    }