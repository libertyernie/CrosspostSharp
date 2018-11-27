namespace SourceWrappers

open InkbunnyLib
open System
open FSharp.Control

type InkbunnyPostWrapper(submission: InkbunnySubmissionDetail, client: InkbunnyClient) =
    interface IRemotePhotoPost with
        member this.Title = submission.title
        member this.HTMLDescription = submission.description_bbcode_parsed
        member this.Mature = submission.rating_id = InkbunnyRating.Mature
        member this.Adult = submission.rating_id = InkbunnyRating.Adult
        member this.Tags = submission.keywords |> Seq.map (fun s -> s.keyword_name)
        member this.Timestamp = submission.create_datetime.UtcDateTime
        member this.ViewURL = sprintf "https://inkbunny.net/submissionview.php?id=%d" submission.submission_id
        member this.ImageURL = submission.file_url_full
        member this.ThumbnailURL =
            (Option.ofObj submission.thumbnail_url_medium)
            |> Option.orElse (Option.ofObj submission.thumbnail_url_medium_noncustom)
            |> Option.defaultValue submission.file_url_full
    interface IDeletable with
        member this.SiteName = "Inkbunny"
        member this.DeleteAsync() = client.DeleteSubmissionAsync(submission.submission_id)

type InkbunnyDeferredPostWrapper(submission: InkbunnySubmission, client: InkbunnyClient) =
    inherit DeferredPhotoPost()

    override __.Title = submission.title
    override __.ViewURL = sprintf "https://inkbunny.net/submissionview.php?id=%d" submission.submission_id
    override __.ThumbnailURL =
        (Option.ofObj submission.thumbnail_url_medium)
        |> Option.orElse (Option.ofObj submission.thumbnail_url_medium_noncustom)
        |> Option.defaultValue submission.file_url_full
    override __.Timestamp = Some submission.create_datetime.UtcDateTime
    override __.AsyncGetActual() = async {
        let! details = client.GetSubmissionsAsync(Seq.singleton submission.submission_id, show_description_bbcode_parsed=true) |> Async.AwaitTask
        let s = Seq.head details.submissions
        return new InkbunnyPostWrapper(s, client) :> IRemotePhotoPost
    }

    interface IDeletable with
        member this.SiteName = "Inkbunny"
        member this.DeleteAsync() = client.DeleteSubmissionAsync(submission.submission_id)

type InkbunnySourceWrapper(client: InkbunnyClient, loadAll: bool) =
    inherit AsyncSeqWrapper()

    let initialFetch maxCount =
        let searchParams = new InkbunnySearchParameters()
        searchParams.UserId <- client.UserId |> Nullable
        client.SearchAsync(searchParams, maxCount |> Option.toNullable) |> Async.AwaitTask

    let furtherFetch rid page maxCount =
        client.SearchAsync(rid, page, maxCount |> Option.toNullable) |> Async.AwaitTask

    let getDetails (submissions: seq<InkbunnySearchSubmission>) = async {
        let! details = client.GetSubmissionsAsync(submissions |> Seq.map (fun s -> s.submission_id), show_description_bbcode_parsed=true) |> Async.AwaitTask
        if details.error_code.HasValue then
            failwith details.error_message
        return details.submissions
    }

    let fetch count = asyncSeq {
        let mutable rid: string option = None
        let mutable page = 1
        let mutable more = true
        let maxCount = count |> Option.map (min 100)

        while more do
            let! response =
                match page with
                | 1 -> initialFetch maxCount
                | p ->
                    match rid with
                    | None -> initialFetch maxCount
                    | Some r -> furtherFetch r p maxCount

            rid <- Some response.rid
        
            if loadAll then
                // We need to get more information on these submissions so we can grab the description and tags.
                let! details = getDetails response.submissions

                for o in details |> Seq.sortByDescending (fun s -> s.create_datetime) do
                    if o.``public``.value then
                        yield new InkbunnyPostWrapper(o, client) :> IPostBase
            else
                for o in response.submissions |> Seq.sortByDescending (fun s -> s.create_datetime) do
                    if o.``public``.value then
                        yield new InkbunnyDeferredPostWrapper(o, client) :> IPostBase

            page <- response.page + 1
            more <- response.pages_count >= page
    }

    member val BatchSize: Nullable<int> = Nullable() with get, set

    override __.Name = "Inkbunny"

    override this.FetchSubmissionsInternal() = fetch (Option.ofNullable this.BatchSize)

    override __.FetchUserInternal() = async {
        let! response = initialFetch (Some 1)
        if Seq.isEmpty response.submissions then
            failwith "Cannot get Inkbunny username and icon if no submissions are present"

        let! details = getDetails response.submissions
        let first = Seq.head details
        return {
            username = first.username
            icon_url = Option.ofObj first.user_icon_url_small
        }
    }