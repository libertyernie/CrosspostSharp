namespace SourceWrappers

open DeviantartApi.Objects
open DeviantartApi.Objects.SubObjects.DeviationMetadata
open System
open DeviantartApi.Requests.User
open AsyncHelpers
open DeviantartApi.Requests.Gallery
open DeviantartApi.Requests.Deviation

type DeviantArtPostWrapper(deviation: Deviation, metadata: Metadata option) =
    interface IPostWrapper with
        member this.Title = deviation.Title
        member this.HTMLDescription =
            match metadata with
                | Some m -> 
                    if m.Description.Contains("<P>") || m.Description.Contains("<p>") then
                        m.Description
                    else
                        sprintf "<p>%s</p>" m.Description
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
    inherit SourceWrapper<uint32>()

    let mutable cached_user: User = null

    let getUser = async {
        if isNull cached_user then
            let req = new WhoAmIRequest()
            let! u =
                req.ExecuteAsync()
                |> Async.AwaitTask
                |> whenDone processDeviantArtError
            cached_user <- u
        return cached_user
    }
    
    override this.Name = "DeviantArt (F#)"
    override this.SubmissionsFiltered = true

    override this.Fetch cursor take = async {
        let position = cursor |> Option.defaultValue (uint32 0)

        let galleryRequest = new AllRequest()
        galleryRequest.Limit <- take |> uint32 |> Nullable
        galleryRequest.Offset <- position |> Nullable

        let! gallery =
            galleryRequest.ExecuteAsync()
            |> Async.AwaitTask
            |> whenDone processDeviantArtError
        
        let metadataRequest =
            gallery.Results
            |> Seq.map (fun d -> d.DeviationId)
            |> MetadataRequest
        
        let! metadata =
            metadataRequest.ExecuteAsync()
            |> Async.AwaitTask
            |> whenDone processDeviantArtError
        
        let wrappers = seq {
            for d in gallery.Results do
                let m =
                    metadata.Metadata
                    |> Seq.filter (fun m -> m.DeviationId = d.DeviationId)
                    |> Seq.tryHead
                yield DeviantArtPostWrapper(d, m) :> IPostWrapper
        }

        return {
            Posts = wrappers
            Next = gallery.NextOffset |> Option.ofNullable |> Option.defaultValue 0 |> uint32
        }
    }

    override this.Whoami () = getUser |> whenDone (fun u -> u.Username)

    override this.GetUserIcon size = getUser |> whenDone (fun u -> u.UserIconUrl.AbsoluteUri)