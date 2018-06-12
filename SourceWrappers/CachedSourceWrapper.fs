namespace SourceWrappers

open System.Collections.Generic

type CachedSourceWrapper<'a when 'a : struct>(source: SourceWrapper<'a>) =
    inherit SourceWrapper<int>()

    let cache = new List<IPostWrapper>()
    let mutable cursor: 'a option = None
    let mutable ended = false

    let skipSafe num = 
        Seq.zip (Seq.initInfinite id)
        >> Seq.skipWhile (fun (i, _) -> i < num)
        >> Seq.map snd

    override this.Name = source.Name

    override this.Fetch index take = async {
        let skip = index |> Option.defaultValue 0

        if cache.Count >= skip + take then
            return {
                Posts = cache |> skipSafe skip |> Seq.truncate take
                Next = skip + take
                HasMore = true
            }
        else if ended then
            let posts = cache |> skipSafe skip |> Seq.truncate take
            return {
                Posts = posts
                Next = skip + Seq.length posts
                HasMore = false
            }
        else
            let needed = skip + take - cache.Count
            let! result = source.Fetch cursor needed

            cache.AddRange(result.Posts)
            cursor <- Some result.Next
            ended <- not result.HasMore

            return! this.Fetch index take
    }

    override this.Whoami() = source.Whoami()

    override this.GetUserIcon size = source.GetUserIcon size