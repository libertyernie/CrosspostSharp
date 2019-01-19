namespace SourceWrappers

open System
open System.Threading.Tasks

/// A shared base interface for text and photo posts.
type IPostBase =
    abstract member Title: string with get
    abstract member HTMLDescription: string with get
    abstract member Mature: bool with get
    abstract member Adult: bool with get
    abstract member Tags: seq<string> with get
    abstract member Timestamp: DateTime with get
    abstract member ViewURL: string with get

/// A wrapper around a post (probably an image post) from an art or social media site.
type IRemotePhotoPost =
    inherit IPostBase

    abstract member ImageURL: string with get
    abstract member ThumbnailURL: string with get

/// A wrapper around a post with a video (probably a converted animated GIF.)
type IRemoteVideoPost =
    inherit IPostBase

    abstract member VideoURL: string with get

type DeferredPhotoPostParameters = {
    Title: string
    Mature: bool
    Adult: bool
    Timestamp: DateTime
    ViewURL: string
    ThumbnailURL: string
}

[<AbstractClass>]
type DeferredPhotoPost() =
    abstract member Title: string with get
    abstract member ViewURL: string with get
    abstract member ThumbnailURL: string with get

    abstract member Timestamp: DateTime option with get

    abstract member AsyncGetActual: unit -> Async<IRemotePhotoPost>
    member this.GetActualAsync() = this.AsyncGetActual() |> Async.StartAsTask

    interface IRemotePhotoPost with
        member this.Title = this.Title
        member this.ViewURL = this.ViewURL
        member this.ThumbnailURL = this.ThumbnailURL

        member this.HTMLDescription = ""
        member this.Mature = false
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = this.Timestamp |> Option.defaultValue DateTime.MinValue
        member this.ImageURL = this.ThumbnailURL

/// An object representing an image post, including the image data. Can be serialized to JSON.
type SavedPhotoPost =
    {
        data: byte[]
        title: string
        description: string
        url: string
        tags: seq<string>
        mature: bool
        adult: bool
    }
    interface IPostBase with
        member this.Title = this.title
        member this.HTMLDescription = this.description
        member this.Mature = this.mature
        member this.Adult = this.adult
        member this.Tags = this.tags
        member this.ViewURL = this.url
        member __.Timestamp = DateTime.UtcNow

/// An interface that provides a method for deleting a post.
type IDeletable =
    abstract member DeleteAsync: unit -> Task
    abstract member SiteName: string with get

/// Supplies methods for converting an IPostWrapper or a file to an ArtworkData and finding valid mimetypes and filenames for images.
module PostConverter =
    open System.Net
    open System.IO
    open System.Text
    open Newtonsoft.Json
    open System.Drawing
    open System.Drawing.Imaging
    open System.Security.Cryptography

    let AsyncDownload (post: IRemotePhotoPost) = async {
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

    let GetContentType (post: SavedPhotoPost) =
        let startsWith header file =
            let byte_header = header |> Seq.map byte
            Seq.forall2 (=) byte_header file && Array.length file >= Seq.length header

        if startsWith [0xFF; 0xD8] post.data then "image/jpeg"
        else if startsWith [0x89; 0x50; 0x4E; 0x47; 0x0D; 0x0A; 0x1A; 0x0A] post.data then "image/png"
        else if startsWith [0x47; 0x49; 0x46] post.data then "image/gif"
        else "application/octet-stream"

    let ReplaceData (data: byte[]) (post: SavedPhotoPost) =
        {
            data = data
            title = post.title
            description = post.description
            tags = post.tags
            mature = post.mature
            adult = post.adult
            url = post.url
        }

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
                data |> Encoding.UTF8.GetString |> JsonConvert.DeserializeObject<SavedPhotoPost> |> Some
            with
                | _ -> None
        else
            None

    let FromFile filename =
        let default_title = Path.GetFileName filename
        let data = File.ReadAllBytes filename
        TryDeserializeJson data |> Option.defaultValue (FromData data default_title)

    let CreateFilename (post: SavedPhotoPost) =
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