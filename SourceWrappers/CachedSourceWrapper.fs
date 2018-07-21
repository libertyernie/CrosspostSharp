namespace SourceWrappers

open System.Collections.Generic

/// A wrapper around another IPagedSourceWrapper that caches results and uses an integer index as the cursor.
[<AbstractClass>]
type CachedSourceWrapper() =
    inherit SourceWrapper<int>()

/// A wrapper around another IPagedSourceWrapper that caches results and uses an integer index as the cursor.
type CachedSourceWrapperImpl<'a when 'a : struct>(source: ISourceWrapper<'a>) =
    inherit CachedSourceWrapper()

    let cache = new List<IPostWrapper>()
    let mutable cursor: 'a option = None
    let mutable ended = false
    
    override this.Name = source.Name
    override this.SuggestedBatchSize = 1

    override this.Fetch index take = async {
        let skip = index |> Option.defaultValue 0

        let found = cache |> Swu.skipSafe skip |> Seq.truncate take
        let at_end = ended && cache |> Swu.skipSafe (skip + take) |> Seq.isEmpty

        if cache.Count >= skip + take then
            return {
                Posts = found
                Next = skip + take
                HasMore = not at_end
            }
        else if ended then
            return {
                Posts = found
                Next = skip + Seq.length found
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
