namespace SourceWrappers

open System
open System.Threading.Tasks
open System.Security.Cryptography

type IPostWrapper =
    abstract member Title: string with get
    abstract member HTMLDescription: string with get
    abstract member Mature: bool with get
    abstract member Adult: bool with get
    abstract member Tags: seq<string> with get
    abstract member Timestamp: DateTime with get
    abstract member ViewURL: string with get
    abstract member ImageURL: string with get
    abstract member ThumbnailURL: string with get

type IStatusUpdate =
    abstract member PotentiallySensitive: bool with get
    abstract member FullHTML: string with get
    abstract member HasPhoto: bool with get
    abstract member AdditionalLinks: seq<string> with get

type IDeletable =
    abstract member DeleteAsync: unit -> Task
    abstract member SiteName: string with get

type IFetchResult =
    abstract member Posts: seq<IPostWrapper>
    abstract member HasMore: bool

type PagedFetchResult<'cursor when 'cursor : struct> =
    {
        Posts: seq<IPostWrapper>
        Next: 'cursor
        HasMore: bool
    }
    interface IFetchResult with
        member this.Posts = this.Posts
        member this.HasMore = this.HasMore

type ISourceWrapper =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int with get
    abstract member FetchAllAsync: int -> Task<seq<IPostWrapper>>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>

type IPagedSourceWrapper<'cursor when 'cursor : struct> =
    inherit ISourceWrapper
    abstract member StartAsync: int -> Task<PagedFetchResult<'cursor>>
    abstract member MoreAsync: 'cursor -> int -> Task<PagedFetchResult<'cursor>>

[<AbstractClass>]
type SourceWrapper<'cursor when 'cursor : struct>() =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int
    
    abstract member Fetch: 'cursor option -> int -> Async<PagedFetchResult<'cursor>>
    abstract member Whoami: Async<string>
    abstract member GetUserIcon: int -> Async<string>

    member this.AsISourceWrapper () = this :> IPagedSourceWrapper<'cursor>

    member private this.FetchAll (initial: seq<IPostWrapper>) (cursor: 'cursor option) (limit: int) = async {
        let! result = this.Fetch cursor limit
        if not result.HasMore || Seq.length result.Posts >= limit then
            return result.Posts |> Seq.truncate limit |> Seq.append initial
        else
            return! this.FetchAll (Seq.append initial result.Posts) (Some result.Next) (limit - Seq.length result.Posts)
    }
    
    interface IPagedSourceWrapper<'cursor> with
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

    let toPostWrapperInterface w = w :> IPostWrapper
