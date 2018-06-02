namespace ArtSourceWrapperFs

open ArtSourceWrapper
open DeviantartApi.Objects
open DeviantartApi.Requests
open System
open System.Threading.Tasks

type internal DeviantArtInternalWrapper() =
    inherit AsynchronousCachedEnumerable<Deviation, uint32>()

    override val BatchSize = 24 with get, set
    override this.MinBatchSize = 1
    override this.MaxBatchSize = 24

    override this.InternalFetchAsync(startPosition, count) =
        Async.StartAsTask (async {
            let position = if startPosition.HasValue then startPosition.Value else uint32 0
            
            let galleryRequest = Gallery.AllRequest()
            galleryRequest.Limit <- count |> max this.MinBatchSize |> min this.MaxBatchSize |> uint32 |> Nullable
            galleryRequest.Offset <- position |> Nullable

            let! galleryResponse = Async.AwaitTask <| galleryRequest.GetNextPageAsync()

            if galleryResponse.IsError then
                failwith galleryResponse.ErrorText
            else if galleryResponse.Result.Error |> (not << String.IsNullOrEmpty) then
                failwith galleryResponse.Result.ErrorDescription

            return InternalFetchResult(galleryResponse.Result.Results, position + uint32 galleryResponse.Result.Results.Count, not galleryResponse.Result.HasMore)
        })

type public DeviantArtWrapper() =
    inherit SiteWrapper<DeviantArtSubmissionWrapper, int32>()

    let idWrapper = DeviantArtInternalWrapper()
    let maxBatch = min 50 idWrapper.MaxBatchSize

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
    override this.MaxBatchSize = maxBatch
    override val BatchSize = maxBatch with get, set

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
        
        idWrapper.BatchSize <- this.BatchSize

        let countWanted = skip + take
        while Seq.length idWrapper.Cache < countWanted && not idWrapper.IsEnded do
            do! idWrapper.FetchAsync() |> Async.AwaitTask |> Async.Ignore

        let skipSafe num = 
            Seq.zip (Seq.initInfinite id)
            >> Seq.skipWhile (fun (i, _) -> i < num)
            >> Seq.map snd

        let deviations =
            idWrapper.Cache
            |> skipSafe skip
            |> Seq.truncate take
            |> Seq.filter (fun d -> d.Content |> (not << isNull))
    
        let execute (r: Request<'a>) = r.ExecuteAsync() |> Async.AwaitTask

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

        let moreIdsCached =
            idWrapper.Cache
            |> skipSafe skip
            |> skipSafe take
            |> (not << Seq.isEmpty)

        let moreIdsAvailable = not idWrapper.IsEnded

        return InternalFetchResult(wrappers, skip + take, (not moreIdsCached && not moreIdsAvailable))
    }

    static member LogoutAsync() = Async.StartAsTask <| async {
        let! a = DeviantartApi.Login.LogoutAsync(DeviantartApi.Requester.AccessToken) |> Async.AwaitTask
        let! b = DeviantartApi.Login.LogoutAsync(DeviantartApi.Requester.RefreshToken) |> Async.AwaitTask
        return a && b
    }
