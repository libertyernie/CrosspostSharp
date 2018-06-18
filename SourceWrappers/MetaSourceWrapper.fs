namespace SourceWrappers

open WrapperUtility
open System.Collections.Generic

type MetaSourceWrapper(name: string, sources: seq<AbstractCachedSourceWrapper>) =
    inherit SourceWrapper<int>()

    let queues = sources |> Seq.map SourceWrapperQueue |> Seq.toList
    let cache = new List<IPostWrapper>()
    let mutable ended = false

    let addNext = async {
        do!
            queues
            |> Seq.map (fun s -> s.AdvanceIfNeeded())
            |> Async.Parallel
            |> whenDone ignore
        let first =
            queues
            |> Seq.sortByDescending (fun queue -> queue.PeekTimestamp())
            |> Seq.head
        match first.Dequeue() with
        | Some s -> cache.Add(s)
        | None -> ended <- true
    }
    
    override this.Name = name
    override this.SuggestedBatchSize = 1
    
    override this.Fetch index take = async {
        let skip = index |> Option.defaultValue 0
        while cache.Count < skip + take && not ended do
            do! addNext

        return {
            Posts = cache |> skipSafe skip |> Seq.truncate take
            Next = skip + take
            HasMore = not ended
        }
    }

    override this.Whoami = sources |> Seq.map (fun w -> w.Whoami) |> Seq.head

    override this.GetUserIcon size = sources |> Seq.map (fun w -> w.GetUserIcon size) |> Seq.head