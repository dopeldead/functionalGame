module App

open Suave
open Suave.RequestErrors
open Suave.Filters
open Suave.Operators
open Suave.Successful
open Models
open System

open Newtonsoft.Json

let mutable games : List<Game> = []
let mutable players : List<Player> = []
let mutable lobbyPlayers : List<Player> = []

let HandleLogin (request : HttpRequest) : WebPart =  

     let name = request.rawForm |> System.Text.Encoding.UTF8.GetString
     let player = {Name=name.Replace("=", ""); Ships=[]; Shots=[]}

     if String.IsNullOrEmpty(player.Name)
        then 
        BAD_REQUEST "Empty username"
     elif (List.exists(fun p -> p.Name=player.Name) players)
        then
        CONFLICT "Username already exists"
     else
         players <- players @ [player]
         lobbyPlayers <- lobbyPlayers @ [player]
         OK ""

let rec PlaceShips (player:Player) (shipList: List<int>) : Player =
    if List.isEmpty(shipList) 
        then player
    else        
        let rnd = System.Random()
        let size = shipList.Head
        let startX = rnd.Next(0,10-size)
        let startY = rnd.Next(0,10-size)
        let startPos = Position(startX,startY)
        let horizontal = (rnd.Next(0,2)=0)
        let endPos = Position(startX + (if horizontal then size-1 else 0), startY + (if horizontal then 0 else size-1))
        let ship = {StartCell = startPos; EndCell = endPos; Length=size}
        let shipCells = [
            for x in ship.StartCell.X .. ship.EndCell.X do
                for y in ship.StartCell.Y .. ship.EndCell.Y do 
                    yield Position(x,y)
        ]
        let usedCells = [
            for s in player.Ships do
                for x in s.StartCell.X .. s.EndCell.X do
                    for y in s.StartCell.Y .. s.EndCell.Y do 
                        yield Position(x,y)
        ]
        if not ((List.except(usedCells) shipCells).Length = size)
            then PlaceShips player shipList
        else PlaceShips {player with Ships= ship::player.Ships } shipList.Tail

// Creates game from 2 players
let CreateGame (username : string ) : Game =
    let firstPlayer = List.find(fun p -> (p.Name = username)) lobbyPlayers
    let otherPlayer = List.find(fun p -> not (p.Name = username)) lobbyPlayers
    let boatList = [5;4;3;3;2]

    let game = {Active= (PlaceShips firstPlayer boatList); Passive=(PlaceShips otherPlayer boatList); Message="";WinnerName=""}
    lobbyPlayers <- List.where(fun p -> not(p.Name=firstPlayer.Name || p.Name = otherPlayer.Name ) ) lobbyPlayers
    games <- games @ [game]
    game
        


let HandleGamePolling (username : string) : WebPart = 
    if not (List.exists(fun p -> p.Name = username) players)
        then NOT_FOUND username
    elif (List.exists(fun g -> g.Active.Name=username || g.Passive.Name=username) games)
        then
        if List.exists(fun g-> g.Active.Name=username && not(String.isEmpty( g.WinnerName))) games
            then
                let game = List.find(fun g-> g.Active.Name=username && not(String.isEmpty( g.WinnerName))) games
                games <- List.where(fun g-> not( g.Active.Name=username && not(String.isEmpty( g.WinnerName)))) games
                OK  (JsonConvert.SerializeObject(game))
        else
             OK  (JsonConvert.SerializeObject(List.find(fun g -> g.Active.Name=username || g.Passive.Name=username) games))
    elif lobbyPlayers.Length >=2
    then 
        OK (JsonConvert.SerializeObject(CreateGame username))
    else
        OK "waiting"

let HandleCellSelection (cellIdx : string)(username : string) ( req : HttpRequest)  : WebPart = 
    let idx = LanguagePrimitives.ParseInt32(cellIdx)
    let shot = Position(idx/10,idx%10)
    if not (List.exists(fun g -> g.Active.Name=username) games)
        then FORBIDDEN "Not your turn"
    else
        let game =  List.find(fun g -> g.Active.Name=username) games
        games <-  (GameShot game shot) :: List.where(fun g-> not(g.Active.Name=game.Active.Name)) games
        HandleGamePolling username

let LoginPost = request HandleLogin
    
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
let setCORSHeaders =
    Suave.Writers.addHeader  "Access-Control-Allow-Origin" "*" 
    >=> Suave.Writers.addHeader "Access-Control-Allow-Headers" "content-type" 
    >=> Suave.Writers.addHeader "Access-Control-Allow-Methods" "GET,POST" 

let routes = 
    choose [
        GET >=>
            fun context ->
                context |> (
                    setCORSHeaders
                    >=> choose
                        [ 
                            path "/Game" >=> GetGame  
                        ] )
        POST >=>
            fun context ->
                context |> (
                    setCORSHeaders
                    >=> choose
                        [
                            path "/Login" >=> LoginPost
                        ] )
    ]


startWebServer defaultConfig routes