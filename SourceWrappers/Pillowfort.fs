namespace SourceWrappers

open PillowfortFs
open System
open FSharp.Control

type PillowfortPostWrapper(client: PillowfortClient, post: PillowfortPost, media: PillowfortMedia option) =
    interface IRemotePhotoPost with
        member __.Title = post.title
        member __.HTMLDescription = post.content
        member __.Mature = post.nsfw
        member __.Adult = post.nsfw
        member __.Tags = post.tags
        member __.Timestamp = post.created_at.UtcDateTime
        member __.ViewURL = sprintf "https://pillowfort.social/posts/%d" post.id
        member __.ImageURL =
            match media with
            | Some m -> m.url
            | None -> post.avatar_url
        member __.ThumbnailURL =
            match media with
            | Some m -> m.url
            | None -> post.avatar_url
    interface IDeletable with
        member __.SiteName = "Pillowfort"
        member __.DeleteAsync() = client.DeletePostAsync post.id

type PillowfortSourceWrapper(client: PillowfortClient) =
    inherit AsyncSeqWrapper()

    let wrap p m = PillowfortPostWrapper (client, p, m) :> IPostBase

    override __.Name = "Pillowfort"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable page = 1
        let mutable more = true

        let! username = this.WhoamiAsync() |> Async.AwaitTask

        while more do
            let! response = client.AsyncGetPosts username page
            for p in response.posts do
                if Seq.isEmpty p.media then
                    yield wrap p None
                else
                    for m in p.media do
                        if not (isNull m.url) then
                            let (success, _) = Uri.TryCreate(m.url, UriKind.Absolute)
                            if success then
                                yield wrap p (Some m)

            more <- 20 * (page - 1) + (Seq.length response.posts) > response.total_count
            page <- page + 1
    }

    override __.FetchUserInternal() = async {
        let! fn1 = Async.StartChild client.AsyncWhoami
        let! fn2 = Async.StartChild client.AsyncGetAvatar
        let! username = fn1
        let! avatar = fn2
        return {
            username = username
            icon_url = avatar
        }
    }