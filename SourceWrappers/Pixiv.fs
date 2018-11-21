namespace SourceWrappers

open Pixeez.Objects
open Pixeez
open FSharp.Control
open System

type PixivPostWrapper(work: IllustWork) =
    let thumbnails =
        if isNull work.ImageUrls
        then []
        else 
            [work.ImageUrls.Original;
            work.ImageUrls.Large;
            work.ImageUrls.Medium;
            work.ImageUrls.Small] |> List.filter (fun x -> not (isNull x))

    let thumbnailUrl = List.tryLast thumbnails |> Option.defaultValue null

    let imageUrl =
        work.meta_single_page |> Option.ofObj |> Option.map (fun x -> x.OriginalImageUrl)
        |> Option.orElse (List.tryHead thumbnails)
        |> Option.defaultValue thumbnailUrl

    interface IRemotePhotoPost with
        member this.Title = work.Title
        member this.HTMLDescription = work.Caption
        member this.Mature = false
        member this.Adult = work.AgeLimit <> "all-age"
        member this.Tags = work.Tags :> seq<string>
        member this.Timestamp = work.CreatedTime
        member this.ViewURL = sprintf "https://www.pixiv.net/member_illust.php?mode=medium&illust_id=%d" work.Id.Value
        member this.ImageURL = imageUrl
        member this.ThumbnailURL = thumbnailUrl

type PixivSourceWrapper(username: string, password: string) =
    inherit AsyncSeqWrapper()

    let login_task = lazy(Auth.AuthorizeAsync(username, password, null, null))

    override __.Name = "Pixiv"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let! login_info = login_task.Force() |> Async.AwaitTask

        let mutable offset = 0
        let mutable more = true
        while more do
            let! result = login_info.Tokens.GetUserWorksAsync(login_info.Authorize.User.Id.Value, offset = (Nullable offset)) |> Async.AwaitTask
            for i in result.illusts do
                yield new PixivPostWrapper(i) :> IPostBase
            offset <- result.illusts.Length + offset
            more <- not (isNull result.next_url)
    }

    override __.FetchUserInternal() = async {
        let! login_info = login_task.Force() |> Async.AwaitTask
        return {
            username = login_info.Authorize.User.Name
            icon_url = login_info.Authorize.User.GetAvatarUrl() |> Option.ofObj
        }
    }
