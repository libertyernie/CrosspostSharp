namespace SourceWrappers

open System
open System.Threading.Tasks

/// A result returned from an IPagedSourceWrapper. Use the Next cursor to fetch the next page of results.
type FetchResult<'cursor when 'cursor : struct> = {
    Posts: seq<IPostBase>
    Next: 'cursor
    HasMore: bool
}

/// A wrapper to get information from an art or social media site.
type ISourceWrapper<'cursor when 'cursor : struct> =
    abstract member Name: string with get
    abstract member FetchAllAsync: int -> Task<seq<IPostBase>>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>
    abstract member SuggestedBatchSize: int with get
    abstract member StartAsync: int -> Task<FetchResult<'cursor>>
    abstract member MoreAsync: 'cursor -> int -> Task<FetchResult<'cursor>>

/// An abstract class defined in F# that implements StartAsync, MoreAsync, and FetchAllAsync through one Fetch function. Wrappers in other languages (such as C# or VB.NET) should probably implement IPagedSourceWrapper instead.
[<AbstractClass>]
type SourceWrapper<'cursor when 'cursor : struct>() =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int
    
    abstract member Fetch: 'cursor option -> int -> Async<FetchResult<'cursor>>
    abstract member Whoami: Async<string>
    abstract member GetUserIcon: int -> Async<string>

    member this.AsISourceWrapper () = this :> ISourceWrapper<'cursor>

    member private this.FetchAll (initial: seq<IPostBase>) (cursor: 'cursor option) (limit: int) = async {
        let! result = this.Fetch cursor limit
        if not result.HasMore || Seq.length result.Posts >= limit then
            return result.Posts |> Seq.truncate limit |> Seq.append initial
        else
            return! this.FetchAll (Seq.append initial result.Posts) (Some result.Next) (limit - Seq.length result.Posts)
    }
    
    interface ISourceWrapper<'cursor> with
        member this.Name = this.Name
        member this.SuggestedBatchSize = this.SuggestedBatchSize
        member this.StartAsync take = this.Fetch None take |> Async.StartAsTask
        member this.MoreAsync cursor take = this.Fetch (Some cursor) take |> Async.StartAsTask
        member this.FetchAllAsync limit = this.FetchAll Seq.empty None limit |> Async.StartAsTask
        member this.WhoamiAsync () = this.Whoami |> Async.StartAsTask
        member this.GetUserIconAsync size = this.GetUserIcon size |> Async.StartAsTask

module internal Swu =
    open DeviantartApi.Objects

    let tryMapSingle x f =
        if isNull x then None else Some (f x)

    let skipSafe num = 
        Seq.zip (Seq.initInfinite id)
        >> Seq.skipWhile (fun (i, _) -> i < num)
        >> Seq.map snd

    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

    let processDeviantArtError<'a when 'a :> BaseObject> (resp: DeviantartApi.Requests.Response<'a>) =
        if (resp.IsError) then failwith resp.ErrorText
        if (resp.Result.Error |> String.IsNullOrEmpty |> not) then failwith resp.Result.Error
        resp.Result

    let potBase w = w :> IPostBase
