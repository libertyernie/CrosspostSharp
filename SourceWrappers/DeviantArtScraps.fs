namespace SourceWrappers

open SourceWrappers
open System
open System.Text.RegularExpressions
open DeviantartApi.Requests.Deviation

type internal DeviantArtScrapsLinkWrapper(url: string) =
    interface IPostBase with
        member __.Title = url
        member __.HTMLDescription = null
        member __.Mature = false
        member __.Adult = false
        member __.Tags = Seq.empty
        member __.Timestamp = DateTime.MinValue
        member __.ViewURL = url

[<Struct>]
type internal DeviantArtScrapsCursor = {
    NextURL: string
}

type internal DeviantArtScrapsLinkSourceWrapper(username: string) =
    inherit SourceWrapper<DeviantArtScrapsCursor>()

    let link_regex = new Regex("https://www\.deviantart\.com/[^/]+/art/[^'\"]+")
    let next_regex = new Regex("<link href=\"([^'\"]+)\" rel=\"next\">")

    override __.Name = "DeviantArt (scraps - links only)"
    override __.SuggestedBatchSize = 24

    override __.Fetch cursor _ = async {
        let! html =
            cursor
            |> Option.map (fun c -> c.NextURL)
            |> Option.defaultValue (sprintf "https://%s.deviantart.com/gallery/?catpath=scraps" username)
            |> Uri
            |> DeviantartApi.Requester.MakeRequestRawAsync
            |> Async.AwaitTask

        let matches = seq {
            for o in link_regex.Matches(html) do
                yield o
        }

        let next = next_regex.Match(html)

        // DeviantArt bug workaround
        let next_url = next.Groups.[1].Value.Replace(sprintf "%s/%s" username username, username)

        return {
            Posts = matches
                |> Seq.map (fun m -> m.Value)
                |> Seq.distinct
                |> Seq.map DeviantArtScrapsLinkWrapper
                |> Seq.cast
            Next = { NextURL = next_url }
            HasMore = next.Success
        }
    }

    override __.Whoami = async { return username }

    override __.GetUserIcon _ = async { return sprintf "https://a.deviantart.net/avatars/%c/%c/%s.png" username.[0] username.[1] username }

type DeviantArtScrapsSourceWrapper(username: string) =
    inherit SourceWrapper<int>()

    let link_wrapper = username |> DeviantArtScrapsLinkSourceWrapper |> CachedSourceWrapperImpl<DeviantArtScrapsCursor>
    let app_link_regex = new Regex("DeviantArt://deviation/(........-....-....-....-............)")

    let wrapPost (w: IPostBase) = async {
        let! html =
            w.ViewURL
            |> Uri
            |> DeviantartApi.Requester.MakeRequestRawAsync
            |> Async.AwaitTask

        let m = app_link_regex.Match(html)
        if not m.Success then
            failwithf "Could not scrape GUID from DeviantArt page: %s" w.ViewURL

        let! deviation =
            m.Groups.[1].Value
            |> DeviationRequest
            |> Swu.executeAsync

        let! metadata =
            m.Groups.[1].Value
            |> Seq.singleton
            |> MetadataRequest
            |> Swu.executeAsync

        let d = deviation
        let m = metadata.Metadata |> Seq.head

        return new DeviantArtPostWrapper(d, Some m)
    }

    override __.Name = "DeviantArt (scraps)"
    override __.SuggestedBatchSize = 1

    override __.Fetch cursor take = async {
        let! items = link_wrapper.Fetch cursor take
        
        let! posts = items.Posts |> Seq.map wrapPost |> Async.Parallel

        return {
            Posts = posts |> Seq.cast
            Next = items.Next
            HasMore = items.HasMore
        }
    }

    override __.Whoami = link_wrapper.Whoami

    override __.GetUserIcon size = link_wrapper.GetUserIcon size