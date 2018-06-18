namespace SourceWrappers

open System.Threading.Tasks

type IPagedWrapperConsumer =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int with get
    abstract member NextAsync: Task<GenericFetchResult>
    abstract member PrevAsync: Task<GenericFetchResult>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>

type internal PagedWrapperCursor<'a> = {
    cursor: 'a
    last: PagedWrapperCursor<'a> option
}

type PagedWrapperConsumer<'a when 'a : struct>(wrapper: ISourceWrapper<'a>, page_size: int) =
    let mutable next_cursor: PagedWrapperCursor<'a> option = None

    let back cursor =
        match cursor with
        | Some c -> c.last
        | None -> None

    let next = async {
        let! r =
            match next_cursor with
            | Some c -> wrapper.MoreAsync c.cursor page_size |> Async.AwaitTask
            | None -> wrapper.StartAsync page_size |> Async.AwaitTask

        let n = {
            cursor = r.Next
            last = next_cursor
        }
        next_cursor <- Some n

        return r :> GenericFetchResult
    }

    let prev = async {
        let prev_cursor = next_cursor |> back |> back
        next_cursor <- prev_cursor
        return! next
    }

    interface IPagedWrapperConsumer with
        member this.Name = wrapper.Name
        member this.SuggestedBatchSize = wrapper.SuggestedBatchSize
        member this.NextAsync = next |> Async.StartAsTask
        member this.PrevAsync = prev |> Async.StartAsTask
        member this.WhoamiAsync() = wrapper.WhoamiAsync()
        member this.GetUserIconAsync size = wrapper.GetUserIconAsync size