namespace SourceWrappers

open System.Collections.Generic
open System

/// A wrapper around another IPagedSourceWrapper that sorts each page in descending order by post date. The default page size is 10 times the default page size of the source.
type OrderedSourceWrapper<'a when 'a : struct>(source: SourceWrapper<'a>) =
    inherit SourceWrapper<'a>()

    override this.Name = source.Name
    override this.SuggestedBatchSize = source.SuggestedBatchSize * 10

    override this.Fetch initialCursor take = async {
        let cache = new List<IPostWrapper>()
        let mutable cursor = initialCursor
        
        let mutable hasMore = true
        while hasMore && cache.Count < take do
            let! result = source.Fetch cursor take
            cache.AddRange(result.Posts)
            cursor <- Some result.Next
            hasMore <- result.HasMore

        return {
            Posts = cache |> Seq.sortByDescending (fun w -> w.Timestamp)
            Next = cursor.Value
            HasMore = hasMore
        }
    }

    override this.Whoami = source.Whoami

    override this.GetUserIcon size = source.GetUserIcon size
