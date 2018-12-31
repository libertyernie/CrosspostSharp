namespace SourceWrappers

open FSharp.Control
open DeviantArtFs

type internal Status = DeviantArtFs.User.StatusesStatusResponse.Root
type internal StatusDeviation = DeviantArtFs.User.StatusesStatusResponse.Deviation

type DeviantArtStatusPostWrapper(status: Status, deviation: StatusDeviation option) =
    let Mature =
        deviation
        |> Option.map (fun d -> d.IsMature)
        |> Option.defaultValue false

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.Ts.UtcDateTime
        member this.ViewURL = status.Url

type DeviantArtStatusPhotoPostWrapper(status: Status, deviation: StatusDeviation) =
    inherit DeviantArtStatusPostWrapper(status, Some deviation)
    let Icon = status.Author.Usericon

    interface IRemotePhotoPost with
        member __.ImageURL =
            deviation.Content
            |> Option.map (fun d -> d.Src)
            |> Option.defaultValue Icon
        member __.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun t -> t.Src)
            |> Seq.tryHead
            |> Option.defaultValue Icon

type DeviantArtStatusSourceWrapper(client: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    override this.Name = "DeviantArt (statuses)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable position = 0
        let mutable more = true

        let! username = this.WhoamiAsync() |> Async.AwaitTask

        while more do
            let! statuses =
                new DeviantArtFs.User.StatusesListRequest(username, Offset = position, Limit = 50)
                |> DeviantArtFs.User.StatusesList.AsyncExecute client

            for r in statuses.Results do
                let items = seq {
                    for i in r.Items do
                        match i.Deviation with
                        | Some d -> yield d
                        | None -> ()
                }
                if Seq.isEmpty items then
                    yield new DeviantArtStatusPostWrapper(r, None) :> IPostBase
                else
                    for d in items do
                        yield new DeviantArtStatusPhotoPostWrapper(r, d) :> IPostBase

            position <-
                statuses.NextOffset
                |> Option.defaultValue position
            more <- statuses.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }