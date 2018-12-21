namespace DeviantArtFs.Data

open DeviantArtFs
open FSharp.Data

type SubmissionResponse = JsonProvider<"""{
    "text": "html_content"
}""">

module Submission =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/submission"
        let! json = dafs.asyncRead req
        let obj = SubmissionResponse.Parse json
        return obj.Text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask