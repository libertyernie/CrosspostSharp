namespace SourceWrappers

open System
open System.Text
open System.Threading.Tasks
open DeviantArtFs
open FSharp.Control

type StashPostWrapper(entry: DeviantArtFs.Stash.DeltaResponse.Entry) =
    let imageUrl =
        entry.Metadata.Files
        |> Seq.sortByDescending (fun f -> f.Width)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.defaultValue null

    let delete = async {
        return raise (new NotImplementedException())
        //let req = new DeviantartApi.Requests.Stash.DeleteRequest(entry.ItemId)
        //do! Swu.executeAsync req |> Swu.whenDone ignore
    }

    member __.ItemId = entry.Itemid

    interface IRemotePhotoPost with
        member this.Title = entry.Metadata.Title
        member this.HTMLDescription = entry.Metadata.ArtistComments |> Option.defaultValue ""
        member this.Mature = false
        member this.Adult = false
        member this.Tags = entry.Metadata.Tags :> seq<string>
        member this.Timestamp = entry.Metadata.CreationTime |> Option.map Swu.fromUnixTime |> Option.defaultValue DateTime.MinValue
        member this.ViewURL =
            let url = new StringBuilder()
            let mutable itemId = entry.Itemid |> Option.defaultValue 0L
            while itemId > int64 0 do
                let n = itemId % int64 36
                let c = "0123456789abcdefghijklmnopqrstuvwxyz".[int32 n]
                url.Insert(0, c) |> ignore
                itemId <- itemId / int64 36
            url.Insert(0, "https://sta.sh/0") |> ignore
            url.ToString()
        member this.ImageURL = imageUrl
        member this.ThumbnailURL =
            entry.Metadata.Thumb
            |> Option.map (fun t -> t.Src)
            |> Option.defaultValue imageUrl

    interface IDeletable with
        member this.SiteName = "Sta.sh"
        member this.DeleteAsync() = delete |> Async.StartAsTask :> Task

type UnorderedStashSourceWrapper(token: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    override __.Name = "Sta.sh"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = 0
        let mutable more = true
        
        while more do
            let! result =
                new DeviantArtFs.Stash.DeltaRequest(Limit = 120, Offset = cursor)
                |> DeviantArtFs.Stash.Delta.AsyncExecute token

            let wrappers =
                result.Entries
                |> Seq.filter (fun e -> (not << Option.isNone) e.Itemid)
                |> Seq.filter (fun e -> (not << isNull) e.Metadata.Files)
                |> Seq.map StashPostWrapper
            for o in wrappers do
                yield o :> IPostBase

            cursor <- result.NextOffset
                |> Option.defaultValue 0
            more <- result.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.User.Whoami.AsyncExecute token
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }