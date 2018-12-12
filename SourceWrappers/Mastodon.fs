namespace SourceWrappers

open Mastonet
open Mastonet.Entities
open FSharp.Control
open System

type MastodonPostWrapper(status: Status) =
    interface IPostBase with
        member __.Title = ""
        member __.HTMLDescription = status.Content
        member __.Mature = status.Sensitive.GetValueOrDefault()
        member __.Adult = status.Sensitive.GetValueOrDefault()
        member __.Tags = status.Tags |> Seq.map (fun t -> t.Name)
        member __.Timestamp = status.CreatedAt
        member __.ViewURL = status.Url

type MastodonPhotoPostWrapper(status: Status, attachment: Attachment) =
    inherit MastodonPostWrapper(status)
    interface IRemotePhotoPost with
        member __.ImageURL = attachment.Url
        member __.ThumbnailURL = attachment.PreviewUrl

type MastodonSourceWrapper(client: IMastodonClient) =
    inherit AsyncSeqWrapper()

    let user_task = lazy(client.GetCurrentUser())

    let getUser = user_task.Force() |> Async.AwaitTask

    let isPhoto (a: Attachment) = a.Type = "image"

    override __.Name = sprintf "%s (Mastodon)" client.Instance

    override __.FetchSubmissionsInternal() = asyncSeq {
        let! user = getUser

        let mutable maxId = Nullable<int64>()
        let mutable more = true

        while more do
            let! toots = client.GetAccountStatuses(user.Id, maxId) |> Async.AwaitTask

            for t in toots do
                if isNull t.Reblog then
                    let photos = t.MediaAttachments |> Seq.filter isPhoto
                    if Seq.isEmpty photos then
                        yield new MastodonPostWrapper(t) :> IPostBase
                    else
                        for a in photos do
                            yield new MastodonPhotoPostWrapper(t, a) :> IPostBase
                
            more <- not (Seq.isEmpty toots)
            if more then
                maxId <- (toots |> Seq.map (fun t -> t.Id) |> Seq.min) - 1L |> Nullable
    }

    override __.FetchUserInternal() = async {
        let! u = getUser
        return {
            username = u.UserName
            icon_url = u.AvatarUrl |> Option.ofObj
        }
    }