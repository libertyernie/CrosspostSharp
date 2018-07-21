namespace SourceWrappers

open FurryNetworkLib
open System.Net
open System

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
    inherit SourceWrapper<int>()
    
    let mutable character: Character = null

    let getCharacter = async {
        if isNull character then
            let! user = client.GetUserAsync() |> Async.AwaitTask
            character <-
                user.characters
                |> Seq.filter (fun c -> c.Name = characterName)
                |> Seq.tryHead
                |> Option.defaultValue user.DefaultCharacter
        return character
    }
    
    override this.Name = "Furry Network"
    override this.SuggestedBatchSize = 30

    override this.Fetch cursor take = async {
        let start = cursor |> Option.defaultValue 0
        let! character = getCharacter
        let! searchResults = client.SearchByCharacterAsync(character.Name, Seq.singleton "artwork", from = (start |> Nullable)) |> Async.AwaitTask
        let nextPosition = start + Seq.length searchResults.Hits
        return {
            Posts = searchResults.Hits
                |> Seq.map (fun h -> h.Submission)
                |> Seq.filter (fun s -> s :? Artwork || s :? Photo)
                |> Seq.map (fun s -> s :?> FileSubmission)
                |> Seq.map (fun s -> new FurryNetworkPostWrapper(s, client))
                |> Seq.map Swu.toPostWrapperInterface
            Next = nextPosition
            HasMore = nextPosition > searchResults.Total
        }
    }

    override this.Whoami = async {
        let! character = getCharacter
        return character.Name
    }

    override this.GetUserIcon size = async {
        let! character = getCharacter
        return character.Avatars
            |> Option.ofObj
            |> Option.map (fun a -> a.GetBySize(size))
            |> Option.defaultValue null
    }