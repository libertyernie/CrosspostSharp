namespace SourceWrappers

open InkbunnyLib
open System
open AsyncHelpers

type InkbunnyPostWrapper(submission: InkbunnySubmissionDetail, client: InkbunnyClient) =
    interface IPostWrapper with
        member this.Title = submission.title
        member this.HTMLDescription = submission.description_bbcode_parsed
        member this.Mature = submission.rating_id = InkbunnyRating.Mature
        member this.Adult = submission.rating_id = InkbunnyRating.Adult
        member this.Tags = submission.keywords |> Seq.map (fun s -> s.keyword_name)
        member this.Timestamp = submission.create_datetime.UtcDateTime
        member this.ViewURL = sprintf "https://inkbunny.net/submissionview.php?id=%d" submission.submission_id
        member this.ImageURL = submission.file_url_full
        member this.ThumbnailURL =
            [submission.thumbnail_url_medium; submission.thumbnail_url_medium_noncustom]
            |> Seq.filter (fun s -> not <| isNull s)
            |> Seq.tryHead
            |> Option.defaultValue submission.file_url_full
    interface IDeletable with
        member this.SiteName = "Inkbunny"
        member this.DeleteAsync() = client.DeleteSubmissionAsync(submission.submission_id)

type InkbunnySourceWrapper(client: InkbunnyClient, batchSize: int) =
    inherit SourceWrapper<int>()

    let mutable rid: string option = None
    let mutable firstSubmission: InkbunnySubmissionDetail option = None

    let initialFetch maxCount =
        let searchParams = new InkbunnySearchParameters()
        searchParams.UserId <- client.UserId |> Nullable
        client.SearchAsync(searchParams, maxCount |> Nullable) |> Async.AwaitTask

    let furtherFetch rid page maxCount =
        client.SearchAsync(rid, page, maxCount |> Nullable) |> Async.AwaitTask

    let inkbunnyFetch cursor = async {
        let maxCount = batchSize |> min 100
        let! response =
            match cursor with
            | None -> initialFetch maxCount
            | Some c ->
                match rid with
                | None -> initialFetch maxCount
                | Some r -> furtherFetch r (c + 1) maxCount

        rid <- Some response.rid
        
        let! details = client.GetSubmissionsAsync(response.submissions |> Seq.map (fun s -> s.submission_id), show_description_bbcode_parsed=true) |> Async.AwaitTask
        
        firstSubmission <- details.submissions |> Seq.tryHead

        return {
            Posts = details.submissions
                |> Seq.filter (fun s -> s.``public``.value)
                |> Seq.sortByDescending (fun s -> s.create_datetime)
                |> Seq.map (fun s -> new InkbunnyPostWrapper(s, client))
                |> Seq.map (fun w -> w :> IPostWrapper)
            Next = response.page + 1
            HasMore = response.pages_count >= (cursor |> Option.defaultValue 1)
        }
    }

    let getFirstSubmission = async {
        match firstSubmission with
        | Some s -> return s
        | None ->
            do! inkbunnyFetch None |> whenDone ignore
            match firstSubmission with
            | Some s -> return s
            | None -> return failwith "Cannot get Inkbunny icon if no submissions are present"
    }

    override this.Name = "Inkbunny"
    override this.SuggestedBatchSize = batchSize

    override this.Fetch cursor take = inkbunnyFetch cursor

    override this.Whoami = async {
        let! s = getFirstSubmission
        return s.username
    }

    override this.GetUserIcon size = async {
        let! s = getFirstSubmission
        return s.user_icon_url_small
    }