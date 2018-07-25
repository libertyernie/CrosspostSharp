namespace SourceWrappers

open System.Net
open System.Threading.Tasks
open Tweetinvi.Models
open Tweetinvi.Models.Entities
open Tweetinvi
open Tweetinvi.Parameters

type TwitterPostWrapper(tweet: ITweet, media: IMediaEntity option, twitterCredentials: ITwitterCredentials) =
    interface IPostBase with
        member this.Title = ""
        member this.HTMLDescription =
            let text =
                match media with
                | Some m -> tweet.FullText.Replace(m.URL, "")
                | None -> tweet.FullText
            sprintf "<p>%s</p>" (WebUtility.HtmlEncode(text).Replace("\n", "<br/>"))
        member this.Mature = tweet.PossiblySensitive
        member this.Adult = false
        member this.Tags = tweet.Hashtags |> Seq.map (fun t -> t.Text)
        member this.Timestamp = tweet.CreatedAt
        member this.ViewURL = tweet.Url
    interface IDeletable with
        member this.SiteName = "Twitter"
        member this.DeleteAsync () = Auth.ExecuteOperationWithCredentials(twitterCredentials, (fun () -> TweetAsync.DestroyTweet(tweet))) :> Task

type TwitterPhotoPostWrapper(tweet: ITweet, media: IMediaEntity, twitterCredentials: ITwitterCredentials) =
    inherit TwitterPostWrapper(tweet, Some media, twitterCredentials)
    interface IRemotePhotoPost with
        member this.ImageURL = sprintf "%s:large" media.MediaURLHttps
        member this.ThumbnailURL = sprintf "%s:thumb" media.MediaURLHttps

type TwitterSourceWrapper(twitterCredentials: ITwitterCredentials, photosOnly: bool) =
    inherit SourceWrapper<int64>()

    let execute_with_credentials (f: unit -> Task<'a>) = Auth.ExecuteOperationWithCredentials(twitterCredentials, f) |> Async.AwaitTask

    let mutable user: IAuthenticatedUser option = None

    let getUser = async {
        match user with
        | Some u -> return u
        | None ->
            let! u = execute_with_credentials (fun () -> UserAsync.GetAuthenticatedUser())
            user <- Some u
            return u
    }

    let wrap t = TwitterPostWrapper (t, None, twitterCredentials) |> Swu.potBase
    let wrapPhoto t m = TwitterPhotoPostWrapper (t, m, twitterCredentials) |> Swu.potBase

    let isPhoto (m: IMediaEntity) = m.MediaType = "photo"

    override this.Name =
        if photosOnly then
            "Twitter (photos)"
        else
            "Twitter (text + photos)"
    
    override this.SuggestedBatchSize = 20

    override this.Fetch cursor take = async {
        let ps = new UserTimelineParameters()
        ps.ExcludeReplies <- false
        ps.IncludeEntities <- true
        ps.IncludeRTS <- true
        ps.MaxId <- cursor |> Option.defaultValue -1L
        ps.MaximumNumberOfTweetsToRetrieve <- take

        let! user = getUser
        let! tweets = execute_with_credentials (fun () -> TimelineAsync.GetUserTimeline(user, ps))

        if Seq.isEmpty tweets then
            return {
                Posts = Seq.empty
                Next = ps.MaxId
                HasMore = false
            }
        else
            return {
                Posts = seq {
                    for t in tweets do
                        if not t.IsRetweet then
                            let photos = t.Media |> Seq.filter isPhoto
                            if Seq.isEmpty photos then
                                if not photosOnly then
                                    yield wrap t
                            else
                                for m in photos do
                                    yield wrapPhoto t m

                }
                Next = (tweets |> Seq.map (fun t -> t.Id) |> Seq.min) - 1L
                HasMore = true
            }
    }

    override this.Whoami = async {
        let! u = getUser
        return u.ScreenName
    }

    override this.GetUserIcon size = async {
        let! u = getUser
        return u.ProfileImageUrl
    }