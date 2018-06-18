namespace SourceWrappers

open DeviantartApi.Objects
open System
open DeviantartApi.Requests.User
open DeviantartApi.Requests.Gallery
open DeviantartApi.Requests.Deviation
open DeviantartApi.Objects.SubObjects.StashDelta
open System.Text
open System.Threading.Tasks
open DeviantartApi.Requests.Stash

type StashPostWrapper(entry: Entry) =
    let imageUrl =
        entry.Metadata.Files
        |> Seq.sortByDescending (fun f -> f.Width)
        |> Seq.map (fun f -> f.Src)
        |> Seq.tryHead
        |> Option.defaultValue null

    let delete = async {
        let req = new DeviantartApi.Requests.Stash.DeleteRequest(entry.ItemId)
        do! req.ExecuteAsync()
            |> Async.AwaitTask
            |> Swu.whenDone Swu.processDeviantArtError
            |> Swu.whenDone ignore
    }

    interface IPostWrapper with
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

type StashSourceWrapper() =
    inherit SourceWrapper<uint32>()

    let deviantArtWrapper = new DeviantArtSourceWrapper()
    
    override this.Name = "Sta.sh"
    override this.SuggestedBatchSize = 120

    override this.Fetch cursor take = async {
        let position = cursor |> Option.defaultValue (uint32 0)

        let deltaRequest = new DeltaRequest()
        deltaRequest.Limit <- take |> min 120 |> uint32 |> Nullable
        deltaRequest.Offset <- position |> Nullable

        let! result =
            deltaRequest.ExecuteAsync()
            |> Async.AwaitTask
            |> Swu.whenDone Swu.processDeviantArtError
        
        let wrappers =
            result.Entries
            |> Seq.filter (fun e -> e.ItemId <> int64 0)
            |> Seq.filter (fun e -> (not << isNull) e.Metadata.Files)
            |> Seq.map StashPostWrapper
            |> Seq.map Swu.toPostWrapperInterface

        return {
            Posts = wrappers
            Next = result.NextOffset |> Option.ofNullable |> Option.defaultValue 0 |> uint32
            HasMore = result.HasMore
        }
    }

    override this.Whoami = deviantArtWrapper.Whoami

    override this.GetUserIcon size = deviantArtWrapper.GetUserIcon size