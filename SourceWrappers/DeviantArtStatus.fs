namespace SourceWrappers

open FSharp.Control
open DeviantArtFs
open System

type DeviantArtStatusPostWrapper(status: DeviantArtStatus) =
    let Mature =
        status.GetEmbeddedDeviations()
        |> Seq.exists (fun i -> i.is_mature = Some true)

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription =
            status.body
            |> Option.defaultValue ""
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp =
            status.ts
            |> Option.map (fun d -> d.UtcDateTime)
            |> Option.defaultValue DateTime.UtcNow
        member this.ViewURL =
            status.url
            |> Option.defaultValue "https://www.example.com"

type DeviantArtStatusPhotoPostWrapper(status: DeviantArtStatus, deviation: Deviation) =
    inherit DeviantArtStatusPostWrapper(status)

    let icon =
        status.author
        |> Option.map (fun a -> a.usericon)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    interface IRemotePhotoPost with
        member __.ImageURL =
            deviation.content
            |> Option.map (fun d -> d.src)
            |> Option.defaultValue icon
        member __.ThumbnailURL =
            deviation.thumbs
            |> Option.defaultValue List.empty
            |> Seq.map (fun t -> t.src)
            |> Seq.tryHead
            |> Option.defaultValue icon

type DeviantArtStatusSourceWrapper(client: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    override this.Name = "DeviantArt (statuses)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let! username = this.WhoamiAsync() |> Async.AwaitTask
        for r in DeviantArtFs.Requests.User.StatusesList.ToAsyncSeq client 0 username do
            if r.GetEmbeddedDeviations() |> Seq.isEmpty then
                yield new DeviantArtStatusPostWrapper(r) :> IPostBase
            else
                for d in r.GetEmbeddedDeviations() do
                    yield new DeviantArtStatusPhotoPostWrapper(r, d) :> IPostBase
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute client
        return {
            username = u.username
            icon_url = Some u.usericon
        }
    }