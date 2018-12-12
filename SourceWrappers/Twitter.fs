namespace SourceWrappers

open System.Net
open System.Threading.Tasks
open Tweetinvi.Models
open Tweetinvi.Models.Entities
open Tweetinvi
open Tweetinvi.Parameters
open FSharp.Control

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
    inherit AsyncSeqWrapper()

    let execute_with_credentials (f: unit -> Task<'a>) = Auth.ExecuteOperationWithCredentials(twitterCredentials, f) |> Async.AwaitTask

    let user_task = lazy(Auth.ExecuteOperationWithCredentials (twitterCredentials, fun () -> UserAsync.GetAuthenticatedUser()))

    let getUser = user_task.Force() |> Async.AwaitTask

    let wrap t = TwitterPostWrapper (t, None, twitterCredentials) :> IPostBase
    let wrapPhoto t m = TwitterPhotoPostWrapper (t, m, twitterCredentials) :> IPostBase

    let isPhoto (m: IMediaEntity) = m.MediaType = "photo"

    member val BatchSize: int = 20 with get, set

    override __.Name =
        if photosOnly then
            "Twitter (images)"
        else
            "Twitter (text + images)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable maxId = -1L
        let mutable more = true

        while more do
            let ps = new UserTimelineParameters()
            ps.ExcludeReplies <- false
            ps.IncludeEntities <- true
            ps.IncludeRTS <- true
            ps.MaxId <- maxId
            ps.MaximumNumberOfTweetsToRetrieve <- this.BatchSize

            let! user = getUser
            let! tweets = execute_with_credentials (fun () -> TimelineAsync.GetUserTimeline(user, ps))

            for t in tweets do
                if not t.IsRetweet then
                    let photos = t.Media |> Seq.filter isPhoto
                    if Seq.isEmpty photos then
                        if not photosOnly then
                            yield wrap t
                    else
                        for m in photos do
                            yield wrapPhoto t m
                            
            more <- not (Seq.isEmpty tweets)
            if more then
                maxId <- (tweets |> Seq.map (fun t -> t.Id) |> Seq.min) - 1L
    }

    override this.FetchUserInternal() = async {
        let! u = getUser
        return {
            username = u.ScreenName
            icon_url = u.ProfileImageUrl |> Option.ofObj
        }
    }