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

[<AbstractClass>]
type SourceWrapper() =
    abstract member Name: string with get
    abstract member SubmissionsFiltered: bool with get
    
    abstract member Fetch: int -> int -> Async<seq<IPostWrapper>>
    abstract member Whoami: unit -> Async<string>
    abstract member GetUserIcon: int -> Async<string>

    member this.FetchAsync (skip, take) = this.Fetch skip take |> Async.StartAsTask
    member this.WhoamiAsync () = this.Whoami () |> Async.StartAsTask
    member this.GetUserIconAsync size = this.GetUserIcon size |> Async.StartAsTask

type SampleWrapper2(folder: string) =
    inherit SourceWrapper()

    override this.Name with get() = ""
    override this.SubmissionsFiltered with get() = false

    override this.Fetch skip take = async {
        return Seq.empty
    }

    override this.Whoami () = async {
        return "user"
    }

    override this.GetUserIcon size = async {
        return null;
    }