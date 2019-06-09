namespace PixivUploader

open System
open System.Net
open System.IO
open System.Text
open System.Text.RegularExpressions

module Uploader =
    let BaseUri = new Uri("https://www.pixiv.net/")
    let ToUri (path: string) = new Uri(BaseUri, path)

    let GetCookiesFor (credentials: IPixivSession) =
        let c = new CookieContainer()
        c.Add(BaseUri, new Cookie("PHPSESSID", credentials.PHPSESSID))
        c

    let CreateRequest (credentials: IPixivSession) (uri: Uri) =
        WebRequest.CreateHttp(uri, UserAgent = "PixivUploader/0.1 (https://github.com/libertyernie/CrosspostSharp)", CookieContainer = GetCookiesFor credentials)

    let AsyncGetHtml (credentials: IPixivSession) (path: string) = async {
        let req = path |> ToUri |> CreateRequest credentials
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        return! sr.ReadToEndAsync() |> Async.AwaitTask
    }

    let UploadIllustration (credentials: IPixivSession) (parameters: NewSubmission) = async {
       if parameters.tag |> Seq.exists (fun p -> p.Contains(" ")) then
           failwithf "Spaces in tags not allowed"

       let! html = AsyncGetHtml credentials "/upload.php"
       let rmatch = Regex.Match(html, """<input type="hidden" name="tt" value="([^"]+)">""")
       let tt = rmatch.Groups.[1].Value

       // multipart separators
       let h1 = sprintf "-----------------------------%d" DateTime.UtcNow.Ticks
       let h2 = sprintf "--%s" h1
       let h3 = sprintf "--%s--" h1

       let req2 = "/upload.php" |> ToUri |> CreateRequest credentials
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
           w (sprintf "Content-Disposition: form-data; name=\"files[]\"; filename=\"blob\"")
           w (sprintf "Content-Type: application/octet-stream")
           w ""
           ms.Flush()
           ms.Write(parameters.data, 0, parameters.data.Length)
           ms.Flush()
           w ""
           w h3

           use! reqStream = req2.GetRequestStreamAsync() |> Async.AwaitTask
           ms.Position <- 0L
           do! ms.CopyToAsync(reqStream) |> Async.AwaitTask
       }

       use! resp = req2.AsyncGetResponse()
       use sr = new StreamReader(resp.GetResponseStream())
       let! html = sr.ReadToEndAsync() |> Async.AwaitTask
       ignore html
}