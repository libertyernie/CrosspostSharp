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
        let doc = HtmlDocument.Parse html
        return
            doc.CssSelect "select[name=timezone] option[selected]"
            |> Seq.map (fun e -> e.InnerText())
            |> Seq.head
            |> GetTimeZoneFromFurAffinityName
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