namespace DeviantArtFs.Data

open DeviantArtFs
open FSharp.Data

type TosResponse = JsonProvider<"""{
    "text": "html_content"
}""">

module Tos =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/tos"
        let! json = dafs.asyncRead req
        let obj = TosResponse.Parse json
        return obj.Text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask