namespace SourceWrappers

open System.Collections.Generic

[<AbstractClass>]
type CachedSourceWrapper() =
    inherit SourceWrapper<int>()

type CachedSourceWrapperImpl<'a when 'a : struct>(source: IPagedSourceWrapper<'a>) =
    inherit CachedSourceWrapper()

    let cache = new List<IPostWrapper>()
    let mutable cursor: 'a option = None
    let mutable ended = false
    
    override this.Name = source.Name
    override this.SuggestedBatchSize = 1

    override this.Fetch index take = async {
        let skip = index |> Option.defaultValue 0

        if cache.Count >= skip + take then
            return {
                Posts = cache |> Swu.skipSafe skip |> Seq.truncate take
                Next = skip + take
                HasMore = true
            }
        else if ended then
            let posts = cache |> Swu.skipSafe skip |> Seq.truncate take
            return {
                Posts = posts
                Next = skip + Seq.length posts
                HasMore = false
            }
        else
            let! result =
                match cursor with
                | Some c -> source.MoreAsync c (max take source.SuggestedBatchSize) |> Async.AwaitTask
                | None -> source.StartAsync (max take source.SuggestedBatchSize) |> Async.AwaitTask

            cache.AddRange(result.Posts)
            cursor <- Some result.Next
            ended <- not result.HasMore

            return! this.Fetch index take
    }

    override this.Whoami = source.WhoamiAsync() |> Async.AwaitTask

    override this.GetUserIcon size = source.GetUserIconAsync size |> Async.AwaitTask
