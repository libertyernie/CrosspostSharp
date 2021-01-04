namespace SourceWrappers

open ArtworkSourceSpecification
open System
open System.Text
open System.Threading.Tasks
open DeviantArtFs
open FSharp.Control

type StashPostWrapper(itemId: int64, metadata: StashMetadata, token: IDeviantArtAccessToken) =
    let imageUrl =
        metadata.files
        |> Option.map Seq.ofList
        |> Option.defaultValue Seq.empty
        |> Seq.where (fun f -> f.width * f.height > 0)
        |> Seq.sortByDescending (fun f -> f.width)
        |> Seq.map (fun f -> f.src)
        |> Seq.tryHead
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    member __.ItemId = itemId

    interface IRemotePhotoPost with
        member this.Title = metadata.title
        member this.HTMLDescription = metadata.artist_comments |> Option.defaultValue ""
        member this.Mature = false
        member this.Adult = false
        member this.Tags = metadata.tags |> Option.map Seq.ofList |> Option.defaultValue Seq.empty
        member this.Timestamp =
            metadata.creation_time
            |> Option.map (fun t -> t.UtcDateTime)
            |> Option.defaultValue DateTime.MinValue
        member this.ViewURL =
            let url = new StringBuilder()
            let mutable itemId = itemId
            while itemId > int64 0 do
                let n = itemId % int64 36
                let c = "0123456789abcdefghijklmnopqrstuvwxyz".[int32 n]
                url.Insert(0, c) |> ignore
                itemId <- itemId / int64 36
            url.Insert(0, "https://sta.sh/0") |> ignore
            url.ToString()
        member this.ImageURL = imageUrl
        member this.ThumbnailURL =
            metadata.thumb
            |> Option.map (fun t -> t.src)
            |> Option.defaultValue imageUrl

type StashSourceWrapper(token: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    let getTimestamp (o: StashDeltaEntry) =
       o.metadata
        |> Option.bind (fun m -> m.creation_time)
        |> Option.defaultValue DateTimeOffset.MinValue

    override __.Name = "Sta.sh"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let req = new DeviantArtFs.Api.Stash.DeltaRequest()
        let! source = DeviantArtFs.Api.Stash.Delta.ToAsyncSeq token req 0 |> AsyncSeq.toListAsync |> Swu.whenDone (List.sortByDescending getTimestamp)
        for o in source do
            match (o.itemid, o.metadata) with
            | (Some itemid, Some metadata) -> yield new StashPostWrapper(itemid, metadata, token) :> IPostBase
            | _ -> ()
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Api.User.Whoami.AsyncExecute token DeviantArtObjectExpansion.None
        return {
            username = u.username
            icon_url = Some u.usericon
        }
    }