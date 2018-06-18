namespace SourceWrappers

open System
open System.Threading.Tasks

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

type FetchResult<'cursor when 'cursor : struct> = {
    Posts: seq<IPostWrapper>
    Next: 'cursor
    HasMore: bool
}

type ISourceWrapper<'cursor when 'cursor : struct> =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int with get
    abstract member StartAsync: int -> Task<FetchResult<'cursor>>
    abstract member MoreAsync: 'cursor -> int -> Task<FetchResult<'cursor>>
    abstract member WhoamiAsync: unit -> Task<string>
    abstract member GetUserIconAsync: int -> Task<string>

[<AbstractClass>]
type SourceWrapper<'cursor when 'cursor : struct>() =
    abstract member Name: string with get
    abstract member SuggestedBatchSize: int
    
    abstract member Fetch: 'cursor option -> int -> Async<FetchResult<'cursor>>
    abstract member Whoami: Async<string>
    abstract member GetUserIcon: int -> Async<string>

    member this.AsISourceWrapper () = this :> ISourceWrapper<'cursor>
    
    interface ISourceWrapper<'cursor> with
        member this.Name = this.Name
        member this.SuggestedBatchSize = this.SuggestedBatchSize
        member this.StartAsync take = this.Fetch None take |> Async.StartAsTask
        member this.MoreAsync cursor take = this.Fetch (Some cursor) take |> Async.StartAsTask
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
