namespace SourceWrappers

open ArtworkSourceSpecification
open System.Net
open Tweetinvi.Models
open Tweetinvi.Models.Entities
open Tweetinvi
open Tweetinvi.Parameters
open FSharp.Control
open System

type TwitterPostWrapper(tweet: ITweet, media: IMediaEntity option, twitterClient: ITwitterClient) =
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
        member this.Timestamp = tweet.CreatedAt.UtcDateTime
        member this.ViewURL = tweet.Url
    interface IDeletable with
        member this.SiteName = "Twitter"
        member this.DeleteAsync () = twitterClient.Tweets.DestroyTweetAsync tweet

type TwitterPhotoPostWrapper(tweet: ITweet, media: IMediaEntity, twitterClient: ITwitterClient) =
    inherit TwitterPostWrapper(tweet, Some media, twitterClient)
    interface IRemotePhotoPost with
        member this.ImageURL = sprintf "%s:large" media.MediaURLHttps
        member this.ThumbnailURL = sprintf "%s:thumb" media.MediaURLHttps

type TwitterAnimatedGifPostWrapper(tweet: ITweet, media: IMediaEntity, twitterClient: ITwitterClient) =
    inherit TwitterPostWrapper(tweet, Some media, twitterClient)
    interface IRemoteVideoPost with
        member this.VideoURL =
            media.VideoDetails.Variants
            |> Seq.where (fun v -> v.ContentType = "video/mp4")
            |> Seq.sortByDescending (fun v -> v.Bitrate)
            |> Seq.map (fun v -> v.URL)
            |> Seq.head
        member this.ThumbnailURL = media.MediaURLHttps

type TwitterSourceWrapper(twitterClient: ITwitterClient, photosOnly: bool) =
    inherit AsyncSeqWrapper()

    let user_task = lazy(twitterClient.Users.GetAuthenticatedUserAsync())

    let getUser = user_task.Force() |> Async.AwaitTask

    let wrap t = TwitterPostWrapper (t, None, twitterClient) :> IPostBase
    let wrapPhoto t m = TwitterPhotoPostWrapper (t, m, twitterClient) :> IPostBase
    let wrapAnimatedGif t m = TwitterAnimatedGifPostWrapper (t, m, twitterClient) :> IPostBase

    let isPhoto (m: IMediaEntity) = m.MediaType = "photo"
    let isAnimatedGif (m: IMediaEntity) = m.MediaType = "animated_gif"

    member val BatchSize: int = 20 with get, set

    override __.Name =
        if photosOnly then
            "Twitter (images)"
        else
            "Twitter (text + images)"

    override this.FetchSubmissionsInternal() = asyncSeq {
        let mutable maxId = Nullable<int64>()
        let mutable more = true

        while more do
            let! user = getUser

            let ps = new GetUserTimelineParameters(user)
            ps.ExcludeReplies <- false
            ps.IncludeEntities <- true
            ps.IncludeRetweets <- true
            ps.MaxId <- maxId
            ps.PageSize <- this.BatchSize

            let! tweets = twitterClient.Timelines.GetUserTimelineAsync ps |> Async.AwaitTask

            for t in tweets do
                if not t.IsRetweet then
                    let photos = t.Media |> Seq.filter isPhoto
                    let gifs =
                        if photosOnly
                        then Seq.empty
                        else t.Media |> Seq.filter isAnimatedGif
                    if Seq.isEmpty photos && Seq.isEmpty gifs then
                        if not photosOnly then
                            yield wrap t
                    else
                        for m in photos do
                            yield wrapPhoto t m
                        for m in gifs do
                            yield wrapAnimatedGif t m
                            
            more <- not (Seq.isEmpty tweets)
            if more then
                maxId <- Nullable ((tweets |> Seq.map (fun t -> t.Id) |> Seq.min) - 1L)
    }

    override this.FetchUserInternal() = async {
        let! u = getUser
        return {
            username = u.ScreenName
            icon_url = u.ProfileImageUrl |> Option.ofObj
        }
    }