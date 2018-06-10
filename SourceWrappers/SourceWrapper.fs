namespace SourceWrappers

open System

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

type FetchResult<'cursor when 'cursor : struct> = {
    Posts: seq<IPostWrapper>
    Next: 'cursor
}

[<AbstractClass>]
type SourceWrapper<'cursor when 'cursor : struct>() =
    abstract member Name: string with get
    abstract member SubmissionsFiltered: bool with get
    
    abstract member Fetch: 'cursor option -> int -> Async<FetchResult<'cursor>>
    abstract member Whoami: unit -> Async<string>
    abstract member GetUserIcon: int -> Async<string>
    
    member this.StartAsync take = this.Fetch None take |> Async.StartAsTask
    member this.MoreAsync skip take = this.Fetch skip take |> Async.StartAsTask
    member this.WhoamiAsync () = this.Whoami () |> Async.StartAsTask
    member this.GetUserIconAsync size = this.GetUserIcon size |> Async.StartAsTask
