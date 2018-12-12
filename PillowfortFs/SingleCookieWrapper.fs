namespace PillowfortFs

open System
open System.Net

type internal SingleCookieWrapper(c: CookieContainer, u: Uri, name: string) =
    member __.getCookie() =
        c.GetCookies(u).[name] |> Option.ofObj

    member this.getCookieValue() =
        match this.getCookie() with
        | Some c -> Some c.Value
        | None -> None
    member this.setCookieValue (value: string option) =
        match this.getCookie() with
        | Some c -> c.Expires <- DateTime.Now.AddDays(-1.0)
        | None -> ()
        match value with
        | Some s -> c.Add(u, new Cookie(name, s))
        | None -> ()

    member this.Cookie
        with get() = this.getCookieValue() |> Option.defaultValue null
        and set (v: string) = this.setCookieValue <| Option.ofObj v