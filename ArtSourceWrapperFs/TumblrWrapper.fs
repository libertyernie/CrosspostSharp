namespace ArtSourceWrapperFs

open DontPanic.TumblrSharp.Client
open ArtSourceWrapper
open DontPanic.TumblrSharp
open System.Threading.Tasks

type public TumblrWrapper(client: TumblrClient, blogName: string, photosOnly: bool) =
    inherit SiteWrapper<ISubmissionWrapper, int64>()

    let mutable blogNames: seq<string> = null

    override this.WrapperName =
        if photosOnly then
            "Tumblr (photos)"
        else
            "Tumblr (text + photos)"
    
    override this.SubmissionsFiltered = true

    override val BatchSize = 20 with get, set
    override this.MinBatchSize = 1
    override this.MaxBatchSize = 20
    
    override this.InternalFetchAsync(startPosition, maxCount) =
        Async.StartAsTask (async {
            if isNull blogNames then
                let! user = client.GetUserInfoAsync() |> Async.AwaitTask
                blogNames <- user.Blogs |> Seq.map (fun b -> b.Name)

            let contains x = Seq.exists ((=) x)
            if not (blogNames |> contains blogName) then
                invalidOp "Yor account does not have access to the given blog name."
            
            let position =
                if startPosition.HasValue then startPosition.Value
                else int64 0
            let count = maxCount |> max this.MinBatchSize |> min this.MaxBatchSize

            let t = if photosOnly then PostType.Photo else PostType.All

            let! posts =
                Async.AwaitTask <| client.GetPostsAsync(
                    blogName,
                    position,
                    count,
                    t,
                    true)
            
            if posts.Result.Length = 0 then
                return InternalFetchResult<ISubmissionWrapper, int64>(position, true)
            else
                let newPos = position + posts.Result.LongLength
                let wrapped = seq {
                    for post in posts.Result do
                        let postBlogName =
                            if not (isNull post.RebloggedRootName)
                                then post.RebloggedRootName
                                else post.BlogName
                        if blogNames |> contains postBlogName then
                            match post with
                            | :? PhotoPost as photo -> yield TumblrPhotoPostSubmissionWrapper(client, photo) :> ISubmissionWrapper
                            | :? TextPost as text -> yield TumblrTextPostSubmissionWrapper(client, text) :> ISubmissionWrapper
                            | _ -> ()
                }
                return InternalFetchResult<ISubmissionWrapper, int64>(wrapped, newPos, false)
        })

    override this.WhoamiAsync() = Task.FromResult(blogName)
    override this.GetUserIconAsync(size) = Task.FromResult(sprintf "https://api.tumblr.com/v2/blog/%s.tumblr.com/avatar/%d" blogName size)
