namespace CrosspostSharp2.Compat

open SourceWrappers
open System.Threading.Tasks
open FSharp.Control

type FetchResult<'cursor when 'cursor : struct> = {
    Posts: seq<IPostBase>
    Next: 'cursor
    HasMore: bool
}

type ISourceWrapper<'cursor when 'cursor : struct> =
    abstract member Name: string with get
    abstract member FetchAllAsync: int -> Task<seq<IPostBase>>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>
    abstract member SuggestedBatchSize: int with get
    abstract member StartAsync: int -> Task<FetchResult<'cursor>>
    abstract member MoreAsync: 'cursor -> int -> Task<FetchResult<'cursor>>

type SourceWrapper(w: AsyncSeqWrapper) =
    let fetch cursor take = async {
        let! arrayPlusOne =
            w.GetSubmissions()
            |> AsyncSeq.skip cursor
            |> AsyncSeq.take (take + 1)
            |> AsyncSeq.toArrayAsync
        let s = arrayPlusOne |> Seq.truncate take
        let len = Seq.length s
        return {
            Posts = s
            Next = cursor + len
            HasMore = Seq.length arrayPlusOne > len
        }
    }

    let fetchAll limit = async {
        let! o =
            w.GetSubmissions()
            |> AsyncSeq.take limit
            |> AsyncSeq.toArrayAsync
        return o |> Seq.ofArray
    }

    member this.AsISourceWrapper () = this :> ISourceWrapper<int>

    interface ISourceWrapper<int> with
        member this.Name = w.Name
        member this.SuggestedBatchSize = 4
        member this.StartAsync take = fetch 0 take |> Async.StartAsTask
        member this.MoreAsync cursor take = fetch cursor take |> Async.StartAsTask
        member this.FetchAllAsync limit = fetchAll limit |> Async.StartAsTask
        member this.WhoamiAsync() = w.AsyncWhoami() |> Async.StartAsTask
        member this.GetUserIconAsync _ = w.AsyncGetUserIcon() |> Async.StartAsTask