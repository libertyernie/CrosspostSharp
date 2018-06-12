namespace SourceWrappers

open DeviantartApi.Objects
open System
open DeviantartApi.Requests.User
open AsyncHelpers
open DeviantartApi.Requests.Gallery
open DeviantartApi.Requests.Deviation

type DeviantArtStatusPostWrapper(status: Status) =
    let Deviations =
        status.Items
        |> Seq.map (fun i -> i.Deviation)
        |> Seq.filter (not << isNull)
    let MainDeviation =
        Deviations
        |> Seq.filter (fun d -> d.Author.UserId = status.Author.UserId)
        |> Seq.tryHead
    let OtherDeviations =
        match MainDeviation with
            | Some d -> Deviations |> Seq.except [d]
            | None -> Deviations

    let Icon = status.Author.UserIconUrl.AbsoluteUri

    let Mature = Deviations |> Seq.exists (fun d -> d.IsMature |> Option.ofNullable |> Option.defaultValue false)

    interface IPostWrapper with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.TimeStamp
        member this.ViewURL = status.Url.AbsoluteUri
        member this.ImageURL =
            match MainDeviation with
                | Some d -> if isNull d.Content then Icon else d.Content.Src
                | None -> Icon
        member this.ThumbnailURL =
            match MainDeviation with
                | Some d -> d.Thumbs |> Seq.map (fun t -> t.Src) |> Seq.tryHead |> Option.defaultValue Icon
                | None -> Icon

    interface IStatusUpdate with
        member this.PotentiallySensitive = Mature
        member this.FullHTML = status.Body
        member this.HasPhoto = Option.isSome MainDeviation
        member this.AdditionalLinks = OtherDeviations |> Seq.map (fun d -> d.Url.AbsoluteUri)

type DeviantArtStatusSourceWrapper() =
    inherit SourceWrapper<uint32>()

    let mutable cached_user: User = null

    let getUser = async {
        if isNull cached_user then
            let req = new WhoAmIRequest()
            let! u =
                req.ExecuteAsync()
                |> Async.AwaitTask
                |> whenDone processDeviantArtError
            cached_user <- u
        return cached_user
    }
    
    override this.Name = "DeviantArt (statuses)"

    override this.Fetch cursor take = async {
        let position = cursor |> Option.defaultValue (uint32 0)

        let! username = this.Whoami

        let statusesRequest = new StatusesRequest(username)
        statusesRequest.Limit <- take |> uint32 |> Nullable
        statusesRequest.Offset <- position |> Nullable

        let! statuses =
            statusesRequest.ExecuteAsync()
            |> Async.AwaitTask
            |> whenDone processDeviantArtError
        
        return {
            Posts = statuses.Results |> Seq.map (fun s -> new DeviantArtStatusPostWrapper(s) :> IPostWrapper)
            Next = statuses.NextOffset |> Option.ofNullable |> Option.defaultValue 0 |> uint32
            HasMore = statuses.HasMore
        }
    }

    override this.Whoami = getUser |> whenDone (fun u -> u.Username)

    override this.GetUserIcon size = getUser |> whenDone (fun u -> u.UserIconUrl.AbsoluteUri)