module AsyncHelpers
    open DeviantartApi.Objects
    open System

    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }
    
    let processDeviantArtError<'a when 'a :> BaseObject> (resp: DeviantartApi.Requests.Response<'a>) =
        if (resp.IsError) then failwith resp.ErrorText
        if (resp.Result.Error |> String.IsNullOrEmpty |> not) then failwith resp.Result.Error
        resp.Result
