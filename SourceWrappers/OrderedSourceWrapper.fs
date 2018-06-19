namespace SourceWrappers

/// A wrapper around another ISourceWrapper that fetches a given number of posts and returns them sorted by post date (newest first.)
type OrderedSourceWrapper<'a when 'a : struct>(source: ISourceWrapper<'a>) =
    inherit SourceWrapper<int>()

    override this.Name = source.Name
    override this.SuggestedBatchSize = 1000

    override this.Fetch initialCursor take = async {
        let! result = source.FetchAllAsync take |> Async.AwaitTask

        return {
            Posts = result |> Seq.sortByDescending (fun w -> w.Timestamp)
            Next = 0
            HasMore = false
        }
    }

    override this.Whoami = source.WhoamiAsync() |> Async.AwaitTask

    override this.GetUserIcon size = source.GetUserIconAsync size |> Async.AwaitTask
