namespace SourceWrappers

open DeviantartApi.Objects
open System
open DeviantartApi.Requests.User

type DeviantArtStatusPostWrapper(status: Status, deviation: Deviation option) =
    let Mature =
        deviation
        |> Option.map (fun d -> Option.ofNullable d.IsMature)
        |> Option.flatten
        |> Option.defaultValue false

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = status.Body
        member this.Mature = Mature
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = status.TimeStamp
        member this.ViewURL = status.Url.AbsoluteUri

type DeviantArtStatusPhotoPostWrapper(status: Status, deviation: Deviation) =
    inherit DeviantArtStatusPostWrapper(status, Some deviation)
    let Icon = status.Author.UserIconUrl.AbsoluteUri

    interface IRemotePhotoPost with
        member __.ImageURL =
            deviation.Content
            |> Option.ofObj
            |> Option.map (fun d -> d.Src)
            |> Option.defaultValue Icon
        member __.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun t -> t.Src)
            |> Seq.tryHead
            |> Option.defaultValue Icon

type DeviantArtStatusSourceWrapper() =
    inherit SourceWrapper<int>()

    let mutable cached_user: User = null

    let getUser = async {
        if isNull cached_user then
            let req = new WhoAmIRequest()
            let! u =
                req.ExecuteAsync()
                |> Async.AwaitTask
                |> Swu.whenDone Swu.processDeviantArtError
            cached_user <- u
        return cached_user
    }
    
    override this.Name = "DeviantArt (statuses)"
    override this.SuggestedBatchSize = 10

    override this.Fetch cursor take = async {
        let position = cursor |> Option.defaultValue 0

        let! username = this.Whoami

        let statusesRequest = new StatusesRequest(username)
        statusesRequest.Limit <- take |> min 50 |> uint32 |> Nullable
        statusesRequest.Offset <- position |> uint32 |> Nullable

        let! statuses =
            statusesRequest.ExecuteAsync()
            |> Async.AwaitTask
            |> Swu.whenDone Swu.processDeviantArtError
        
        return {
            Posts = seq {
                for r in statuses.Results do
                    let items = r.Items |> Seq.map (fun i -> i.Deviation) |> Seq.filter (not << isNull)
                    if Seq.isEmpty items then
                        yield new DeviantArtStatusPostWrapper(r, None)
                    else
                        for d in items do
                            yield new DeviantArtStatusPostWrapper(r, Some d)
            } |> Seq.map Swu.potBase
            Next = statuses.NextOffset |> Option.ofNullable |> Option.defaultValue 0
            HasMore = statuses.HasMore
        }
    }

    override this.Whoami = getUser |> Swu.whenDone (fun u -> u.Username)

    override this.GetUserIcon size = getUser |> Swu.whenDone (fun u -> u.UserIconUrl.AbsoluteUri)