namespace SourceWrappers

open DontPanic.TumblrSharp.Client
open DontPanic.TumblrSharp
open FSharp.Control

[<AbstractClass>]
type TumblrPostWrapper<'T when 'T :> BasePost>(client: TumblrClient, post: BasePost) =
    abstract member HTMLDescription: string with get

    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription = this.HTMLDescription
        member this.Mature = false
        member this.Adult = false
        member this.Tags = post.Tags :> seq<string>
        member this.Timestamp = post.Timestamp
        member this.ViewURL = post.Url

    interface IDeletable with
        member this.SiteName = "Tumblr"
        member this.DeleteAsync () = client.DeletePostAsync(post.BlogName, post.Id)

type TumblrPhotoPostWrapper(client: TumblrClient, post: PhotoPost) =
    inherit TumblrPostWrapper<PhotoPost>(client, post)
    override __.HTMLDescription = post.Caption

    interface IRemotePhotoPost with
        member __.ImageURL = post.Photo.OriginalSize.ImageUrl
        member __.ThumbnailURL =
            post.Photo.AlternateSizes
            |> Seq.sortBy (fun s -> s.Width)
            |> Seq.filter (fun s -> s.Width >= 120 && s.Height >= 120)
            |> Seq.map (fun s -> s.ImageUrl)
            |> Seq.append (Seq.singleton post.Photo.OriginalSize.ImageUrl)
            |> Seq.head

type TumblrTextPostWrapper(client: TumblrClient, post: TextPost) =
    inherit TumblrPostWrapper<TextPost>(client, post)
    override __.HTMLDescription = post.Body

type TumblrSourceWrapper(client: TumblrClient, blogName: string, photosOnly: bool) =
    inherit AsyncSeqWrapper()

    let blogNamesTask = lazy(
        let a = async {
            let! user = client.GetUserInfoAsync() |> Async.AwaitTask
            return user.Blogs |> Seq.map (fun b -> b.Name)
        }
        Async.StartAsTask a
    )

    override __.Name =
        if photosOnly then
            "Tumblr (images)"
        else
            "Tumblr (text + images)"
    
    override __.FetchSubmissionsInternal() = asyncSeq {
        let! blogNames = Async.AwaitTask (blogNamesTask.Force())
        let t = if photosOnly then PostType.Photo else PostType.All

        let mutable skip = 0L
        let mutable more = true
        while more do
            let! posts =
                client.GetPostsAsync(
                    blogName,
                    skip,
                    20,
                    t,
                    true) |> Async.AwaitTask

            for post in posts.Result do
                let postBlogName =
                    if not (isNull post.RebloggedRootName)
                        then post.RebloggedRootName
                        else post.BlogName
                if blogNames |> Seq.contains postBlogName then
                    match post with
                    | :? PhotoPost as photo -> yield TumblrPhotoPostWrapper(client, photo) :> IPostBase
                    | :? TextPost as text -> yield TumblrTextPostWrapper(client, text) :> IPostBase
                    | _ -> ()
            skip <- skip + posts.Result.LongLength
            more <- posts.Result.LongLength > 0L
    }

    override __.FetchUserInternal() = async {
        let blogHostname =
            if blogName.Contains(".") then
                sprintf "%s.tumblr.com" blogName
            else
                blogName

        return {
            username = blogName
            icon_url = Some (sprintf "https://api.tumblr.com/v2/blog/%s/avatar/64" blogHostname)
        }
    }