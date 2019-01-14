namespace SourceWrappers

open FSharp.Control
open DeviantArtFs

type DeviantArtStatusPostWrapper(status: DeviantArtStatus) =
    let Mature =
        status.EmbeddedDeviations
        |> Seq.exists (fun i -> i.IsMature)

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.Ts.UtcDateTime
        member this.ViewURL = status.Url

type DeviantArtStatusPhotoPostWrapper(status: DeviantArtStatus, deviation: Deviation) =
    inherit DeviantArtStatusPostWrapper(status)

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
        let! username = this.WhoamiAsync() |> Async.AwaitTask
        for r in DeviantArtFs.Requests.User.StatusesList.ToAsyncSeq client username 0 do
            if Seq.isEmpty r.EmbeddedDeviations then
                yield new DeviantArtStatusPostWrapper(r) :> IPostBase
            else
                for d in r.EmbeddedDeviations do
                    let dd = d :?> Deviation
                    yield new DeviantArtStatusPhotoPostWrapper(r, dd) :> IPostBase
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }