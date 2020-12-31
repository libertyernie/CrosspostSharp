namespace SourceWrappers

open ArtworkSourceSpecification
open System.Text
open Newtonsoft.Json
open System.IO
open System

type SavedPhotoPost = {
    data: byte[]
    title: string
    description: string
    url: string
    tags: seq<string>
    mature: bool
    adult: bool
} with
    member this.ContentType =
        let startsWith header file =
            let byte_header = header |> Seq.map byte
            Seq.forall2 (=) byte_header file && Array.length file >= Seq.length header

        if startsWith [0xFF; 0xD8] this.data then "image/jpeg"
        else if startsWith [0x89; 0x50; 0x4E; 0x47; 0x0D; 0x0A; 0x1A; 0x0A] this.data then "image/png"
        else if startsWith [0x47; 0x49; 0x46] this.data then "image/gif"
        else "application/octet-stream"

    member this.Filename =
        let invalid = System.IO.Path.GetInvalidFileNameChars()
        let title = this.title |> String.filter (fun c -> not (Seq.contains c invalid))
        let md5 =
            System.Security.Cryptography.MD5.Create().ComputeHash(this.data)
            |> Seq.map (fun b -> (int b).ToString("x2"))
            |> String.concat ""
        let ext = this.ContentType.Split('/') |> Seq.last
        sprintf "%s (%s).%s" title md5 ext
        
    static member FromPost (post: IPostBase) (downloaded: IDownloadedData) =
        {
            data = downloaded.Data
            title = post.Title
            description = post.HTMLDescription
            tags = post.Tags
            mature = post.Mature
            adult = post.Adult
            url = post.ViewURL
        }
        
    static member FromData data title =
        {
            data = data
            title = title
            description = ""
            tags = Seq.empty
            mature = false
            adult = false
            url = null
        }

    static member private TryDeserializeJson data =
        if Seq.tryHead data = Some (byte '{') then
            // Try to parse JSON
            try
                data |> Encoding.UTF8.GetString |> JsonConvert.DeserializeObject<SavedPhotoPost> |> Some
            with
                | _ -> None
        else
            None

    static member FromFile (filename: string) =
        let default_title = Path.GetFileName filename
        let data = File.ReadAllBytes filename
        SavedPhotoPost.TryDeserializeJson data |> Option.defaultValue (SavedPhotoPost.FromData data default_title)

    interface IPostBase with
        member this.Title = this.title
        member this.HTMLDescription = this.description
        member this.Mature = this.mature
        member this.Adult = this.adult
        member this.Tags = this.tags
        member this.ViewURL = this.url
        member __.Timestamp = DateTime.UtcNow

    interface IDownloadedData with
        member this.Data = this.data
        member this.ContentType = this.ContentType
        member this.Filename = this.Filename