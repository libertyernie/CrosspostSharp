namespace SourceWrappers

open DontPanic.TumblrSharp.Client
open DontPanic.TumblrSharp

[<AbstractClass>]
type TumblrPostWrapper<'T when 'T :> BasePost>(client: TumblrClient, post: BasePost) =
    let getImageUrl size =
        let blogHostname =
            if post.BlogName.Contains(".") then
                sprintf "%s.tumblr.com" post.BlogName
            else
                post.BlogName
        sprintf "https://api.tumblr.com/v2/blog/%s/avatar/%d" blogHostname size

    abstract member HTMLDescription: string with get
    abstract member ImageURL: string with get
    abstract member ThumbnailURL: string with get

    default this.ImageURL = getImageUrl 512
    default this.ThumbnailURL = getImageUrl 128

    interface IPostWrapper with
        member this.Title = ""
        member this.HTMLDescription = this.HTMLDescription
        member this.Mature = false
        member this.Adult = false
        member this.Tags = post.Tags |> Seq.map id
        member this.Timestamp = post.Timestamp
        member this.ViewURL = post.Url
        member this.ImageURL = this.ImageURL
        member this.ThumbnailURL = this.ThumbnailURL

type TumblrPhotoPostWrapper(client: TumblrClient, post: PhotoPost) =
    inherit TumblrPostWrapper<PhotoPost>(client, post)

    override this.HTMLDescription = post.Caption
    override this.ImageURL = post.Photo.OriginalSize.ImageUrl
    override this.ThumbnailURL =
        post.Photo.AlternateSizes
        |> Seq.sortBy (fun s -> s.Width)
        |> Seq.filter (fun s -> s.Width >= 120 && s.Height >= 120)
        |> Seq.map (fun s -> s.ImageUrl)
        |> Seq.append (Seq.singleton this.ImageURL)
        |> Seq.head

type TumblrTextPostWrapper(client: TumblrClient, post: TextPost) =
    inherit TumblrPostWrapper<TextPost>(client, post)

    override this.HTMLDescription = post.Body

type TumblrWrapper2(client: TumblrClient, blogName: string, photosOnly: bool) =
    inherit SourceWrapper()

    let mutable blogNames: seq<string> = null

    override this.Name =
        if photosOnly then
            "Tumblr (photos) (F#)"
        else
            "Tumblr (text + photos) (F#)"

    override this.SubmissionsFiltered = false

    override this.Fetch skip take = async {
        if isNull blogNames then
            let! user = client.GetUserInfoAsync() |> Async.AwaitTask
            blogNames <- user.Blogs |> Seq.map (fun b -> b.Name)

        let t = if photosOnly then PostType.Photo else PostType.All

        let! posts =
            Async.AwaitTask <| client.GetPostsAsync(
                blogName,
                int64 skip,
                take,
                t,
                true)
                
        return seq {
            for post in posts.Result do
                let postBlogName =
                    if not (isNull post.RebloggedRootName)
                        then post.RebloggedRootName
                        else post.BlogName
                if blogNames |> Seq.contains postBlogName then
                    match post with
                    | :? PhotoPost as photo -> yield TumblrPhotoPostWrapper(client, photo) :> IPostWrapper
                    | :? TextPost as text -> yield TumblrTextPostWrapper(client, text) :> IPostWrapper
                    | _ -> ()
        }
    }

    override this.Whoami () = async {
        return "user"
    }

    override this.GetUserIcon size = async {
        return null;
    }