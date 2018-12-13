namespace PillowfortFs

open System
open System.Net
open System.IO
open System.Text.RegularExpressions
open System.Threading.Tasks
open Newtonsoft.Json

type PillowfortClientException(message: string) =
    inherit ApplicationException(message)

type PillowfortClient() =
    let pillowfail str = raise (PillowfortClientException str)

    let cookies = new CookieContainer()
    let cookie_wrapper = new SingleCookieWrapper(cookies, new Uri("https://www.pillowfort.io"), "_Pillowfort_session")

    let createRequest (url: string) =
        WebRequest.CreateHttp(url, UserAgent = UserAgent.String, CookieContainer = cookies)

    let asyncOptionDefault (d: 'a) (w: Async<'a option>) = async {
        let! o = w
        return Option.defaultValue d o
    }

    member __.Cookie
        with get() = cookie_wrapper.getCookieValue() |> Option.defaultValue null
        and set (v: string) = cookie_wrapper.setCookieValue <| Option.ofObj v

    member __.AsyncWhoami = async {
        let req = createRequest "https://www.pillowfort.io/edit/username"

        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! html = sr.ReadToEndAsync() |> Async.AwaitTask
        let m = Regex.Match(html, """value="([^"]+)" name="user\[username\]" """)
        if not m.Success then
            pillowfail "Could not find username on /edit/username page (are you logged in?)"

        return m.Groups.[1].Value
    }

    member __.AsyncGetAvatar = async {
        let req = createRequest "https://www.pillowfort.io/settings"

        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! html = sr.ReadToEndAsync() |> Async.AwaitTask
        let m = Regex.Match(html, """http://s3.amazonaws.com/pillowfortmedia/settings/avatars/[^"]+""")
        return if m.Success then Some m.Value else None
    }

    member __.AsyncGetPosts username page = async {
        let url = sprintf " https://www.pillowfort.io/%s/json/?p=%d" (WebUtility.UrlEncode(username)) page
        let req = createRequest url

        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())

        let! json = sr.ReadToEndAsync() |> Async.AwaitTask
        return JsonConvert.DeserializeObject<PillowfortPostsResponse>(json)
    }

    member __.AsyncSubmitPost (post: PostRequest) = async {
        if post.tags |> Seq.exists (fun t -> t.Contains(",")) then
            invalidArg "post" "Commas are not allowed in tags"

        // Get the server-generated token for the form submission
        let! a = AuthenticityToken.get_authenticity_token "https://www.pillowfort.io/posts/new" cookies

        let authenticity_token =
            match a with
            | Some s -> s
            | None -> pillowfail "Authenticity token not found"

        // multipart separators
        let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
        let h2 = sprintf "--%s" h1
        let h3 = sprintf "--%s--" h1

        let req = createRequest "https://www.pillowfort.io/posts/create"
        req.Method <- "POST"
        req.ContentType <- sprintf "multipart/form-data; boundary=%s" h1

        do! async {
            use! reqStream = req.GetRequestStreamAsync() |> Async.AwaitTask
            use sw = new StreamWriter(reqStream)

            let w (s: string) = sw.WriteLineAsync s |> Async.AwaitTask

            let privacy =
                match post.privacy with
                | PrivacyLevel.Public -> "public"
                | PrivacyLevel.Followers -> "followers"
                | _ -> "private"

            do! w h2
            do! w "Content-Disposition: form-data; name=\"utf8\""
            do! w ""
            do! w "✓"
            do! w h2
            do! w "Content-Disposition: form-data; name=\"authenticity_token\""
            do! w ""
            do! w authenticity_token
            do! w h2
            do! w "Content-Disposition: form-data; name=\"post_to\""
            do! w ""
            do! w "current_user"
            do! w h2
            do! w "Content-Disposition: form-data; name=\"post_type\""
            do! w ""
            match post.media with
            | None -> do! w "text"
            | Some (PhotoMedia _) -> do! w "picture"
            do! w h2
            do! w "Content-Disposition: form-data; name=\"title\""
            do! w ""
            do! w post.title
            match post.media with
            | None -> ()
            | Some (PhotoMedia url) ->
                do! w h2
                do! w "Content-Disposition: form-data; name=\"picture[][file]\"; filename=\"\""
                do! w "Content-Type: application/octet-stream"
                do! w ""
                do! w ""
                do! w h2
                do! w "Content-Disposition: form-data; name=\"picture[][pic_url]\""
                do! w ""
                do! w url
                do! w h2
                do! w "Content-Disposition: form-data; name=\"picture[][row]\""
                do! w ""
                do! w "1"
                do! w h2
                do! w "Content-Disposition: form-data; name=\"picture[][col]\""
                do! w ""
                do! w "0"
            do! w h2
            do! w "Content-Disposition: form-data; name=\"content\""
            do! w ""
            do! w post.content
            do! w h2
            do! w "Content-Disposition: form-data; name=\"tags\""
            do! w ""
            do! w (String.concat "," post.tags)
            do! w h2
            do! w "Content-Disposition: form-data; name=\"privacy\""
            do! w ""
            do! w privacy
            if post.commentable then
                do! w h2
                do! w "Content-Disposition: form-data; name=\"commentable\""
                do! w ""
                do! w "on"
            if post.rebloggable then
                do! w h2
                do! w "Content-Disposition: form-data; name=\"rebloggable\""
                do! w ""
                do! w "on"
            if post.nsfw then
                do! w h2
                do! w "Content-Disposition: form-data; name=\"nsfw\""
                do! w ""
                do! w "on"
            do! w h2
            do! w "Content-Disposition: form-data; name=\"commit\""
            do! w ""
            do! w "Submit"
            do! w h3
            do! w ""
        }
        
        use! resp = req.AsyncGetResponse()
        return ignore resp
    }

    member __.AsyncDeletePost id = async {
        let req = sprintf "https://www.pillowfort.io/posts/%d/destroy" id |> createRequest
        req.Method <- "POST"
        use! resp = req.AsyncGetResponse()
        return ignore resp
    }

    member __.AsyncSignout = async {
        let req = createRequest "https://www.pillowfort.io/signout"
        use! resp = req.AsyncGetResponse()
        return ignore resp
    }
    
    member this.WhoamiAsync() = Async.StartAsTask this.AsyncWhoami
    member this.GetAvatarAsync() = Async.StartAsTask (this.AsyncGetAvatar |> asyncOptionDefault null)
    member this.GetPostsAsync username page = Async.StartAsTask (this.AsyncGetPosts username page)
    member this.SubmitPostAsync post = Async.StartAsTask (this.AsyncSubmitPost post) :> Task
    member this.DeletePostAsync id = Async.StartAsTask (this.AsyncDeletePost id) :> Task
    member this.SignoutAsync() = Async.StartAsTask this.AsyncSignout :> Task