module App

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
         Console.WriteLine(sprintf "%A" players)
         Console.WriteLine(sprintf "%A" lobbyPlayers)
         OK ""

let HandleGamePolling (username : string) : WebPart = 
    if not (List.exists(fun p -> p.Name = username) players)
        then NOT_FOUND username
    elif (List.exists(fun g -> g.Active.Name=username || g.Passive.Name=username) games)
        then (OK  (Json.toJson(List.find(fun g -> g.Active.Name=username || g.Passive.Name=username) games)|> System.Text.Encoding.UTF8.GetString))
    elif lobbyPlayers.Length >=2
        then OK "gotta create game"
    else
        OK "waiting"

let HandleCellSelection (cellIdx : string)(username : string) ( req : HttpRequest)  : WebPart = 
    let idx = LanguagePrimitives.ParseInt32(cellIdx)
    let shot = Position(idx%10,idx/10)
    let game =  List.find(fun g -> g.Active.Name=username) games
    games <-  (GameShot game shot) :: List.where(fun g-> not(g.Active.Name=game.Active.Name)) games
    HandleGamePolling username

let LoginPost = POST >=> request HandleLogin
    
let GetGame =
    request (fun r ->
        let cellidx =  match r.queryParam "cell" with
                        | Choice1Of2 cellIdx -> cellIdx 
                        | Choice2Of2 _ -> ""
        let username = match r.queryParam "username" with  
                        | Choice1Of2 username -> username
                        | Choice2Of2 _ -> ""
        if String.isEmpty(username)
            then BAD_REQUEST "Username should be provided"
        elif String.isEmpty(cellidx)
            then HandleGamePolling username
        else (HandleCellSelection cellidx username r)
    )

let routes = 
    choose [
        path "/Login" >=> LoginPost
        path "/Game" >=> GetGame     
    ]

startWebServer defaultConfig routes