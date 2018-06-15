namespace SourceWrappers

open System.Net
open PinSharp.Models
open PinSharp
open PinSharp.Models.Images

type PinterestPostWrapper(pin: IPin) =
    //let imageUrl =
    //    let o = pin.Images.Original
    //    if isNull o then null else o.Url

    interface IPostWrapper with
        member this.Title = ""
        member this.HTMLDescription = WebUtility.HtmlEncode(pin.Note)
        member this.Mature = false
        member this.Adult = false
        member this.Tags = Seq.empty
        member this.Timestamp = pin.CreatedAt
        member this.ViewURL = pin.Url
        member this.ImageURL = pin.Images.Original.Url
        member this.ThumbnailURL = pin.Images.Original.Url

[<Struct>]
type PinterestCursor = {
    cursor: string
}

type PinterestSourceWrapper(accessToken: string, boardName: string) =
    inherit SourceWrapper<PinterestCursor>()

    let client = new PinSharpClient(accessToken)
    let mutable user: IDetailedUser option = None

    let getUser = async {
        match user with
        | Some u -> return u
        | None -> 
            let! u = client.Me.GetUserAsync() |> Async.AwaitTask
            user <- Some u
            return u
    }
    
    override this.Name = "Pinterest"
    override this.SuggestedBatchSize = 25

    override this.Fetch cursor take = async {
        let! pins =
            match cursor with
            | Some c -> client.Boards.GetPinsAsync(boardName, c.cursor, take |> min 100) |> Async.AwaitTask
            | None -> client.Boards.GetPinsAsync(boardName, take |> min 100) |> Async.AwaitTask

        if isNull pins then
            failwith <| sprintf "No pins returned. Board name \"%s\" may be invalid." boardName
        
        return {
            Posts = pins |> Seq.map PinterestPostWrapper |> Seq.map (fun w -> w :> IPostWrapper)
            Next = { cursor = pins.NextPageCursor }
            HasMore = not (isNull pins.NextPageCursor)
        }
    }

    override this.Whoami = async {
        return boardName
    }

    override this.GetUserIcon size = async {
        let! user = getUser
        //let o = user.Images.W60
        //return if isNull o then null else o.Url
        return user.Images.W60.Url
    }