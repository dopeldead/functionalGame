module App

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Web
open Suave.RequestErrors

let LoginPost =
    choose [
        POST >=> warbler (fun ctx -> OK "On crée la session du mec")
    ]

let GetGame =
    request (fun r ->
        match r.queryParam "cell" with
        | Choice1Of2 genre -> OK "Le mec a cliqué sur une cell - > renvoi d'une game"
        | Choice2Of2 msg -> OK "Le mec poll - > renvoi d'une game"
    )


let routes = 
    choose [
        path "/Login" >=> LoginPost
        path "/Game" >=> GetGame     
    ]

startWebServer defaultConfig routes