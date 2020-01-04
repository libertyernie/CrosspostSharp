namespace FurAffinityFs.Requests

module GetTimeZone =
    open FSharp.Data
    open FurAffinityFs
    open System

    let GetTimeZoneFromFurAffinityName (text: string) =
        try
            let n = text.Substring(text.IndexOf("] ") + 2).Replace("T ime", "Time")
            let found =
                if n = "Greenwich Mean Time" then
                    TimeZoneInfo.Utc
                else if n = "International Date Line West" then
                    TimeZoneInfo.FindSystemTimeZoneById "Dateline Standard Time"
                else
                    TimeZoneInfo.FindSystemTimeZoneById n
            Some found
        with
        | :? TimeZoneNotFoundException -> None

    let AsyncExecute (credentials: IFurAffinityCredentials) = async {
        let! html = Shared.AsyncGetHtml credentials "/controls/settings/"
        let regex = System.Text.RegularExpressions.Regex """<option selected="selected" value=".....">([^<]+)</option>"""
        let m = regex.Match html
        return
            if m.Success then
                GetTimeZoneFromFurAffinityName m.Groups.[1].Value
            else
                failwith "Cannot pull time zone from FurAffinity HTML"
    }

    let AsyncExecuteWithDefault credentials def = async {
        let! result = AsyncExecute credentials
        return result |> Option.defaultValue def
    }

    let ExecuteAsync credentials =
        AsyncExecute credentials
        |> Async.StartAsTask

    let ExecuteAsyncWithDefault credentials def =
        AsyncExecuteWithDefault credentials def
        |> Async.StartAsTask