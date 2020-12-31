namespace FurAffinityFs

module internal Shared =
    open System
    open System.Net
    open System.IO
    open FSharp.Data

    let BaseUri = new Uri("https://www.furaffinity.net/")

    // String processing
    let (|HasPrefix|_|) (p: string) (s: string) =
        if s.StartsWith(p) then
            Some (s.Substring(p.Length))
        else
            None

    let (|CanParseInt|_|) (str: string) =
        match System.Int32.TryParse str with
        | (true, int) -> Some int
        | _ -> None

    let StripTilde (str: string) =
        match str with
        | HasPrefix "~" rest -> rest
        | _ -> str

    // Other helper functions
    let ToUri (path: string) = new Uri(BaseUri, path)

    let ExtractAuthenticityToken (html: HtmlDocument) =
        let m =
            html.CssSelect("form[name=myform] input[name=key]")
            |> Seq.map (fun e -> e.AttributeValue("value"))
            |> Seq.tryHead
        match m with
            | Some token -> token
            | None -> raise (FurAffinityClientException "Input \"key\" not found in HTML")

    let GetCookiesFor (credentials: IFurAffinityCredentials) =
        let c = new CookieContainer()
        c.Add(BaseUri, new Cookie("a", credentials.A))
        c.Add(BaseUri, new Cookie("b", credentials.B))
        c

    let CreateRequest (credentials: IFurAffinityCredentials) (uri: Uri) =
        WebRequest.CreateHttp(uri, UserAgent = "Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:68.0) Gecko/20100101 Firefox/68.0", CookieContainer = GetCookiesFor credentials)

    let AsyncGetHtml (credentials: IFurAffinityCredentials) (path: string) = async {
        let req = path |> ToUri |> CreateRequest credentials
        use! resp = req.AsyncGetResponse()
        use sr = new StreamReader(resp.GetResponseStream())
        return! sr.ReadToEndAsync() |> Async.AwaitTask
    }