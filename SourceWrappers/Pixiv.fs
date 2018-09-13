namespace SourceWrappers

open Pixeez.Objects
open Pixeez

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
    inherit SourceWrapper<int>()

    let mutable login_info: (Tokens * User) option = None

    let login = async {
        if Option.isNone login_info then
            let! authResult = Auth.AuthorizeAsync(username, password, null, null) |> Async.AwaitTask
            login_info <- Some (authResult.Tokens, authResult.Authorize.User)
    }

    override this.Name = "Pixiv"
    override this.SuggestedBatchSize = 1

    override this.Fetch cursor take = async {
        do! login
        let (tokens, user) = login_info.Value
        let! result = tokens.GetUserWorksAsync(user.Id.Value, offset = (cursor |> Option.toNullable)) |> Async.AwaitTask
        return {
            Posts = result.illusts |> Seq.map PixivPostWrapper |> Seq.cast
            Next = result.illusts.Length + (cursor |> Option.defaultValue 0)
            HasMore = not (isNull result.next_url)
        }
    }

    override this.Whoami = async {
        do! login
        let (tokens, user) = login_info.Value
        return user.Name
    }

    override this.GetUserIcon size = async {
        do! login
        let (tokens, user) = login_info.Value
        return user.GetAvatarUrl()
    }