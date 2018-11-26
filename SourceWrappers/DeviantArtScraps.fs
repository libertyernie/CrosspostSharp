namespace SourceWrappers

open SourceWrappers
open System
open System.Text.RegularExpressions
open DeviantartApi.Requests.Deviation
open FSharp.Control
open DeviantartApi.Requests.User.Profile

type internal DeviantArtScrapsLinkWrapper(url: string, title: string, img: string) =
    inherit DeferredPhotoPost()
    
    let app_link_regex = new Regex("DeviantArt://deviation/(........-....-....-....-............)")

    override __.Title = title
    override __.ViewURL = url
    override __.ThumbnailURL = img
    override __.AsyncGetActual() = async {
        let! html =
            new Uri(url)
            |> DeviantartApi.Requester.MakeRequestRawAsync
            |> Async.AwaitTask

        let m = app_link_regex.Match(html)
        if not m.Success then
            failwithf "Could not scrape GUID from DeviantArt page: %s" url

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

        return new DeviantArtPostWrapper(d, Some m) :> IRemotePhotoPost
    }

type DeviantArtScrapsLinkSourceWrapper(username: string) =
    inherit AsyncSeqWrapper()

    let next_regex = new Regex("<link href=\"([^'\"]+)\" rel=\"next\">")

    let findMatches html (r: Regex) = seq {
        for o in r.Matches(html) do
            yield o.Value
    }

    override __.Name = "DeviantArt (scraps)"

    override __.FetchSubmissionsInternal() = asyncSeq {
        let mutable url = sprintf "https://%s.deviantart.com/gallery/?catpath=scraps" username
        let mutable more = true

        while more do
            let! html =
                new Uri(url)
                |> DeviantartApi.Requester.MakeRequestRawAsync
                |> Async.AwaitTask

            let posts_complex = Seq.cache <| seq {
                let link_complex_regex = new Regex("<a class=\"torpedo-thumb-link\" href=\"([^\"]+)\"><img data-sigil=\"torpedo-img\" src=\"([^\"]+)\" alt=\"([^\"]+)\"")
                for o in link_complex_regex.Matches(html) do
                    let url = o.Groups.[1].Value
                    let img = o.Groups.[2].Value
                    let title = o.Groups.[3].Value
                    yield (url, title, img)
            }

            for (url, title, img) in posts_complex do
                yield new DeviantArtScrapsLinkWrapper(url, title, img) :> IPostBase

            if Seq.isEmpty posts_complex then
                let urls = seq {
                    let link_simple_regex = new Regex("https://www\.deviantart\.com/[^/]+/art/[^'\"]+")
                    for o in link_simple_regex.Matches(html) do
                        yield o.Value
                }
                for url in Seq.distinct urls do
                    let w = new DeviantArtScrapsLinkWrapper(url, "", "")
                    let! p = w.AsyncGetActual()
                    yield p :> IPostBase

            let next = next_regex.Match(html)

            url <- next.Groups.[1].Value
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

    let parent = new DeviantArtScrapsLinkSourceWrapper(username) :> AsyncSeqWrapper

    let get (p: IPostBase) = async {
        match p with
        | :? DeferredPhotoPost as d ->
            let! post = d.AsyncGetActual()
            return post :> IPostBase
        | _ ->
            return p
    }

    override __.Name = "DeviantArt (scraps)"

    override __.FetchSubmissionsInternal() = AsyncSeq.mapAsync get parent

    override __.FetchUserInternal() = parent.FetchUserInternal()