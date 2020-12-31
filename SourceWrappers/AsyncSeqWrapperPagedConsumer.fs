namespace SourceWrappers

open ArtworkSourceSpecification
open System.Threading.Tasks
open FSharp.Control

/// Consumes IPagedSourceWrapper, one page at a time. Some wrappers might not support a constant page size and need to be wrapped in a CachedSourceWrapper first.
type IPagedWrapperConsumer =
    abstract member Name: string with get
    abstract member HasMoreAsync: unit -> Task<bool>
    abstract member SuggestedBatchSize: int with get
    abstract member NextAsync: unit -> Task<seq<IPostBase>>
    abstract member PrevAsync: unit -> Task<seq<IPostBase>>
    abstract member FirstAsync: unit -> Task<seq<IPostBase>>
    abstract member FetchAllAsync: int -> Task<seq<IPostBase>>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>

type AsyncSeqWrapperPagedConsumer(wrapper: AsyncSeqWrapper, page_size: int) =
    let toSeqAsync a =
        AsyncSeq.toListAsync a
        |> Swu.whenDone Seq.ofList

    let anyAsync a = async {
        let! first = AsyncSeq.tryFirst a
        return Option.isSome first
    }

    let mutable skip = 0
    let current() =
        wrapper.GetSubmissions()
        |> AsyncSeq.skip skip
        |> AsyncSeq.take page_size
        |> toSeqAsync
        |> Async.StartAsTask

    member __.Name = wrapper.Name

    interface IPagedWrapperConsumer with
        member __.Name = wrapper.Name
        member __.HasMoreAsync() =
            wrapper.GetSubmissions()
            |> AsyncSeq.skip (skip + page_size)
            |> anyAsync
            |> Async.StartAsTask
        member this.SuggestedBatchSize = 1
        member this.NextAsync() =
            skip <- skip + page_size
            current()
        member this.PrevAsync() =
            skip <- skip - page_size |> max 0
            current()
        member __.FirstAsync() =
            skip <- 0
            current()
        member __.FetchAllAsync limit =
            wrapper.GetSubmissions()
            |> AsyncSeq.take limit
            |> toSeqAsync
            |> Async.StartAsTask
        member this.WhoamiAsync() =
            wrapper.WhoamiAsync()
        member this.GetUserIconAsync _ =
            wrapper.GetUserIconAsync()