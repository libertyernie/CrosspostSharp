namespace SourceWrappers

open System
open FSharp.Control
open ArtworkSourceSpecification

module internal Swu =
    let skipSafe num = 
        Seq.zip (Seq.initInfinite id)
        >> Seq.skipWhile (fun (i, _) -> i < num)
        >> Seq.map snd

    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

type AsyncSeqWrapperUserInfo = {
    username: string
    icon_url: string option
} with
    interface IAuthor with
        member this.Name = this.username
        member this.IconUrl = this.icon_url |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

[<AbstractClass>]
type AsyncSeqWrapper() =
    abstract member Name: string with get
    abstract member FetchUserInternal: unit -> Async<AsyncSeqWrapperUserInfo>
    abstract member FetchSubmissionsInternal: unit -> AsyncSeq<IPostBase>

    interface IArtworkSource with
        member this.Name = this.Name
        member this.GetPostsAsync() = this.FetchSubmissionsInternal() |> AsyncSeq.toAsyncEnum
        member this.GetUserAsync() = this.FetchUserInternal() |> Swu.whenDone (fun a -> a :> IAuthor) |> Async.StartAsTask

type AsyncSeqWrapperOfSeq(name: string, seq: seq<IPostBase>) =
    inherit AsyncSeqWrapper()

    override __.Name = name
    override __.FetchSubmissionsInternal() = AsyncSeq.ofSeq seq
    override __.FetchUserInternal() = async {
        return {
            username = ""
            icon_url = None
        }
    }