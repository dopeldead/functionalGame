module App

open Suave
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Web
open Suave.RequestErrors

let HandleLogin (ctx : HttpContext) : WebPart = 
     OK "On crée la session du mec" 

let HandleGamePolling : WebPart = 
    OK "Le mec poll - > renvoi d'une game"

let HandleCellSelection (cellIdx : string) : WebPart = 
//update game bard with new shot
   HandleGamePolling



let LoginPost =
    POST >=> warbler (fun ctx -> HandleLogin ctx)
    

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