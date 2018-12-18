namespace SourceWrappers

open SourceWrappers
open System
open System.Text.RegularExpressions
open FSharp.Control
open System.Net
open System.IO
open DeviantArtFs

type DeviantArtScrapsPostWrapper(deviation: DeviantArtFs.Deviation.IdResponse.Root, metadata: DeviantArtMetadataResponse.Metadata) =
    let src =
        deviation.Content
        |> Option.map (fun c -> c.Src)
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

    interface IRemotePhotoPost with
        member __.Title =
            deviation.Title
            |> Option.defaultValue (deviation.Deviationid.ToString())
        member __.HTMLDescription = metadata.Description
        member __.Mature =
            deviation.IsMature
            |> Option.defaultValue false
        member __.Adult = false
        member __.Tags = metadata.Tags |> Seq.map (fun t -> t.TagName)
        member __.Timestamp =
            deviation.PublishedTime
            |> Option.defaultValue 0
            |> Swu.fromUnixTime
        member __.ViewURL =
            deviation.Url
            |> Option.defaultValue "https://www.example.com"
        member __.ImageURL = src
        member __.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue src

type DeviantArtScrapsLinkWrapper(url: string, title: string, img: string, client: DeviantArtClient) =
    inherit DeferredPhotoPost()
    
    let app_link_regex = new Regex("DeviantArt://deviation/(........-....-....-....-............)")

    override __.Title = title
    override __.ViewURL = url
    override __.ThumbnailURL = img
    override __.Timestamp = None
    override __.AsyncGetActual() = async {
        let req = WebRequest.CreateHttp url
        req.UserAgent <- "SourceWrapper/0.0 (https://github.com/libertyernie/CrosspostSharp)"
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! html = sr.ReadToEndAsync() |> Async.AwaitTask

        let m = app_link_regex.Match(html)
        if not m.Success then
            failwithf "Could not scrape GUID from DeviantArt page: %s" url

        let! deviation =
            m.Groups.[1].Value
            |> Guid.Parse
            |> DeviantArtFs.Deviation.Id.AsyncExecute client

        let! metadata =
            m.Groups.[1].Value
            |> Guid.Parse
            |> Seq.singleton
            |> client.AsyncDeviationMetadata None None None None

        let d = deviation
        let m = metadata.Metadata |> Seq.head

        return new DeviantArtScrapsPostWrapper(d, m) :> IRemotePhotoPost
    }

type DeviantArtScrapsLinkSourceWrapper(username: string, client: DeviantArtClient) =
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
            let req = WebRequest.CreateHttp url
            req.UserAgent <- "SourceWrapper/0.0 (https://github.com/libertyernie/CrosspostSharp)"
            use! resp = req.AsyncGetResponse()
            use sr = new StreamReader(resp.GetResponseStream())

            let! html = sr.ReadToEndAsync() |> Async.AwaitTask

            let posts_complex = Seq.cache <| seq {
                let link_complex_regex = new Regex("<a class=\"torpedo-thumb-link\" href=\"([^\"]+)\"><img data-sigil=\"torpedo-img\" src=\"([^\"]+)\" alt=\"([^\"]+)\"")
                for o in link_complex_regex.Matches(html) do
                    let url = o.Groups.[1].Value
                    let img = o.Groups.[2].Value
                    let title = o.Groups.[3].Value
                    yield (url, title, img)
            }

            for (url, title, img) in posts_complex do
                yield new DeviantArtScrapsLinkWrapper(url, title, img, client) :> IPostBase

            if Seq.isEmpty posts_complex then
                let urls = seq {
                    let link_simple_regex = new Regex("https://www\.deviantart\.com/[^/]+/art/[^'\"]+")
                    for o in link_simple_regex.Matches(html) do
                        yield o.Value
                }
                for url in Seq.distinct urls do
                    let w = new DeviantArtScrapsLinkWrapper(url, "", "", client)
                    let! p = w.AsyncGetActual()
                    yield p :> IPostBase

            let next = next_regex.Match(html)

            url <- next.Groups.[1].Value
            more <- next.Success
    }

    override __.FetchUserInternal() = async {
        let! profile =
            username
            |> DeviantArtFs.User.ProfileRequest
            |> DeviantArtFs.User.Profile.AsyncExecute client
        return {
            username = username
            icon_url = Some profile.User.Usericon
        }
    }

type DeviantArtScrapsSourceWrapper(username: string, client: DeviantArtClient) =
    inherit AsyncSeqWrapper()

    let parent = new DeviantArtScrapsLinkSourceWrapper(username, client) :> AsyncSeqWrapper

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