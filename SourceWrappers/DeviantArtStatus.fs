namespace SourceWrappers

open DeviantartApi.Objects
open System
open DeviantartApi.Requests.User
open FSharp.Control

type DeviantArtStatusPostWrapper(status: Status, deviation: Deviation option) =
    let Mature =
        deviation
        |> Option.map (fun d -> Option.ofNullable d.IsMature)
        |> Option.flatten
        |> Option.defaultValue false

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.TimeStamp
        member this.ViewURL = status.Url.AbsoluteUri

type DeviantArtStatusPhotoPostWrapper(status: Status, deviation: Deviation) =
    inherit DeviantArtStatusPostWrapper(status, Some deviation)
    let Icon = status.Author.UserIconUrl.AbsoluteUri

    interface IRemotePhotoPost with
        member __.ImageURL =
            deviation.Content
            |> Option.ofObj
            |> Option.map (fun d -> d.Src)
            |> Option.defaultValue Icon
        member __.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun t -> t.Src)
            |> Seq.tryHead
            |> Option.defaultValue Icon

type DeviantArtStatusSourceWrapper() =
    inherit AsyncSeqWrapper()

    override this.Name = "DeviantArt (statuses)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable position = 0
        let mutable more = true

        while more do
            let! username = this.AsyncWhoami()

            let statusesRequest = new StatusesRequest(username)
            statusesRequest.Limit <- 50 |> uint32 |> Nullable
            statusesRequest.Offset <- position |> uint32 |> Nullable

            let! statuses =
                statusesRequest.ExecuteAsync()
                |> Async.AwaitTask
                |> Swu.whenDone Swu.processDeviantArtError

            for r in statuses.Results do
                let items = r.Items |> Seq.map (fun i -> i.Deviation) |> Seq.filter (not << isNull)
                if Seq.isEmpty items then
                    yield new DeviantArtStatusPostWrapper(r, None) :> IPostBase
                else
                    for d in items do
                        yield new DeviantArtStatusPhotoPostWrapper(r, d) :> IPostBase

            position <-
                statuses.NextOffset
                |> Option.ofNullable
                |> Option.defaultValue position
            more <- statuses.HasMore
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