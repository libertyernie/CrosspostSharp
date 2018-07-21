namespace SourceWrappers

open System

/// A shared base interface for text and photo posts.
type IPostMetadata =
    abstract member Title: string with get
    abstract member HTMLDescription: string with get
    abstract member Mature: bool with get
    abstract member Adult: bool with get
    abstract member Tags: seq<string> with get

/// A wrapper around a post (probably an image post) from an art or social media site.
type IPostWrapper =
    inherit IPostMetadata

    abstract member Timestamp: DateTime with get
    abstract member ViewURL: string with get
    abstract member ImageURL: string with get
    abstract member ThumbnailURL: string with get

/// An object representing an image post, including the image data. Can be serialized to JSON.
type ArtworkData =
    {
        data: byte[]
        title: string
        description: string
        url: string
        tags: seq<string>
        mature: bool
        adult: bool
    }
    interface IPostMetadata with
        member this.Title = this.title
        member this.HTMLDescription = this.description
        member this.Mature = this.mature
        member this.Adult = this.adult
        member this.Tags = this.tags

module PostConverter =
    open System.Net
    open System.IO
    open System.Text
    open Newtonsoft.Json
    open System.Drawing
    open System.Drawing.Imaging
    open System.Security.Cryptography

    let AsyncDownload (post: IPostWrapper) = async {
        if isNull post.ImageURL then
            invalidArg "post" "Cannot create a SavedPost object from a post without an image."

        let req = WebRequest.Create post.ImageURL
        use! resp = req.AsyncGetResponse()

        use stream = resp.GetResponseStream()
        use ms = new MemoryStream()
        do! stream.CopyToAsync ms |> Async.AwaitTask

        return {
            data = ms.ToArray()
            title = post.Title
            description = post.HTMLDescription
            tags = post.Tags
            mature = post.Mature
            adult = post.Adult
            url = post.ViewURL
        }
    }

    let DownloadAsync post = AsyncDownload post |> Async.StartAsTask

    let GetContentType (post: ArtworkData) =
        try
            use ms = new MemoryStream(post.data, false)
            let image = Image.FromStream(ms)
            match image.RawFormat.Guid with
            | x when x = ImageFormat.Png.Guid -> "image/png"
            | x when x = ImageFormat.Jpeg.Guid -> "image/jpeg"
            | x when x = ImageFormat.Gif.Guid -> "image/gif"
            | _ -> "application/octet-stream"
        with
            | _ -> "application/octet-stream"

    let FromData data title =
        {
            data = data
            title = title
            description = ""
            tags = Seq.empty
            mature = false
            adult = false
            url = null
        }

    let TryDeserializeJson data =
        if Seq.tryHead data = Some (byte '{') then
            // Try to parse JSON
            try
                data |> Encoding.UTF8.GetString |> JsonConvert.DeserializeObject<ArtworkData> |> Some
            with
                | _ -> None
        else
            None

    let FromFile filename =
        let default_title = Path.GetFileName filename
        let data = File.ReadAllBytes filename
        TryDeserializeJson data |> Option.defaultValue (FromData data default_title)

    let CreateFilename (post: ArtworkData) =
        let invalid = Path.GetInvalidFileNameChars()
        let title =
            post.title
            |> String.filter (fun c -> not (Seq.contains c invalid))
        let md5 =
            MD5.Create().ComputeHash(post.data)
            |> Seq.map (fun b -> (int b).ToString("x2"))
            |> String.concat ""
        let ext = (GetContentType post).Split('/') |> Seq.last
        sprintf "%s (%s).%s" title md5 ext