namespace SourceWrappers

open System
open System.Threading.Tasks
open System.IO

/// A shared base interface for text and photo posts.
type IPostBase = ArtworkSourceSpecification.IPostBase

/// A wrapper around a post with a thumbnail.
type IThumbnailPost =
    inherit IPostBase
    abstract member ThumbnailURL: string with get

/// A wrapper around a post (probably an image post) from an art or social media site.
type IRemotePhotoPost =
    inherit IThumbnailPost
    abstract member ImageURL: string with get

/// A wrapper around a post with a video (probably a converted animated GIF.)
type IRemoteVideoPost =
    inherit IThumbnailPost
    abstract member VideoURL: string with get

[<AllowNullLiteral>]
type IDownloadedData =
    abstract member Data: byte[]
    abstract member ContentType: string
    abstract member Filename: string

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

    interface IThumbnailPost with
        member this.Title = this.Title
        member this.ViewURL = this.ViewURL
        member this.ThumbnailURL = this.ThumbnailURL

        member this.HTMLDescription = ""
        member this.Mature = false
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = this.Timestamp |> Option.defaultValue DateTime.MinValue

/// An interface that provides a method for deleting a post.
type IDeletable =
    abstract member DeleteAsync: unit -> Task
    abstract member SiteName: string with get

module Downloader =
    open System.Net
    open System.Security.Cryptography
    //open System.Drawing.Imaging

    let private AsyncDownloadUrl (url: string) = async {
        let req = WebRequest.Create url
        use! resp = req.AsyncGetResponse()

        use stream = resp.GetResponseStream()
        use ms = new MemoryStream()
        do! stream.CopyToAsync ms |> Async.AwaitTask

        let data = ms.ToArray()

        let md5 =
            MD5.Create().ComputeHash(data)
            |> Seq.map (fun b -> (int b).ToString("x2"))
            |> String.concat ""
        let contentType = resp.ContentType
        let ext = contentType.Split('/') |> Seq.last
        return Some {
            new IDownloadedData with
                member __.Data = data
                member __.ContentType = contentType
                member __.Filename = sprintf "%s.%s" md5 ext
        }
    }

    let rec AsyncDownload (post: IPostBase) = async {
        match post with
        | :? DeferredPhotoPost as deferred ->
            let! x = deferred.AsyncGetActual()
            return! AsyncDownload x
        | :? IDownloadedData as downloaded -> return Some downloaded
        | :? IRemotePhotoPost as remotePhoto -> return! AsyncDownloadUrl remotePhoto.ImageURL
        | :? IRemoteVideoPost as remoteVideo -> return! AsyncDownloadUrl remoteVideo.VideoURL
        | _ -> return None
    }

    let DownloadAsync post = Async.StartAsTask (async {
        let! d = AsyncDownload post
        return d |> Option.toObj
    })

    //let ConvertToPng (post: IDownloadedData) =
    //    use ms = new MemoryStream(post.Data, false)
    //    use image = Image.FromStream(ms)
    //    use ms2 = new MemoryStream()
    //    image.Save(ms2, ImageFormat.Png)
    //    {
    //        new IDownloadedData with
    //            member __.Data = ms2.ToArray()
    //            member __.ContentType = "image/png"
    //            member __.Filename = Path.GetFileNameWithoutExtension(post.Filename) |> sprintf "%s.png"
    //    }