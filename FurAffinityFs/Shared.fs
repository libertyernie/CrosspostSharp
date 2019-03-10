namespace FurAffinityFs

module internal Shared =
    open System
    open System.Text.RegularExpressions
    open System.Net

    let BaseUri = new Uri("https://www.furaffinity.net/")

    let ExtractAuthenticityToken html =
        let m = Regex.Match(html, """<input type="hidden" name="key" value="([^"]+)".""")

        if m.Success
            then m.Groups.[1].Value
            else raise (FurAffinityClientException "Input \"key\" not found in HTML")

    let GetCookiesFor (credentials: IFurAffinityCredentials) =
        let c = new CookieContainer()
        c.Add(BaseUri, new Cookie("a", credentials.A))
        c.Add(BaseUri, new Cookie("b", credentials.B))
        c

    let CreateRequest (credentials: IFurAffinityCredentials) (url: string) =
        WebRequest.CreateHttp(new Uri(BaseUri, url), UserAgent = "FurAffinityFs/0.2 (https://github.com/libertyernie/CrosspostSharp)", CookieContainer = GetCookiesFor credentials)

    let AsyncOptionDefault (d: 'a) (w: Async<'a option>) = async {
        let! o = w
        return Option.defaultValue d o
    }