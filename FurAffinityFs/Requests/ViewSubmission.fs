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
                [".classic-submission-title.information h2"; ".submission-title h2"]
                |> Seq.collect page.Html.CssSelect
                |> Seq.map (fun e -> e.InnerText())
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission title")
            href = href |> Shared.ToUri
            description =
                seq {
                    yield!
                        page.Html.CssSelect ".submission-description"
                        |> Seq.map (fun e -> e.InnerText())

                    let index1 = html.IndexOf("""<td valign="top" align="left" width="70%" class="alt1" style="padding:8px">""")
                    if index1 > -1 then
                        let substr1 = html.Substring(index1)
                        let index3 = substr1.IndexOf("</td>")
                        if index3 > -1 then
                            yield substr1.Substring(0, index3).Trim()
                    yield ""
                }
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission description")
            name =
                [".classic-submission-title.information a"; ".submission-id-sub-container a"]
                |> Seq.collect page.Html.CssSelect
                |> Seq.map (fun e -> e.InnerText())
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission username")
            download =
                ["#page-submission td.alt1 div.actions a"; ".download a"]
                |> Seq.collect page.Html.CssSelect
                |> Seq.where (fun e -> e.InnerText() = "Download")
                |> Seq.map (fun e -> e.AttributeValue "href")
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission download url")
                |> Shared.ToUri
            full =
                page.Html.CssSelect "#submissionImg"
                |> Seq.map (fun e -> e.AttributeValue "data-fullview-src")
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission full url")
                |> Shared.ToUri
            thumbnail =
                page.Html.CssSelect "#submissionImg"
                |> Seq.map (fun e -> e.AttributeValue "data-preview-src")
                |> Seq.tryHead
                |> Option.defaultWith (fun () -> failwith "Could not grab submission thumbnail url")
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
                failwith "Could not grab submission date/time"
                yield DateTime.Now
            })
            keywords =
                ["#keywords a"; ".tags a"]
                |> Seq.collect page.Html.CssSelect
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
                if page.Html.CssSelect ".rating-box.adult" |> Seq.exists (fun _ -> true) then
                    yield Rating.Adult
                if page.Html.CssSelect ".rating-box.mature"|> Seq.exists (fun _ -> true) then
                    yield Rating.Mature
                if page.Html.CssSelect ".rating-box.general"|> Seq.exists (fun _ -> true) then
                    yield Rating.General
                failwithf "Could not determine rating"
            })
        }
    }

    let ExecuteAsync credentials sid =
        AsyncExecute credentials sid
        |> Async.StartAsTask