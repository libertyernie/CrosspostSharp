namespace DeviantArtFs.Data

open DeviantArtFs
open FSharp.Data

type PrivacyResponse = JsonProvider<"""{
    "text": "html_content"
}""">

module Privacy =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/privacy"
        let! json = dafs.asyncRead req
        let obj = PrivacyResponse.Parse json
        return obj.Text
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask