namespace SourceWrappers

open FurryNetworkLib
open System.Net
open System
open FSharp.Control

type FurryNetworkPostWrapper(artwork: FileSubmission, client: FurryNetworkClient) =
    interface IRemotePhotoPost with
        member this.Title = artwork.Title
        member this.HTMLDescription =
            try
                CommonMark.CommonMarkConverter.Convert(artwork.Description)
            with
            | ex -> WebUtility.HtmlEncode(artwork.Description)
        member this.Mature = artwork.Rating = 1
        member this.Adult = artwork.Rating >= 2
        member this.Tags = artwork.TagStrings
        member this.Timestamp = artwork.Created
        member this.ViewURL = sprintf "https://beta.furrynetwork.com/artwork/%d" artwork.Id
        member this.ImageURL = artwork.Images.Original
        member this.ThumbnailURL = artwork.Images.Thumbnail
    interface IDeletable with
        member this.SiteName = "Furry Network"
        member this.DeleteAsync () = client.DeleteArtwork artwork.Id

type FurryNetworkSourceWrapper(client: FurryNetworkClient, characterName: string) =
    inherit AsyncSeqWrapper()

    override this.Name = "Furry Network"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable cursor = 0
        let mutable more = true
        let! characterName = this.WhoamiAsync() |> Async.AwaitTask

        while more do
            let! searchResults = client.SearchByCharacterAsync(characterName, Seq.singleton "artwork", from = (cursor |> Nullable)) |> Async.AwaitTask
            
            for h in searchResults.Hits do
                match h.Submission with
                | :? FileSubmission as f -> 
                    if f :? Artwork || f :? Photo then
                        yield new FurryNetworkPostWrapper(f, client) :> IPostBase
                | _ -> ()
            
            cursor <- cursor + Seq.length searchResults.Hits
            more <- cursor > searchResults.Total
    }

    override this.FetchUserInternal() = async {
        let! user = client.GetUserAsync() |> Async.AwaitTask
        let character =
            user.characters
            |> Seq.filter (fun c -> c.Name = characterName)
            |> Seq.tryHead
            |> Option.defaultValue user.DefaultCharacter
        return {
            username = character.Name
            icon_url = character.Avatars
                |> Option.ofObj
                |> Option.map (fun a -> a.GetLargest())
        }
    }