namespace PillowfortFs

open System
open System.IO
open System.Net
open System.Text.RegularExpressions

module PillowfortClientFactory =
    open System.Text

    let ua = "PillowfortFs/0.1 (https://github.com/libertyernie)"

    let get_authenticity_token cookies = async {
        let req = WebRequest.CreateHttp("https://pillowfort.io/users/sign_in", CookieContainer = cookies, UserAgent = ua)
        
        use! response = req.AsyncGetResponse()
        use sr = new StreamReader(response.GetResponseStream())
        let! contents = sr.ReadToEndAsync() |> Async.AwaitTask
        
        let m = Regex.Match(contents, """<input type="hidden" name="authenticity_token" value="([^"]+)" """)

        if not m.Success then
            failwith "No authenticity_token found"

        return m.Groups.[1].Value
    }

    let login username password = async {
        // Create a container to keep cookies between requests
        let cookies = new CookieContainer()

        // Get the server-generated token for the form submission
        let! authenticity_token = get_authenticity_token cookies
        
        let req = WebRequest.CreateHttp("https://pillowfort.io/users/sign_in", CookieContainer = cookies, UserAgent = ua, Method = "POST", ContentType = "application/x-www-form-urlencoded")
        
        // Write to request body
        // I do this in a separate scope so the stream is automatically flushed and can't be used later.
        do! async {
            let parameters = [
                sprintf "%s=%s" "utf8" (WebUtility.UrlEncode "✓")
                sprintf "%s=%s" "authenticity_token" (WebUtility.UrlEncode authenticity_token)
                sprintf "%s=%s" "user[email]" (WebUtility.UrlEncode username)
                sprintf "%s=%s" "user[password]" (WebUtility.UrlEncode password)
                sprintf "%s=%s" "user[remember_me]" "0"
                "commit=Log+in"
            ]
            let requestBody = String.concat "&" parameters
            
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask

            use sw = new StreamWriter(reqStream, Encoding.UTF8)
            do! requestBody |> sw.WriteLineAsync |> Async.AwaitTask
        }
        
        // Send request and read from response body
        use! resp = req.AsyncGetResponse()
        if resp.ResponseUri.AbsolutePath <> "/" then
            failwith "Did not redirect to / as expected"

        return cookies.GetCookies(new Uri("https://pillowfort.io")).["_Pillowfort_session"].Value
    }
