namespace SourceWrappers

open System.Collections.Generic

/// A wrapper around another IPagedSourceWrapper that sorts each batch in descending order by post date. For best results, ask for a single large batch.
type OrderedSourceWrapper<'a when 'a : struct>(source: ISourceWrapper<'a>) =
    inherit SourceWrapper<'a>()

    override this.Name = source.Name
    override this.SuggestedBatchSize = source.SuggestedBatchSize * 10

    override this.Fetch initialCursor take = async {
        let cache = new List<IPostWrapper>()
        let mutable cursor = initialCursor
        
        let mutable hasMore = true
        while hasMore && cache.Count < take do
            let! result =
                match cursor with
                | Some c -> source.MoreAsync c take |> Async.AwaitTask
                | None -> source.StartAsync take |> Async.AwaitTask
            cache.AddRange(result.Posts)
            cursor <- Some result.Next
            hasMore <- result.HasMore

        return {
            Posts = cache |> Seq.sortByDescending (fun w -> w.Timestamp)
            Next = cursor.Value
            HasMore = hasMore
        }
    }

    override this.Whoami = source.WhoamiAsync() |> Async.AwaitTask

    override this.GetUserIcon size = source.GetUserIconAsync size |> Async.AwaitTask
