namespace SourceWrappers

open FAExportLib
open System
open FSharp.Control

type FurAffinityMinimalPostWrapper(submission: FAFolderSubmission) =
    member this.Id = submission.Id

    interface IRemotePhotoPost with
        member this.Title = submission.Title
        member this.HTMLDescription = ""
        member this.Mature = true
        member this.Adult = true
        member this.Tags = Seq.empty
        member this.Timestamp = DateTime.UtcNow
        member this.ViewURL = submission.Link
        member this.ImageURL = submission.Thumbnail
        member this.ThumbnailURL = submission.Thumbnail

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

type FurAffinityMinimalSourceWrapper(a: string, b: string, scraps: bool) =
    inherit AsyncSeqWrapper()

    let apiClient = new FAUserClient(a, b)

    let mutable cached_username: string = null

    let getUser = async {
        if isNull cached_username then
            let! u = apiClient.WhoamiAsync() |> Async.AwaitTask
            cached_username <- u
        return cached_username
    }

    member this.GetSubmission id = async {
        return! apiClient.GetSubmissionAsync id |> Async.AwaitTask
    }
    
    override this.Name = if scraps then "Fur Affinity (scraps)" else "Fur Affinity (gallery)"

    override this.StartNew() = asyncSeq {
        let mutable page = 1
        let mutable more = true

        let! username = getUser

        while more do
            let folder = if scraps then FAFolder.scraps else FAFolder.gallery

            let! gallery = apiClient.GetSubmissionsAsync(username, folder, page) |> Async.AwaitTask
            for post in gallery do
                yield new FurAffinityMinimalPostWrapper(post) :> IPostBase
        
            page <- page + 1
            more <- Seq.length gallery > 0
    }

    override this.Whoami = getUser

    override this.GetUserIcon size = async {
        let! username = getUser
        let! user = apiClient.GetUserAsync(username) |> Async.AwaitTask
        return user.Avatar
    }

type FurAffinitySourceWrapper(a: string, b: string, scraps: bool) =
    inherit AsyncSeqWrapper()

    let source = new FurAffinityMinimalSourceWrapper(a, b, scraps)

    let wrap id = async {
        let! post = source.GetSubmission id
        return new FurAffinityPostWrapper(post) :> IPostBase
    }

    override this.Name = source.Name

    override this.StartNew() =
        source
        |> AsyncSeq.map (fun w -> w :?> FurAffinityMinimalPostWrapper)
        |> AsyncSeq.map (fun w -> w.Id)
        |> AsyncSeq.mapAsync wrap

    override this.Whoami = source.Whoami
    override this.GetUserIcon size = source.GetUserIcon size