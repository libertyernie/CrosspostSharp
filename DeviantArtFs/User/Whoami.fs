namespace DeviantArtFs.User

open DeviantArtFs
open FSharp.Data

type WhoamiRepsonse = JsonProvider<"""{
    "userid": "CAFD9087-C6EF-2F2C-183B-A658AE61FB95",
    "username": "verycoolusername",
    "usericon": "https://a.deviantart.net/avatars/default.gif",
    "type": "regular"
}""">

module Whoami =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/user/whoami"
        let! json = dafs.asyncRead req
        return WhoamiRepsonse.Parse json
    }

    let GetUsernameAsync token = AsyncExecute token |> dafs.whenDone (fun u -> u.Username) |> Async.StartAsTask
    let GetUserIconAsync token = AsyncExecute token |> dafs.whenDone (fun u -> u.Usericon) |> Async.StartAsTask