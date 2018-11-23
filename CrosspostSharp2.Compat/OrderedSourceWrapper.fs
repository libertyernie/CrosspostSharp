namespace CrosspostSharp2.Compat

open SourceWrappers

/// A wrapper around another ISourceWrapper that fetches a given number of posts and returns them sorted by post date (newest first.)
type OrderedSourceWrapper<'a when 'a : struct>(source: ISourceWrapper<'a>) =
    member this.Name = source.Name
    member this.SuggestedBatchSize = 1000

    member this.Fetch initialCursor take = async {
        let! result = source.FetchAllAsync take |> Async.AwaitTask

        return {
            Posts = result |> Seq.sortByDescending (fun w -> w.Timestamp)
            Next = 0
            HasMore = false
        }
    }

    member this.Whoami = source.WhoamiAsync() |> Async.AwaitTask

    member this.GetUserIcon size = source.GetUserIconAsync size |> Async.AwaitTask

    member private this.FetchAll (initial: seq<IPostBase>) (cursor: 'cursor option) (limit: int) = async {
        let! result = this.Fetch cursor limit
        if not result.HasMore || Seq.length result.Posts >= limit then
            return result.Posts |> Seq.truncate limit |> Seq.append initial
        else
            return! this.FetchAll (Seq.append initial result.Posts) (Some result.Next) (limit - Seq.length result.Posts)
    }
    
    member this.AsISourceWrapper() = {
        new ISourceWrapper<'cursor> with
            member __.Name = this.Name
            member __.SuggestedBatchSize = this.SuggestedBatchSize
            member __.StartAsync take = this.Fetch None take |> Async.StartAsTask
            member __.MoreAsync cursor take = this.Fetch (Some cursor) take |> Async.StartAsTask
            member __.FetchAllAsync limit = this.FetchAll Seq.empty None limit |> Async.StartAsTask
            member __.WhoamiAsync () = this.Whoami |> Async.StartAsTask
            member __.GetUserIconAsync size = this.GetUserIcon size |> Async.StartAsTask
    }