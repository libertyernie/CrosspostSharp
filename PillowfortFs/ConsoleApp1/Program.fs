// Learn more about F# at http://fsharp.org

open System

[<EntryPoint>]
let main argv =
    let c = PillowfortFs.PillowfortClientFactory.AsyncLogin "user@example.com" "password" |> Async.RunSynchronously
    let n = c.AsyncGetAvatar |> Async.RunSynchronously
    printfn "%s" n
    c.AsyncSignout |> Async.RunSynchronously
    0 // return an integer exit code
