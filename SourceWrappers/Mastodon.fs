namespace SourceWrappers

open Mastodon.Model
open FSharp.Control

type MastodonPostWrapper(status: Status) =
    member __.SpoilerText = status.SpoilerText
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

type MastodonVideoPostWrapper(status: Status, attachment: Attachment) =
    inherit MastodonPostWrapper(status)
    interface IRemoteVideoPost with
        member __.VideoURL = attachment.Url

type MastodonSourceWrapper(domain: string, token: string, photosOnly: bool) =
    inherit AsyncSeqWrapper()

    let user_task = lazy(Mastodon.Api.Accounts.VerifyCredentials(domain, token))

    let getUser = user_task.Force() |> Async.AwaitTask

    let isPhoto (a: Attachment) = a.Type = "image"
    let isAnimatedGif (a: Attachment) = a.Type = "gifv"

    override __.Name =
        if photosOnly then
            sprintf "%s (images)" domain
        else
            sprintf "%s (text + images)" domain

    override __.FetchSubmissionsInternal() = asyncSeq {
        let! user = getUser

        let mutable maxId = 0L
        let mutable more = true

        while more do
            let! toots = Mastodon.Api.Accounts.Statuses(domain, token, user.Id, maxId) |> Async.AwaitTask

            for t in toots do
                if isNull t.Reblog then
                    let photos = t.MediaAttachments |> Seq.filter isPhoto
                    let gifs =
                        if photosOnly
                        then Seq.empty
                        else t.MediaAttachments |> Seq.filter isAnimatedGif
                    if Seq.isEmpty photos && Seq.isEmpty gifs then
                        if not photosOnly then
                            yield new MastodonPostWrapper(t) :> IPostBase
                    else
                        for a in photos do
                            yield new MastodonPhotoPostWrapper(t, a) :> IPostBase
                        for a in gifs do
                            yield new MastodonVideoPostWrapper(t, a) :> IPostBase
                
            more <- not (Seq.isEmpty toots)
            if more then
                maxId <- (toots |> Seq.map (fun t -> t.Id) |> Seq.min) - 1L
    }

    override __.FetchUserInternal() = async {
        let! u = getUser
        return {
            username = u.UserName
            icon_url = u.AvatarUrl |> Option.ofObj
        }
    }