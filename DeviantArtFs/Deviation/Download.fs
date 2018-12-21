namespace DeviantArtFs.Deviation

open DeviantArtFs
open FSharp.Data
open System

type DownloadResponse = JsonProvider<"""{
    "src": "https://www.example.com",
    "width": 640,
    "height": 480,
    "filesize": 10000
}""">

module Download =
    let AsyncExecute token (deviationid: Guid) = async {
        let req = sprintf "https://www.deviantart.com/api/v1/oauth2/deviation/download/%O" deviationid |> dafs.createRequest token
        let! json = dafs.asyncRead req
        return DownloadResponse.Parse json
    }