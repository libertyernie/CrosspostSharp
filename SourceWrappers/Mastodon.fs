namespace SourceWrappers

open ArtworkSourceSpecification
open FSharp.Control
open MapleFedNet.Model
open MapleFedNet.Common

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
        member __.ThumbnailURL = attachment.PreviewUrl

type MastodonSourceWrapper(credentials: IMastodonCredentials, photosOnly: bool) =
    inherit AsyncSeqWrapper()

    let user_task = lazy(MapleFedNet.Api.Accounts.VerifyCredentials(credentials))

    let getUser = user_task.Force() |> Async.AwaitTask

    let isPhoto (a: Attachment) = a.Type = "image"
    let isAnimatedGif (a: Attachment) = a.Type = "gifv"

    override __.Name =
        if photosOnly then
            sprintf "%s (images)" credentials.Domain
        else
            sprintf "%s (text + images)" credentials.Domain

    override __.FetchSubmissionsInternal() = asyncSeq {
        let! user = getUser

        let mutable maxId = ""
        let mutable more = true

        while more do
            let! toots = MapleFedNet.Api.Accounts.Statuses(credentials, user.Id, maxId) |> Async.AwaitTask

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
                
            if System.String.IsNullOrEmpty toots.MaxId then
                more <- false
            else
                maxId <- toots.MaxId
                more <- true
    }

    override __.FetchUserInternal() = async {
        let! u = getUser
        return {
            username = u.UserName
            icon_url = u.AvatarUrl |> Option.ofObj
        }
    }