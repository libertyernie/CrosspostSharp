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

    member this.GetSubmission id = async {
        return! apiClient.GetSubmissionAsync id |> Async.AwaitTask
    }
    
    override this.Name = if scraps then "Fur Affinity (scraps)" else "Fur Affinity (gallery)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable page = 1
        let mutable more = true

        let! user = this.AsyncGetUser()

        while more do
            let folder = if scraps then FAFolder.scraps else FAFolder.gallery

            let! gallery = apiClient.GetSubmissionsAsync(user.username, folder, page) |> Async.AwaitTask
            for post in gallery do
                yield new FurAffinityMinimalPostWrapper(post) :> IPostBase
        
            page <- page + 1
            more <- Seq.length gallery > 0
    }

    override this.FetchUserInternal() = async {
        let! username = apiClient.WhoamiAsync() |> Async.AwaitTask
        let! user = apiClient.GetUserAsync(username) |> Async.AwaitTask
        return {
            username = username
            icon_url = Some user.Avatar
        }
    }

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
        |> AsyncSeq.map (fun w -> w.Id)
        |> AsyncSeq.mapAsync wrap

    override this.FetchUserInternal() = source.FetchUserInternal()