// Learn more about F# at http://fsharp.org

open System
open System.IO
open System.Text
open PixivUploader

type IPixivCredentials =
    abstract member PHPSESSID: string
    abstract member device_token: string

module internal Shared =
    open System
    open System.Net
    open System.IO
    open FSharp.Data

    let BaseUri = new Uri("https://www.pixiv.net/")

    // String processing
    let (|HasPrefix|_|) (p: string) (s: string) =
        if s.StartsWith(p) then
            Some (s.Substring(p.Length))
        else
            None

    let StripTilde (str: string) =
        match str with
        | HasPrefix "~" rest -> rest
        | _ -> str

    // Other helper functions
    let ToUri (path: string) = new Uri(BaseUri, path)

    let GetCookiesFor (credentials: IPixivCredentials) =
        let c = new CookieContainer()
        c.Add(BaseUri, new Cookie("PHPSESSID", credentials.PHPSESSID))
        c.Add(BaseUri, new Cookie("device_token", credentials.device_token))
        c

    let CreateRequest (credentials: IPixivCredentials) (uri: Uri) =
        WebRequest.CreateHttp(uri, UserAgent = "FurAffinityFs/0.2 (https://github.com/libertyernie/CrosspostSharp)", CookieContainer = GetCookiesFor credentials)

    let AsyncGetHtml (credentials: IPixivCredentials) (path: string) = async {
        let req = path |> ToUri |> CreateRequest credentials
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        return! sr.ReadToEndAsync() |> Async.AwaitTask
    }

let AsyncExecute (credentials: IPixivCredentials) (parameters: IPixivUploadParameters) (data: byte[]) = async {
    if parameters.tag |> Seq.exists (fun p -> p.Contains(" ")) then
        failwithf "Spaces in tags not allowed"

    let! html = Shared.AsyncGetHtml credentials "/upload.php"
    let rmatch = System.Text.RegularExpressions.Regex.Match(html, """<input type="hidden" name="tt" value="([^"]+)">""")
    let tt = rmatch.Groups.[1].Value

    // multipart separators
    let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
    let h2 = sprintf "--%s" h1
    let h3 = sprintf "--%s--" h1

    let req2 = "/upload.php" |> Shared.ToUri |> Shared.CreateRequest credentials
    req2.Method <- "POST"
    req2.ContentType <- sprintf "multipart/form-data; boundary=%s" h1

    do! async {
        use ms = new MemoryStream()
        let w (s: string) =
            let bytes = Encoding.UTF8.GetBytes(sprintf "%s\n" s)
            ms.Write(bytes, 0, bytes.Length)
        
        w h2
        w "Content-Disposition: form-data; name=\"mode\""
        w ""
        w "upload"
        w h2
        w "Content-Disposition: form-data; name=\"uptype\""
        w ""
        w "illust"
        w h2
        w "Content-Disposition: form-data; name=\"tt\""
        w ""
        w tt
        w h2
        w "Content-Disposition: form-data; name=\"book_style\""
        w ""
        w "0"
        w h2
        w "Content-Disposition: form-data; name=\"title\""
        w ""
        w parameters.title
        w h2
        w "Content-Disposition: form-data; name=\"comment\""
        w ""
        w parameters.comment
        w h2
        w "Content-Disposition: form-data; name=\"tag\""
        w ""
        w (parameters.tag |> String.concat " ")
        //w h2
        //w "Content-Disposition: form-data; name=\"suggested_tags\""
        //w ""
        //w ""
        w h2
        w "Content-Disposition: form-data; name=\"x_restrict_sexual\""
        w ""
        w (parameters.x_restrict_sexual.ToString "d")
        w h2
        w "Content-Disposition: form-data; name=\"sexual\""
        w ""
        w (if parameters.sexual = SexualContent.Implicit then "implicit" else "")
        w h2
        w "Content-Disposition: form-data; name=\"restrict\""
        w ""
        w (parameters.restrict.ToString "d")
        w h2
        //w "Content-Disposition: form-data; name=\"qropen\""
        //w ""
        //w "1"
        //w h2
        //w "Content-Disposition: form-data; name=\"rating\""
        //w ""
        //w "1"
        //w h2
        w (sprintf "Content-Disposition: form-data; name=\"files[]\"; filename=\"blob\"")
        w (sprintf "Content-Type: application/octet-stream")
        w ""
        ms.Flush()
        ms.Write(data, 0, data.Length)
        ms.Flush()
        w ""
        w h3

        use! reqStream = req2.GetRequestStreamAsync() |> Async.AwaitTask
        ms.Position <- 0L
        do! ms.CopyToAsync(reqStream) |> Async.AwaitTask
    }

    let! html = async {
        use! resp = req2.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        let! html = sr.ReadToEndAsync() |> Async.AwaitTask
        return html
    }

    printfn "%s" html
}

[<EntryPoint>]
let main argv =
    printfn "Hello World from F#!"
    let c = {
        new IPixivCredentials with
            member __.PHPSESSID = "4256133_33afe881f93dff6c02364258473e40df"
            member __.device_token = "f7b4028eaa50ff78c1124a79bc9860fc"
    }
    let s = {
        new IPixivUploadParameters with
            member __.title = "Test image 3"
            member __.comment = "the description"
            member __.tag = Seq.singleton "adfggfdgdf"
            member __.x_restrict_sexual = ViewingRestriction.AllAges
            member __.sexual = SexualContent.None
            member __.lo = false
            member __.furry = true
            member __.bl = false
            member __.gl = false
            member __.restrict = PrivacySettings.Private
            member __.original = true
    }
    File.ReadAllBytes """C:\Users\admin\Pictures\600x600_by_doodle_shark-daqzb49.png""" |> AsyncExecute c s |> Async.RunSynchronously
    0 // return an integer exit code
