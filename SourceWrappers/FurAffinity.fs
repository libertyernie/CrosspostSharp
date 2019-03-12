namespace SourceWrappers

open FSharp.Control
open FurAffinityFs
open System

type FurAffinityMinimalPostWrapper(submission: Models.GalleryThumbnail, get: Async<IRemotePhotoPost>) =
    inherit DeferredPhotoPost()

    override __.Title = submission.title
    override __.ViewURL = submission.href.AbsoluteUri
    override __.ThumbnailURL = submission.thumbnail.AbsoluteUri

    override __.Timestamp = None

    override __.AsyncGetActual() = get

type FurAffinityPostWrapper(submission: Models.ExistingSubmission, timeZone: TimeZoneInfo) =
    interface IRemotePhotoPost with
        member __.Title = submission.title
        member __.HTMLDescription = submission.description
        member __.Mature = submission.rating <> Models.Rating.General
        member __.Adult = submission.rating = Models.Rating.Adult
        member __.Tags = submission.keywords
        member __.Timestamp = TimeZoneInfo.ConvertTimeToUtc(submission.date, timeZone)
        member __.ViewURL = submission.href.AbsoluteUri
        member __.ImageURL = submission.full.AbsoluteUri
        member __.ThumbnailURL = submission.thumbnail.AbsoluteUri

[<AbstractClass>]
type FurAffinityAbstractSourceWrapper(credentials: IFurAffinityCredentials, scraps: bool) =
    inherit AsyncSeqWrapper()

    abstract AsyncGetUsername: unit -> Async<string>

    member __.GetSubmission id = Requests.ViewSubmission.AsyncExecute credentials id
    
    override __.Name = if scraps then "Fur Affinity (scraps)" else "Fur Affinity (gallery)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let folder =
            if scraps
            then Requests.Gallery.Folder.Scraps
            else Requests.Gallery.Folder.Gallery
        let! username = this.AsyncGetUsername()
        let! tz = Requests.GetTimeZone.AsyncExecuteWithDefault credentials TimeZoneInfo.Local

        for post in Requests.Gallery.ToAsyncSeq credentials folder username do
            let get = async {
                let! p = this.GetSubmission post.sid
                return new FurAffinityPostWrapper(p, tz) :> IRemotePhotoPost
            }
            yield new FurAffinityMinimalPostWrapper(post, get) :> IPostBase
    }

    override this.FetchUserInternal() = async {
        let! username = this.AsyncGetUsername()
        let! icon_uri =
            FurAffinityFs.Requests.UserPage.AsyncExecute credentials username
            |> Swu.whenDone (fun o -> Some o.avatar.AbsoluteUri)
        return {
            username = username
            icon_url = icon_uri
        }
    }

type FurAffinityMinimalSourceWrapper(creds: IFurAffinityCredentials, scraps: bool) =
    inherit FurAffinityAbstractSourceWrapper(creds, scraps)

    override __.AsyncGetUsername() = FurAffinityFs.Requests.Whoami.AsyncExecute creds
