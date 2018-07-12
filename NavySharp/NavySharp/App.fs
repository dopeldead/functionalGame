module App

open System.Linq
open Suave
open Suave.RequestErrors
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Models
open System



let mutable games : List<Game> = []
let mutable players : List<Player> = []
let mutable lobbyPlayers : List<Player> = []


let HandleLogin ctx =  
     let name = ctx.rawForm |> System.Text.Encoding.UTF8.GetString
     let player = {Name=name; Ships=[]; Shots=[]}

     if String.IsNullOrEmpty(name) then 
         BAD_REQUEST "Empty username"
     elif List.contains player players
     then
         CONFLICT "Username already exists"
     else
         players <- players @ [player]
         lobbyPlayers <- lobbyPlayers @ [player]
         Console.WriteLine(name)
         Console.WriteLine(sprintf "%A" players)
         OK ""


let HandleGamePolling : WebPart = 
    OK "Le mec poll - > renvoi d'une game"

let HandleCellSelection (cellIdx : string) ( req : HttpRequest)  : WebPart = 
    //update game bard with new shot
    let idx = LanguagePrimitives.ParseInt32(cellIdx)
    let shot = Position(idx%10,idx/10)
    let game = games.Where(fun g -> g.Active.Name="").Single()
    game.Active.Shots =  shot :: game.Active.Shots
    HandleGamePolling


let LoginPost = POST >=> request HandleLogin
    

let GetGame =
    request (fun r ->
        match r.queryParam "cell" with
        | Choice1Of2 cellIdx -> (HandleCellSelection cellIdx r)
        | Choice2Of2 _ -> HandleGamePolling
    )


let routes = 
    choose [
        path "/Login" >=> LoginPost
        path "/Game" >=> GetGame     
    ]

startWebServer defaultConfig routes