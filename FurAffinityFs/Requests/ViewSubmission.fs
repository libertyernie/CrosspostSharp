namespace FurAffinityFs.Requests

module ViewSubmission =
    open FSharp.Data
    open FurAffinityFs
    open FurAffinityFs.Models
    open System
    open System.Text.RegularExpressions

    type internal ViewSubmissionHtml = HtmlProvider<"https://www.furaffinity.net/view/30761803/">

    let AsyncExecute (credentials: IFurAffinityCredentials) (sid: int) = async {
        let href = sprintf "/view/%d/" sid
        let! html = Shared.AsyncGetHtml credentials href
        let page = ViewSubmissionHtml.Parse html
        return {
            title =
                page.Html.CssSelect ".classic-submission-title.information h2"
                |> Seq.map (fun e -> e.InnerText())
                |> Seq.head
            href = href |> Shared.ToUri
            description = Seq.head (seq {
                let index1 = html.IndexOf("""<td valign="top" align="left" width="70%" class="alt1" style="padding:8px">""")
                if index1 > -1 then
                    let substr1 = html.Substring(index1)
                    let index3 = substr1.IndexOf("</td>")
                    if index3 > -1 then
                        yield substr1.Substring(0, index3).Trim()
                yield ""
            })
            name =
                page.Html.CssSelect ".classic-submission-title.information a"
                |> Seq.map (fun e -> e.InnerText())
                |> Seq.head
            download =
                page.Html.CssSelect "#page-submission td.alt1 div.actions a"
                |> Seq.where (fun e -> e.InnerText() = "Download")
                |> Seq.map (fun e -> e.AttributeValue "href")
                |> Seq.head
                |> Shared.ToUri
            full =
                page.Html.CssSelect "#submissionImg"
                |> Seq.map (fun e -> e.AttributeValue "data-fullview-src")
                |> Seq.head
                |> Shared.ToUri
            thumbnail =
                page.Html.CssSelect "#submissionImg"
                |> Seq.map (fun e -> e.AttributeValue "data-preview-src")
                |> Seq.head
                |> Shared.ToUri
            date = Seq.head (seq {
                let regex = new Regex("([A-Za-z]+) ([0-9]+)[^ ]+? ([0-9][0-9][0-9][0-9]) ([0-9][0-9]):([0-9][0-9]) (AM|PM)")
                for e in page.Html.CssSelect ".popup_date" do
                    for s in [e.AttributeValue "title"; e.InnerText()] do
                        let m = regex.Match s
                        if m.Success then
                            let month = m.Groups.[1].Value
                            let day = m.Groups.[2].Value
                            let year = m.Groups.[3].Value
                            let hour = m.Groups.[4].Value
                            let minute = m.Groups.[5].Value
                            let ampm = m.Groups.[6].Value
                            let (ok, dt) =
                                sprintf "%s %s %s %s:%s %s" month day year hour minute ampm
                                |> DateTime.TryParse
                            if ok then yield dt
                yield DateTime.Now
            })
            keywords =
                page.Html.CssSelect "#keywords a"
                |> Seq.map (fun e -> e.InnerText())
            rating = Seq.head (seq {
                let ratings = [
                    ("General rating", Rating.General);
                    ("Mature rating", Rating.Mature);
                    ("Adult rating", Rating.Adult)
                ]
                for (alt, r) in ratings do
                    if page.Html.CssSelect "img" |> Seq.exists (fun e -> e.AttributeValue "alt" = alt) then
                        yield r
                failwithf "Could not determine rating"
            })
        }
    }

    let ExecuteAsync credentials sid =
        AsyncExecute credentials sid
        |> Async.StartAsTask