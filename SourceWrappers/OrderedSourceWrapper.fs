namespace SourceWrappers

open System.Collections.Generic
open System

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
