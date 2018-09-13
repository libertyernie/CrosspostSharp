namespace SourceWrappers

open FAExportLib
open System

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
    inherit SourceWrapper<int>()

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
    override this.SuggestedBatchSize = 60

    override this.Fetch cursor take = async {
        let! username = getUser
        let folder = if scraps then FAFolder.scraps else FAFolder.gallery
        let page = cursor |> Option.defaultValue 1

        let! gallery = apiClient.GetSubmissionsAsync(username, folder, page) |> Async.AwaitTask
        
        return {
            Posts = gallery |> Seq.map FurAffinityMinimalPostWrapper |> Seq.cast
            Next = page + 1
            HasMore = Seq.length gallery > 0
        }
    }

    override this.Whoami = getUser

    override this.GetUserIcon size = async {
        let! username = getUser
        let! user = apiClient.GetUserAsync(username) |> Async.AwaitTask
        return user.Avatar
    }

type FurAffinitySourceWrapper(a: string, b: string, scraps: bool) =
    inherit SourceWrapper<int>()

    let source = new FurAffinityMinimalSourceWrapper(a, b, scraps)
    
    let cache = new System.Collections.Generic.List<int>()
    let mutable cache_cursor: int option = None
    let mutable cache_has_more = true
    
    override this.Name = source.Name
    override this.SuggestedBatchSize = 4

    override this.Fetch cursor take = async {
        let skip = cursor |> Option.defaultValue 0
        while cache.Count < skip + take && cache_has_more do
            let! result = source.Fetch cache_cursor 60
            result.Posts
                |> Seq.map (fun w -> w :?> FurAffinityMinimalPostWrapper)
                |> Seq.map (fun w -> w.Id)
                |> cache.AddRange
            cache_cursor <- Some result.Next
            cache_has_more <- result.HasMore

        let ids =
            cache
            |> Swu.skipSafe skip
            |> Seq.truncate take

        let v = Seq.length ids

        let! gallery =
            ids
            |> Seq.map source.GetSubmission
            |> Async.Parallel

        return {
            Posts = gallery |> Seq.map FurAffinityPostWrapper |> Seq.cast
            Next = skip + take
            HasMore = Seq.length gallery > 0
        }
    }

    override this.Whoami = source.Whoami
    override this.GetUserIcon size = source.GetUserIcon size