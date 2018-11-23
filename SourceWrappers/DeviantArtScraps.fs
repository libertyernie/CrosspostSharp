namespace SourceWrappers

open SourceWrappers
open System
open System.Text.RegularExpressions
open DeviantartApi.Requests.Deviation
open FSharp.Control
open DeviantartApi.Requests.User.Profile

type internal DeviantArtScrapsLinkWrapper(url: string) =
    interface IPostBase with
        member __.Title = url
        member __.HTMLDescription = null
        member __.Mature = false
        member __.Adult = false
        member __.Tags = Seq.empty
        member __.Timestamp = DateTime.MinValue
        member __.ViewURL = url

type internal DeviantArtScrapsLinkSourceWrapper(username: string) =
    inherit AsyncSeqWrapper()

    let link_regex = new Regex("https://www\.deviantart\.com/[^/]+/art/[^'\"]+")
    let next_regex = new Regex("<link href=\"([^'\"]+)\" rel=\"next\">")

    let findMatches html (r: Regex) = seq {
        for o in r.Matches(html) do
            yield o.Value
    }

    override __.Name = "DeviantArt (scraps - links only)"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable url = sprintf "https://%s.deviantart.com/gallery/?catpath=scraps" username
        let mutable more = true

        while more do
            let! html =
                new Uri(url)
                |> DeviantartApi.Requester.MakeRequestRawAsync
                |> Async.AwaitTask

            let posts =
                link_regex
                |> findMatches html
                |> Seq.distinct
                |> Seq.map DeviantArtScrapsLinkWrapper
                |> Seq.cast
            for o in posts do
                yield o :> IPostBase

            let next = next_regex.Match(html)

            // DeviantArt bug workaround
            let next_url = next.Groups.[1].Value.Replace(sprintf "%s/%s" username username, username)

            url <- next_url
            more <- next.Success
    }

    override __.FetchUserInternal() = async {
        let req = new UsernameRequest(username)
        let! profile = Swu.executeAsync req
        return {
            username = username
            icon_url = Some profile.User.UserIconUrl.AbsoluteUri
        }
    }

type DeviantArtScrapsSourceWrapper(username: string) =
    inherit AsyncSeqWrapper()

    let link_wrapper = username |> DeviantArtScrapsLinkSourceWrapper
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

        return new DeviantArtPostWrapper(d, Some m) :> IPostBase
    }

    override __.Name = "DeviantArt (scraps)"

    override __.FetchSubmissionsInternal() =
        let parent = link_wrapper :> AsyncSeqWrapper
        let sequence = parent
        AsyncSeq.mapAsync wrapPost sequence

    override __.FetchUserInternal() = link_wrapper.FetchUserInternal()