namespace SourceWrappers

open System.Collections.Generic
open System

[<AbstractClass>]
type AbstractCachedSourceWrapper() =
    inherit SourceWrapper<int>()

type CachedSourceWrapper<'a when 'a : struct>(source: IPagedSourceWrapper<'a>) =
    inherit AbstractCachedSourceWrapper()

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
            let! result = match cursor with
                | Some c -> source.MoreAsync c source.SuggestedBatchSize |> Async.AwaitTask
                | None -> source.StartAsync source.SuggestedBatchSize |> Async.AwaitTask

            cache.AddRange(result.Posts)
            cursor <- Some result.Next
            ended <- not result.HasMore

            return! this.Fetch index take
    }

    override this.Whoami = source.WhoamiAsync() |> Async.AwaitTask

    override this.GetUserIcon size = source.GetUserIconAsync size |> Async.AwaitTask

type internal SourceWrapperQueue(wrapper: AbstractCachedSourceWrapper) =
    let mutable cursor: int option = None
    let mutable ended = false

    let buffer = new Queue<IPostWrapper>()
    
    member this.PeekTimestamp() =
        if buffer.Count = 0 then
            DateTime.MinValue
        else
            buffer.Peek().Timestamp

    member this.Dequeue() =
        if buffer.Count = 0 then
            None
        else
            Some (buffer.Dequeue())

    member this.AdvanceIfNeeded() = async {
        if buffer.Count = 0 && not ended then
            let! result = wrapper.Fetch cursor 1
            cursor <- Some result.Next
            ended <- not result.HasMore
            for p in result.Posts do
                buffer.Enqueue p
    }
