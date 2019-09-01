namespace SourceWrappers

open FSharp.Control
open SourceWrappers.Eclipse
open System

type DeviantArtEclipsePostWrapper(deviation: GalleryContentsResponse.Deviation) =
    interface IRemotePhotoPost with
        member __.Title = deviation.Title
        member __.HTMLDescription = null
        member __.Mature = deviation.IsMature
        member __.Adult = false
        member __.Tags = Seq.empty
        member __.Timestamp = deviation.PublishedTime.UtcDateTime
        member __.ViewURL = deviation.Url
        member __.ImageURL =
            deviation.Files
            |> Seq.sortByDescending (fun d -> d.Width * d.Height)
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"
        member __.ThumbnailURL =
            deviation.Files
            |> Seq.sortBy (fun d -> d.Width * d.Height)
            |> Seq.map (fun d -> d.Src)
            |> Seq.tryHead
            |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"

type DeviantArtEclipseDeferredPostWrapper(deviation: GalleryContentsResponse.Deviation) =
    inherit DeferredPhotoPost()

    override __.Title = deviation.Title
    override __.ThumbnailURL =
        deviation.Files
        |> Seq.sortBy (fun d -> d.Width * d.Height)
        |> Seq.map (fun d -> d.Src)
        |> Seq.tryHead
        |> Option.defaultValue "https://upload.wikimedia.org/wikipedia/commons/c/ce/Transparent.gif"
    override __.ViewURL = deviation.Url
    override __.Timestamp = Some deviation.PublishedTime.UtcDateTime
    override __.AsyncGetActual() = async {
        return new DeviantArtEclipsePostWrapper(deviation) :> IRemotePhotoPost
    }

type DeviantArtEclipseSourceWrapper(username: string, source: GalleryContentsSource) =
    inherit AsyncSeqWrapper()

    override __.Name =
        match source with
        | GalleryContentsSource.All -> "DeviantArt (gallery)"
        | GalleryContentsSource.Default -> "DeviantArt"
        | GalleryContentsSource.Folder f -> sprintf "DeviantArt (%d)" f
        | GalleryContentsSource.Scraps -> "DeviantArt (scraps)"

    override __.FetchSubmissionsInternal() =
        new GalleryContentsRequest(username, Limit = Some 24, Folder = GalleryContentsSource.All)
        |> GalleryContents.AsyncGetAllResults
        |> AsyncSeq.map DeviantArtEclipseDeferredPostWrapper
        |> AsyncSeq.map (fun x -> x :> IPostBase)

    override __.FetchUserInternal() = async {
        return {
            username = username
            icon_url =
                seq {
                    yield "avatars-big"
                    yield username.Substring(0, 1) |> Uri.EscapeDataString
                    yield username.Substring(1, 1) |> Uri.EscapeDataString
                    yield sprintf "%s.png" username |> Uri.EscapeDataString
                }
                |> String.concat "/"
                |> sprintf "https://a.deviantart.net/%s"
                |> Some
        }
    }