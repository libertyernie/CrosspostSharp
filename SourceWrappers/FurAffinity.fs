namespace SourceWrappers

open FSharp.Control
open FurAffinityFs
open System

type FurAffinityMinimalPostWrapper(submission: Models.GalleryThumbnail, get: Async<IRemotePhotoPost>) =
    inherit DeferredPhotoPost()

    override this.Title = submission.title
    override this.ViewURL = submission.href.AbsoluteUri
    override this.ThumbnailURL = submission.thumbnail.AbsoluteUri

    override this.Timestamp = None

    override this.AsyncGetActual() = get

type FurAffinityPostWrapper(submission: Models.ExistingSubmission) =
    interface IRemotePhotoPost with
        member this.Title = submission.title
        member this.HTMLDescription = submission.description
        member this.Mature = submission.rating <> Models.Rating.General
        member this.Adult = submission.rating = Models.Rating.Adult
        member this.Tags = submission.keywords
        member this.Timestamp = DateTime.Now
        member this.ViewURL = submission.href.AbsoluteUri
        member this.ImageURL = submission.download.AbsoluteUri
        member this.ThumbnailURL =
            submission.thumbnail
            |> Option.map (fun u -> u.AbsoluteUri)
            |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

[<AbstractClass>]
type FurAffinityAbstractSourceWrapper(scraps: bool) =
    inherit AsyncSeqWrapper()

    abstract GetCredentials: unit -> IFurAffinityCredentials
    abstract AsyncGetUsername: unit -> Async<string>

    member this.GetSubmission id =
        Requests.ViewSubmission.AsyncExecute (this.GetCredentials()) id
    
    override __.Name = if scraps then "Fur Affinity (scraps)" else "Fur Affinity (gallery)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let folder =
            if scraps
            then Requests.Gallery.Folder.Scraps
            else Requests.Gallery.Folder.Gallery
        let! username = this.AsyncGetUsername()
        let mutable page_href = Some (Requests.Gallery.GetFirstPageHref folder username)

        let credentials = this.GetCredentials()

        while Option.isSome page_href do
            let! gallery = Requests.Gallery.AsyncExecute credentials page_href.Value
            for post in gallery.submissions do
                let get = async {
                    let! p = this.GetSubmission post.sid
                    return new FurAffinityPostWrapper(p) :> IRemotePhotoPost
                }
                yield new FurAffinityMinimalPostWrapper(post, get) :> IPostBase

            page_href <- gallery.next_page_href
    }

    override this.FetchUserInternal() = async {
        let! username = this.AsyncGetUsername()
        let! icon_uri =
            FurAffinityFs.Requests.UserPage.AsyncExecute (this.GetCredentials()) username
            |> Swu.whenDone (fun o -> Some o.avatar.AbsoluteUri)
        return {
            username = username
            icon_url = icon_uri
        }
    }

type FurAffinityUserSourceWrapper(creds: IFurAffinityCredentials, username: string, scraps: bool) =
    inherit FurAffinityAbstractSourceWrapper(scraps)

    override __.GetCredentials() = creds
    override __.AsyncGetUsername() = async { return username }

type FurAffinityMinimalSourceWrapper(creds: IFurAffinityCredentials, scraps: bool) =
    inherit FurAffinityAbstractSourceWrapper(scraps)

    override __.GetCredentials() = creds
    override __.AsyncGetUsername() = FurAffinityFs.Requests.Whoami.AsyncExecute creds

type FurAffinitySourceWrapper(creds: IFurAffinityCredentials, scraps: bool) =
    inherit AsyncSeqWrapper()

    let source = new FurAffinityMinimalSourceWrapper(creds, scraps)

    let wrap id = async {
        let! post = source.GetSubmission id
        return new FurAffinityPostWrapper(post) :> IPostBase
    }

    override this.Name = source.Name

    override this.FetchSubmissionsInternal() =
        source
        |> AsyncSeq.map (fun w -> w :?> FurAffinityMinimalPostWrapper)
        |> AsyncSeq.mapAsync (fun w -> w.AsyncGetActual())
        |> AsyncSeq.map (fun w -> w :> IPostBase)

    override this.FetchUserInternal() = source.FetchUserInternal()