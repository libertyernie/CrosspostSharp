namespace SourceWrappers

open FlickrNet
open System.Threading.Tasks

type FlickrPostWrapper(photo: Photo) =
    interface IRemotePhotoPost with
        member this.Title = photo.Title
        member this.HTMLDescription = photo.Description
        member this.Mature = false
        member this.Adult = false
        member this.Tags = photo.Tags :> seq<string>
        member this.Timestamp = photo.DateUploaded
        member this.ViewURL = sprintf "https://www.flickr.com/photos/%s/%s" photo.UserId photo.PhotoId
        member this.ImageURL = sprintf "https://farm%s.staticflickr.com/%s/%s_%s_o.%s" photo.Farm photo.Server photo.PhotoId photo.OriginalSecret photo.OriginalFormat
        member this.ThumbnailURL = sprintf "https://farm%s.staticflickr.com/%s/%s_%s_q.jpg" photo.Farm photo.Server photo.PhotoId photo.Secret

type FlickrSourceWrapper(flickr: Flickr) =
    inherit SourceWrapper<int>()

    let flickrCall f1 = async {
        let tcs = new TaskCompletionSource<'a>()
        f1(new System.Action<FlickrResult<'a>>(fun r -> if r.HasError then tcs.SetException r.Error else tcs.SetResult r.Result))
        return! tcs.Task |> Async.AwaitTask
    }

    let mutable user: FoundUser = null

    let getUser = async {
        if isNull user then
            let! oauth = flickrCall (fun callback -> flickr.AuthOAuthCheckTokenAsync(callback))
            user <- oauth.User
        return user
    }

    new(apiKey: string, sharedSecret: string, oAuthAccessToken: string, oAuthAccessTokenSecret: string) =
        FlickrSourceWrapper(new Flickr(apiKey, sharedSecret, OAuthAccessToken = oAuthAccessToken, OAuthAccessTokenSecret = oAuthAccessTokenSecret))

    override this.Name = "Flickr"
    override this.SuggestedBatchSize = 100

    override this.Fetch cursor take = async {
        let start = cursor |> Option.defaultValue 1
        let extras = PhotoSearchExtras.Description ||| PhotoSearchExtras.Tags ||| PhotoSearchExtras.DateUploaded ||| PhotoSearchExtras.OriginalFormat
        
        let! posts = flickrCall (fun callback -> flickr.PeopleGetPhotosAsync("me", extras, start, take |> min 500, callback))
        return {
            Posts = posts |> Seq.map FlickrPostWrapper |> Seq.map Swu.potBase
            Next = posts.Page + 1
            HasMore = posts.Page < posts.Pages
        }
    }

    override this.Whoami = async {
        let! user = getUser
        return user.UserName
    }

    override this.GetUserIcon size = async {
        let! user = getUser
        
        let! person = flickrCall (fun callback -> flickr.PeopleGetInfoAsync(user.UserId, callback))
        return if person.IconServer = "0"
            then "https://www.flickr.com/images/buddyicon.gif"
            else sprintf "http://farm%s.staticflickr.com/%s/buddyicons/%s.jpg" person.IconFarm person.IconServer person.UserId
    }