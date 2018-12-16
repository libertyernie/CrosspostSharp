namespace DeviantArtFs

open FSharp.Data
open System.Net
open System.IO
open System.Collections.Generic
open System

type internal TokenResponse = JsonProvider<""" {
  "expires_in": 3600,
  "status": "success",
  "access_token": "Alph4num3r1ct0k3nv4lu3",
  "token_type": "Bearer",
  "refresh_token": "3ul4vn3k0tc1r3mun4hplA",
  "scope": "basic"
} """>

type DeviantArtAuth(client_id: int, client_secret: string) =
    let whenDone (f: 'a -> 'b) (workflow: Async<'a>) = async {
        let! result = workflow
        return f result
    }

    let UserAgent = "DeviantArtFs/0.1 (https://github.com/libertyernie/CrosspostSharp"

    let BuildForm (dict: IDictionary<string, string>) =
        let parameters = seq {
            for p in dict do
                if isNull p.Value then
                    failwithf "Null values in form not allowed"
                let key = WebUtility.UrlEncode p.Key
                let value = WebUtility.UrlEncode (p.Value.ToString())
                yield sprintf "%s=%s" key value
        }
        String.concat "&" parameters

    member __.AsyncRefresh (refresh_token: string) = async {
        if isNull refresh_token then
            nullArg "refresh_token"

        let req = WebRequest.CreateHttp "https://www.deviantart.com/oauth2/token"
        req.UserAgent <- UserAgent
        req.Method <- "POST"
        req.ContentType <- "application/x-www-form-urlencoded"
            
        do! async {
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(reqStream)
            do!
                [
                    ("client_id", client_id.ToString());
                    ("client_secret", client_secret);
                    ("grant_type", "refresh_token");
                    ("refresh_token", refresh_token)
                ]
                |> dict
                |> BuildForm
                |> sw.WriteAsync
                |> Async.AwaitTask
        }

        use! resp = req.GetResponseAsync() |> Async.AwaitTask
        use sr = new StreamReader(resp.GetResponseStream())
        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        let obj = TokenResponse.Parse json
        if obj.Status <> "success" then
            failwithf "An unknown error occured"
        if obj.TokenType <> "Bearer" then
            failwithf "token_type was not Bearer"
        return {
            AccessToken = obj.AccessToken
            RefreshToken = obj.RefreshToken
            ExpiresAt = DateTimeOffset.UtcNow.AddSeconds (float obj.ExpiresIn)
        }
    }

    member this.RefreshAsync t = this.AsyncRefresh t |> Async.StartAsTask