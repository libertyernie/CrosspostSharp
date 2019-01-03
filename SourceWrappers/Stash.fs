namespace SourceWrappers

open System
open System.Text
open DeviantArtFs
open FSharp.Control

type StashPostWrapper(itemId: int64, metadata: DeviantArtFs.Requests.Stash.StashMetadata.Root, token: IDeviantArtAccessToken) =
    let imageUrl =
        metadata.Files
        |> Seq.sortByDescending (fun f -> f.Width)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    member __.ItemId = itemId

    interface IRemotePhotoPost with
        member this.Title = metadata.Title |> Option.defaultValue ""
        member this.HTMLDescription = metadata.ArtistComments |> Option.defaultValue ""
        member this.Mature = false
        member this.Adult = false
        member this.Tags = metadata.Tags :> seq<string>
        member this.Timestamp = metadata.CreationTime |> Option.map Swu.fromUnixTime |> Option.defaultValue DateTime.MinValue
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
            metadata.Thumb
            |> Option.map (fun t -> t.Src)
            |> Option.defaultValue imageUrl

    interface IDeletable with
        member this.SiteName = "Sta.sh"
        member this.DeleteAsync() = DeviantArtFs.Requests.Stash.Delete.ExecuteAsync token itemId

type UnorderedStashSourceWrapper(token: IDeviantArtAccessToken) =
    inherit AsyncSeqWrapper()

    override __.Name = "Sta.sh"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = 0
        let mutable more = true
        
        while more do
            let! result =
                new DeviantArtFs.Requests.Stash.DeltaRequest(Limit = 120, Offset = cursor)
                |> DeviantArtFs.Requests.Stash.Delta.AsyncExecute token

            for o in result.Entries do
                match (o.Itemid, o.Metadata) with
                | (Some itemid, Some metadata) -> yield new StashPostWrapper(itemid, metadata, token) :> IPostBase
                | _ -> ()

            cursor <- result.NextOffset.GetValueOrDefault()
            more <- result.HasMore
    }

    override __.FetchUserInternal() = async {
        let! u = DeviantArtFs.Requests.User.Whoami.AsyncExecute token
        return {
            username = u.Username
            icon_url = Some u.Usericon
        }
    }