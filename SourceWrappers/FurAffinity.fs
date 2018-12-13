namespace SourceWrappers

open FAExportLib
open System
open FSharp.Control
open FurAffinityFs

type FurAffinityMinimalPostWrapper(submission: FAFolderSubmission, get: Async<IRemotePhotoPost>) =
    inherit DeferredPhotoPost()

    override this.Title = submission.Title
    override this.ViewURL = submission.Link
    override this.ThumbnailURL = submission.Thumbnail

    override this.Timestamp = None

    override this.AsyncGetActual() = get

type FurAffinityPostWrapper(submission: FASubmission) =
    interface IRemotePhotoPost with
        member this.Title = submission.Title
        member this.HTMLDescription =
            let html = submission.Description
            let index = html.IndexOf("<br><br>")
            if index > 1 then
                html.Substring(index + 8).TrimStart()
            else
                html
        member this.Mature = submission.Rating.ToLowerInvariant() = "mature"
        member this.Adult = submission.Rating.ToLowerInvariant() = "explicit"
        member this.Tags = submission.Keywords
        member this.Timestamp = submission.PostedAt.UtcDateTime
        member this.ViewURL = submission.Link
        member this.ImageURL = submission.Download
        member this.ThumbnailURL = submission.Thumbnail

[<AbstractClass>]
type FurAffinityAbstractSourceWrapper(scraps: bool) =
    inherit AsyncSeqWrapper()

    abstract GetAPIClient: unit -> FAClient
    abstract GetScraper: unit -> FurAffinityClient option
    abstract AsyncGetUsername: unit -> Async<string>

    member this.GetSubmission id = async {
        let apiClient = this.GetAPIClient()
        return! apiClient.GetSubmissionAsync id |> Async.AwaitTask
    }
    
    override __.Name = if scraps then "Fur Affinity (scraps)" else "Fur Affinity (gallery)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable page = 1
        let mutable more = true

        let! username = this.AsyncGetUsername()
        let apiClient = this.GetAPIClient()

        while more do
            let folder = if scraps then FAFolder.scraps else FAFolder.gallery

            let! gallery = apiClient.GetSubmissionsAsync(username, folder, page) |> Async.AwaitTask
            for post in gallery do
                let get = async {
                    let! p = this.GetSubmission post.Id
                    return new FurAffinityPostWrapper(p) :> IRemotePhotoPost
                }
                yield new FurAffinityMinimalPostWrapper(post, get) :> IPostBase
        
            page <- page + 1
            more <- Seq.length gallery > 0
    }

    override this.FetchUserInternal() = async {
        let dt = DateTime.UtcNow
        let! username = this.AsyncGetUsername()
        DateTime.UtcNow - dt |> printfn "A %O"

        let client = this.GetAPIClient()
        let! icon_uri =
            match this.GetScraper() with
            | Some s -> s.AsyncGetAvatarUri username
            | None -> async { return None }
        DateTime.UtcNow - dt |> printfn "B %O"
        return {
            username = username
            icon_url = icon_uri |> Option.map (fun u -> u.AbsoluteUri)
        }
    }

type FurAffinityUserSourceWrapper(username: string, scraps: bool) =
    inherit FurAffinityAbstractSourceWrapper(scraps)

    override __.GetAPIClient() = new FAClient()
    override __.GetScraper() = None
    override __.AsyncGetUsername() = async { return username }

type FurAffinityMinimalSourceWrapper(a: string, b: string, scraps: bool) =
    inherit FurAffinityAbstractSourceWrapper(scraps)

    let apiClient = FAUserClient(a, b)
    let scraper = FurAffinityClient(a, b)

    override __.GetAPIClient() = apiClient :> FAClient
    override __.GetScraper() = Some scraper
    override __.AsyncGetUsername() = scraper.AsyncWhoami

type FurAffinitySourceWrapper(a: string, b: string, scraps: bool) =
    inherit AsyncSeqWrapper()

    let source = new FurAffinityMinimalSourceWrapper(a, b, scraps)

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