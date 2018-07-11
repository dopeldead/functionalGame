module App

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Web
open Suave.RequestErrors

let HandleLogin (ctx : HttpContext) : WebPart = 
        OK "On crée la session du mec" 

let HandleCellSelection (cellIdx : string) : WebPart = 
    OK "Le mec a cliqué sur une cell - > renvoi d'une game"

let HandleGamePolling : WebPart = 
    OK "Le mec poll - > renvoi d'une game"


let LoginPost =
    choose [
        POST >=> warbler (fun ctx -> HandleLogin ctx)
    ]

let GetGame =
    request (fun r ->
        match r.queryParam "cell" with
        | Choice1Of2 cell -> HandleCellSelection cell
        | Choice2Of2 _ -> HandleGamePolling
    )


let routes = 
    choose [
        path "/Login" >=> LoginPost
        path "/Game" >=> GetGame     
    ]

startWebServer defaultConfig routes