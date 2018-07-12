module App

open System.Linq
open Suave
open Suave.RequestErrors
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Suave.Web
open Models
open Suave


let mutable games : List<Game> = []
let mutable players : List<Player> = []
let mutable lobbyPlayers : List<Player> = []


let HandleLogin ctx =  
     let name = ctx.rawForm |> System.Text.Encoding.UTF8.GetString
     players <- players @ [{Name=name; Ships=[]; Shots=[]}]
     
     OK ""   

let HandleGamePolling (username : string) : WebPart = 
    if not (List.exists(fun p -> p.Name = username) players)
        then NOT_FOUND username
    else if (List.exists(fun g -> g.Active.Name=username || g.Passive.Name=username) games)
        then (OK  (Json.toJson(List.find(fun g -> g.Active.Name=username || g.Passive.Name=username) games)|> System.Text.Encoding.UTF8.GetString))
    else
        OK "temp"

let HandleCellSelection (cellIdx : string)(username : string) ( req : HttpRequest)  : WebPart = 
    //update game bard with new shot
    let idx = LanguagePrimitives.ParseInt32(cellIdx)
    let shot = Position(idx%10,idx/10)
    let game = games.Where(fun g -> g.Active.Name=username).Single()

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
        else if String.isEmpty(cellidx)
            then HandleGamePolling username
        else (HandleCellSelection cellidx username r)
    )


let routes = 
    choose [
        path "/Login" >=> LoginPost
        path "/Game" >=> GetGame     
    ]

startWebServer defaultConfig routes