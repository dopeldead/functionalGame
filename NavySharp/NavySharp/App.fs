module App

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Web
open Suave.RequestErrors

let browse =
    request (fun r ->
        match r.queryParam "genre" with
        | Choice1Of2 genre -> OK (sprintf "Genre: %s" genre)
        | Choice2Of2 msg -> BAD_REQUEST msg)

let details =
    choose [
        GET >=> warbler (fun _ -> OK "GET")

        POST >=> warbler (fun _ -> OK "POST")
    ]



let routes = 
    choose [
        path "/" >=> (OK "Home")
        path "/store" >=> (OK "Store")
        path "/store/browse" >=> browse
        path "/store/details" >=> details
        pathScan "/store/details/%d" 
            (fun id -> OK (sprintf "Details: %d" id))
        pathScan "/store/details/%s/%d" 
            (fun (a, id) -> OK (sprintf "Artist: %s; Id: %d" a id))
    ]

startWebServer defaultConfig routes