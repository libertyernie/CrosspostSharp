namespace SourceWrappers

open WeasylLib.Api
open WeasylLib.Frontend
open FSharp.Control

type WeasylDeferredPostWrapper(submission: WeasylGallerySubmission, get: Async<IRemotePhotoPost>) =
    inherit DeferredPhotoPost()

    override __.Title = submission.title
    override __.ViewURL = submission.link
    override __.ThumbnailURL = submission.media.thumbnail |> Seq.map (fun s -> s.url) |> Seq.head
    override __.AsyncGetActual() = get

type WeasylPostWrapper(submission: WeasylSubmissionBaseDetail) =
    interface IRemotePhotoPost with
        member this.Title = submission.title
        member this.HTMLDescription = submission.HTMLDescription
        member this.Mature = submission.rating = "mature"
        member this.Adult = submission.rating = "explicit"
        member this.Tags = submission.tags :> seq<string>
        member this.Timestamp = submission.posted_at
        member this.ViewURL = submission.link
        member this.ImageURL = submission.media.submission |> Seq.map (fun s -> s.url) |> Seq.head
        member this.ThumbnailURL = submission.media.thumbnail |> Seq.map (fun s -> s.url) |> Seq.head

type WeasylSubmissionWrapper(submission: WeasylSubmissionDetail, client: WeasylFrontendClient) =
    inherit WeasylPostWrapper(submission)

    interface IDeletable with
        member this.SiteName = "Weasyl"
        member this.DeleteAsync() = client.DeleteSubmissionAsync(submission.submitid)

type WeasylSourceWrapper(username: string, loadAll: bool, frontendClientParam: WeasylFrontendClient) =
    inherit AsyncSeqWrapper()

    let apiClient = new WeasylApiClient()
    let frontendClient = Option.ofObj frontendClientParam
    
    override __.Name = "Weasyl"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor: int option = None
        let mutable more = true

        while more do
            let gallery_options = new WeasylApiClient.GalleryRequestOptions()
            gallery_options.nextid <- Option.toNullable cursor

            let! gallery = apiClient.GetUserGalleryAsync(username, gallery_options) |> Async.AwaitTask

            for s1 in gallery.submissions do
                let get = async {
                    let! s2 = apiClient.GetSubmissionAsync(s1.submitid) |> Async.AwaitTask
                    match frontendClient with
                    | Some f -> return new WeasylSubmissionWrapper(s2, f) :> IRemotePhotoPost
                    | None -> return new WeasylPostWrapper(s2) :> IRemotePhotoPost
                }
                
                if loadAll then
                    let! post = get
                    yield post :> IPostBase
                else
                    yield new WeasylDeferredPostWrapper(s1, get) :> IPostBase

            cursor <- Option.ofNullable gallery.nextid
            more <- Option.isSome cursor
    }

    override __.FetchUserInternal() = async {
        let! icon_url = apiClient.GetAvatarUrlAsync(username) |> Async.AwaitTask
        return {
            username = username
            icon_url = Option.ofObj icon_url
        }
    }

type WeasylCharacterSourceWrapper(username: string) =
    inherit AsyncSeqWrapper()

    let apiClient = new WeasylApiClient()
    
    override __.Name = "Weasyl (characters)"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let! allIds = Scraper.GetCharacterIdsAsync(username) |> Async.AwaitTask
        for id in allIds do
            let! c = apiClient.GetCharacterAsync(id) |> Async.AwaitTask
            yield new WeasylPostWrapper(c) :> IPostBase
    }

    override __.FetchUserInternal() = async {
        let! icon_url = apiClient.GetAvatarUrlAsync(username) |> Async.AwaitTask
        return {
            username = username
            icon_url = Option.ofObj icon_url
        }
    }