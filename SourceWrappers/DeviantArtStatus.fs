namespace SourceWrappers

open FSharp.Control
open DeviantArtFs

type DeviantArtStatusPostWrapper(status_obj: Status) =
    let status = status_obj.Original

    let Mature =
        status_obj.EmbeddedDeviations
        |> Seq.exists (fun d -> d.Original.IsMature |> Option.defaultValue false)

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.Ts.UtcDateTime
        member this.ViewURL = status.Url

type DeviantArtStatusPhotoPostWrapper(status_obj: Status, deviation_obj: Deviation) =
    inherit DeviantArtStatusPostWrapper(status_obj)

    let status = status_obj.Original
    let deviation = deviation_obj.Original

    let icon = status.Author.Usericon

    interface IRemotePhotoPost with
        member __.ImageURL =
            deviation.Content
            |> Option.map (fun d -> d.Src)
            |> Option.defaultValue icon
        member __.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun t -> t.Src)
            |> Seq.tryHead
            |> Option.defaultValue icon

type DeviantArtStatusSourceWrapper(client: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    override this.Name = "DeviantArt (statuses)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable position = 0
        let mutable more = true

        let! username = this.WhoamiAsync() |> Async.AwaitTask

        while more do
            let! statuses =
                new DeviantArtFs.Requests.User.StatusesListRequest(username, Offset = position, Limit = 50)
                |> DeviantArtFs.Requests.User.StatusesList.AsyncExecute client

            for r in statuses.Results do
                if Seq.isEmpty r.EmbeddedDeviations then
                    yield new DeviantArtStatusPostWrapper(r) :> IPostBase
                else
                    for d in r.EmbeddedDeviations do
                        yield new DeviantArtStatusPhotoPostWrapper(r, d) :> IPostBase

            position <-
                statuses.NextOffset
                |> Option.defaultValue position
            more <- statuses.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }