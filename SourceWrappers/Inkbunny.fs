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
            [submission.thumbnail_url_medium; submission.thumbnail_url_medium_noncustom]
            |> Seq.filter (fun s -> not <| isNull s)
            |> Seq.tryHead
            |> Option.defaultValue submission.file_url_full
    interface IDeletable with
        member this.SiteName = "Inkbunny"
        member this.DeleteAsync() = client.DeleteSubmissionAsync(submission.submission_id)

type InkbunnySourceWrapper(client: InkbunnyClient, batchSize: int) =
    inherit AsyncSeqWrapper()

    let initialFetch maxCount =
        let searchParams = new InkbunnySearchParameters()
        searchParams.UserId <- client.UserId |> Nullable
        client.SearchAsync(searchParams, maxCount |> Nullable) |> Async.AwaitTask

    let furtherFetch rid page maxCount =
        client.SearchAsync(rid, page, maxCount |> Nullable) |> Async.AwaitTask

    let fetch max = asyncSeq {
        let mutable rid: string option = None
        let mutable page = 1
        let mutable more = true
        let maxCount = max |> min 100

        while more do
            let! response =
                match page with
                | 1 -> initialFetch maxCount
                | p ->
                    match rid with
                    | None -> initialFetch maxCount
                    | Some r -> furtherFetch r p maxCount

            rid <- Some response.rid
        
            let! details = client.GetSubmissionsAsync(response.submissions |> Seq.map (fun s -> s.submission_id), show_description_bbcode_parsed=true) |> Async.AwaitTask

            for o in details.submissions |> Seq.sortByDescending (fun s -> s.create_datetime) do
                if o.``public``.value then
                    yield o

            page <- response.page + 1
            more <- response.pages_count >= page
    }

    override __.Name = "Inkbunny"

    override __.FetchSubmissionsInternal() =
        fetch batchSize
        |> AsyncSeq.map (fun o -> new InkbunnyPostWrapper(o, client) :> IPostBase)

    override __.FetchUserInternal() = async {
        let! submission = fetch 1 |> AsyncSeq.tryFirst
        return match submission with
        | Some s ->
            {
                username = s.username
                icon_url = Option.ofObj s.user_icon_url_small
            }
        | None -> failwith "Cannot get Inkbunny username and icon if no submissions are present"
    }