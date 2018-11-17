namespace SourceWrappers

open FlickrNet
open System.Threading.Tasks
open FSharp.Control

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
    inherit AsyncSeqWrapper()

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

    override this.StartNew() = asyncSeq {
        let mutable cursor = 1
        let mutable more = true
        let extras = PhotoSearchExtras.Description ||| PhotoSearchExtras.Tags ||| PhotoSearchExtras.DateUploaded ||| PhotoSearchExtras.OriginalFormat
        
        while more do
            let! posts = flickrCall (fun callback -> flickr.PeopleGetPhotosAsync("me", extras, cursor, 100 |> min 500, callback))
            for p in posts do
                yield new FlickrPostWrapper(p) :> IPostBase
            
            cursor <- posts.Page + 1
            more <- posts.Page < posts.Pages
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