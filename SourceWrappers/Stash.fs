namespace SourceWrappers

open System
open DeviantartApi.Objects.SubObjects.StashDelta
open System.Text
open System.Threading.Tasks
open DeviantartApi.Requests.Stash
open FSharp.Control
open DeviantartApi.Requests.User

type StashPostWrapper(entry: Entry) =
    let imageUrl =
        entry.Metadata.Files
        |> Seq.sortByDescending (fun f -> f.Width)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.defaultValue null

    let delete = async {
        let req = new DeviantartApi.Requests.Stash.DeleteRequest(entry.ItemId)
        do! Swu.executeAsync req |> Swu.whenDone ignore
    }

    member __.ItemId = entry.ItemId

    interface IRemotePhotoPost with
        member this.Title = entry.Metadata.Title
        member this.HTMLDescription = entry.Metadata.ArtistComment
        member this.Mature = false
        member this.Adult = false
        member this.Tags = entry.Metadata.Tags :> seq<string>
        member this.Timestamp = entry.Metadata.CreationTime
        member this.ViewURL =
            let url = new StringBuilder()
            let mutable itemId = entry.ItemId
            while itemId > int64 0 do
                let n = itemId % int64 36
                let c = "0123456789abcdefghijklmnopqrstuvwxyz".[int32 n]
                url.Insert(0, c) |> ignore
                itemId <- itemId / int64 36
            url.Insert(0, "https://sta.sh/0") |> ignore
            url.ToString()
        member this.ImageURL = imageUrl
        member this.ThumbnailURL =
            if isNull entry.Metadata.Thumb
            then imageUrl
            else entry.Metadata.Thumb.Src

    interface IDeletable with
        member this.SiteName = "Sta.sh"
        member this.DeleteAsync() = delete |> Async.StartAsTask :> Task

type UnorderedStashSourceWrapper() =
    inherit AsyncSeqWrapper()

    override __.Name = "Sta.sh"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = uint32 0
        let mutable more = true
        
        while more do
            let deltaRequest = new DeltaRequest()
            deltaRequest.Limit <- uint32 120 |> Nullable
            deltaRequest.Offset <- cursor |> Nullable

            let! result = Swu.executeAsync deltaRequest

            let wrappers =
                result.Entries
                |> Seq.filter (fun e -> e.ItemId <> int64 0)
                |> Seq.filter (fun e -> (not << isNull) e.Metadata.Files)
                |> Seq.map StashPostWrapper
            for o in wrappers do
                yield o :> IPostBase

            cursor <- result.NextOffset
                |> Option.ofNullable
                |> Option.defaultValue 0
                |> uint32
            more <- result.HasMore
    }

    override __.FetchUserInternal() = async {
        let req = new WhoAmIRequest()
        let! u = Swu.executeAsync req
        return {
            username = u.Username
            icon_url = Some u.UserIconUrl.AbsoluteUri
        }
    }