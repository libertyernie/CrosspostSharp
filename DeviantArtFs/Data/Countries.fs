namespace DeviantArtFs.Data

open DeviantArtFs
open FSharp.Data

type CountriesResponse = JsonProvider<"""{
    "results": [
        {
            "countryid": 1,
            "name": "United States"
        },
        {
            "countryid": 2,
            "name": "Canada"
        }
    ]
}""">

module Countries =
    let AsyncExecute token = async {
        let req = dafs.createRequest token "https://www.deviantart.com/api/v1/oauth2/data/countries"
        let! json = dafs.asyncRead req
        let obj = CountriesResponse.Parse json
        return obj.Results
            |> Seq.map (fun r -> (r.Countryid, r.Name))
            |> dict
    }

    let ExecuteAsync token = AsyncExecute token |> Async.StartAsTask