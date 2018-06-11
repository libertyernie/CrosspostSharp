namespace SourceWrappers

open DeviantartApi.Objects
open DeviantartApi.Objects.SubObjects.DeviationMetadata
open System
open DeviantartApi.Requests.User
open AsyncHelpers

[<AbstractClass>]
type DeviantArtPostWrapper(deviation: Deviation, metadata: Metadata) =
    let defaultValue (def: 'a) (o: Nullable<'a>) = if o.HasValue then o.Value else def

    interface IPostWrapper with
        member this.Title = deviation.Title
        member this.HTMLDescription =
            let html = metadata.Description
            if isNull html then
                null
            else if html.IndexOf("<p>", StringComparison.CurrentCultureIgnoreCase) = -1 then
                sprintf "<p>%s</p>" html
            else
                html
        member this.Mature = deviation.IsMature |> defaultValue false
        member this.Adult = false
        member this.Tags = metadata.Tags |> Seq.map (fun t -> t.TagName)
        member this.Timestamp = deviation.PublishedTime |> defaultValue DateTime.UtcNow
        member this.ViewURL = deviation.Url.AbsoluteUri
        member this.ImageURL = deviation.Content.Src
        member this.ThumbnailURL =
            deviation.Thumbs
            |> Seq.map (fun d -> d.Src)
            |> Seq.append [null]
            |> Seq.head

type DeviantArtSourceWrapper() =
    inherit SourceWrapper<uint32>()

    let mutable cached_user: User = null

    let getUser = async {
        if isNull cached_user then
            let req = new WhoAmIRequest()
            let! u =
                req.ExecuteAsync()
                |> Async.AwaitTask
                |> AsyncHelpers.whenDone AsyncHelpers.processDeviantArtError
            cached_user <- u
        return cached_user
    }
    
    override this.Name = "DeviantArt (F#)"
    override this.SubmissionsFiltered = true

    override this.Fetch cursor take = async {
        return {
            Posts = Seq.empty
            Next = uint32 0
        }
    }

    override this.Whoami () = getUser |> whenDone (fun u -> u.Username)

    override this.GetUserIcon size = getUser |> whenDone (fun u -> u.UserIconUrl.AbsoluteUri)