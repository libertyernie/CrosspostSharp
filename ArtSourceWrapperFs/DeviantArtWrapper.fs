namespace ArtSourceWrapperFs

open ArtSourceWrapper
open DeviantartApi.Objects
open DeviantartApi.Requests
open System
open System.Threading.Tasks

type public DeviantArtWrapper() =
    inherit SiteWrapper<DeviantArtSubmissionWrapper, int32>()
    
    let getUser =
        let workflow = async {
            let! result = Async.AwaitTask <| User.WhoAmIRequest().ExecuteAsync()

            if result.IsError then
                failwith result.ErrorText
            else if result.Result.Error |> (not << String.IsNullOrEmpty) then
                failwith result.Result.ErrorDescription

            return result.Result
        }
        let mutable t: Task<User> = null
        fun () ->
            if isNull t then
                t <- Async.StartAsTask workflow
            Async.AwaitTask t

    override this.WrapperName = "DeviantArt"
    override this.SubmissionsFiltered = true

    override this.MinBatchSize = 1
    override this.MaxBatchSize = 24
    override val BatchSize = 24 with get, set

    override this.WhoamiAsync() = Async.StartAsTask <| async {
        let! user = getUser()
        return user.Username
    }

    override this.GetUserIconAsync(size) = Async.StartAsTask <| async {
        let! user = getUser()
        return user.UserIconUrl.AbsoluteUri
    }

    override this.InternalFetchAsync(startPosition, count) = Async.StartAsTask <| async {
        let skip = if startPosition.HasValue then startPosition.Value else 0
        let take = count |> max this.MinBatchSize |> min this.MaxBatchSize
    
        let execute (r: Request<'a>) = r.ExecuteAsync() |> Async.AwaitTask
        
        let galleryRequest = Gallery.AllRequest()
        galleryRequest.Limit <- take |> uint32 |> Nullable
        galleryRequest.Offset <- skip |> uint32 |> Nullable

        let! galleryResponse = galleryRequest |> execute

        if galleryResponse.IsError then
            failwith galleryResponse.ErrorText
        else if galleryResponse.Result.Error |> (not << String.IsNullOrEmpty) then
            failwith galleryResponse.Result.ErrorDescription
            
        let deviations =
            galleryResponse.Result.Results
            |> Seq.filter (fun d -> d.Content |> (not << isNull))

        let! result =
            deviations
            |> Seq.map (fun d -> d.DeviationId)
            |> Deviation.MetadataRequest
            |> execute

        if result.IsError then
            failwith result.ErrorText
        else if result.Result.Error |> (not << String.IsNullOrEmpty) then
            failwith result.Result.ErrorDescription

        let noneToNull x =
            match x with
            | Some s -> s
            | None -> null

        let metadataForId id =
            result.Result.Metadata
            |> Seq.tryFind (fun m -> m.DeviationId = id)
        
        let wrappers =
            deviations
            |> Seq.map (fun d -> DeviantArtSubmissionWrapper(d, d.DeviationId |> metadataForId |> noneToNull))
            
        return InternalFetchResult(wrappers, skip + take, not galleryResponse.Result.HasMore)
    }

    static member LogoutAsync() = Async.StartAsTask <| async {
        let! a = DeviantartApi.Login.LogoutAsync(DeviantartApi.Requester.AccessToken) |> Async.AwaitTask
        let! b = DeviantartApi.Login.LogoutAsync(DeviantartApi.Requester.RefreshToken) |> Async.AwaitTask
        return a && b
    }
