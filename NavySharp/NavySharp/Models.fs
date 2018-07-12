module Models

    type Position = 
        struct
            val X : int
            val Y : int
            new(x, y) = { X = x; Y = y }
        end    

    type Ship = {
        StartCell : Position
        EndCell : Position
        Length : int
    }
    type Player = {
        Name : string
        Ships : List<Ship>
        Shots : List<Position>
    }

    type Game = {
       Active : Player
       Passive : Player
       Message  : string
       WinnerName : string
    }

    let PlayerShot (player : Player) (coordinates : Position) : Player = 
        { player with Shots = coordinates :: player.Shots}
    
    let IsWon(game:Game) : bool =
        let positions = [
            for s in game.Passive.Ships do
                for x in s.StartCell.X .. s.EndCell.X do
                    for y in s.StartCell.Y .. s.EndCell.Y do 
                        yield Position(x,y)
        ]
        (List.except(game.Active.Shots) positions).Length =0

    let HandleHit (game:Game) (passive:Player) (coordinates: Position) : Game =
        if game.Active.Shots.Length < (List.sumBy(fun s -> s.Length)  game.Passive.Ships)
            then { game with Passive = PlayerShot game.Active coordinates;Active=passive ;  Message = "hit;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}
        elif IsWon game
            then { game with Passive = PlayerShot game.Active coordinates;Active=passive ;  Message = game.Active.Name+"won"; WinnerName = game.Active.Name}
        else { game with Passive = PlayerShot game.Active coordinates;Active=passive ;  Message = "hit;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}

    let GameShot (game : Game) (coordinates:Position) : Game = 
        let passive = game.Passive
        //check if shot valid, succed or miss
        if List.contains(coordinates) game.Active.Shots
            then { game with  Message = "cannot shoot twice at the same place;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}
        else 
            let potentialHits = List.map(fun s -> s.StartCell.X <= coordinates.X && s.EndCell.X >= coordinates.X && s.StartCell.Y <= coordinates.Y && s.EndCell.Y >= coordinates.Y    ) game.Passive.Ships
            let isHit = List.exists(fun b -> b) potentialHits
            match isHit with
                //ifhit have to check if game is won
                | true -> HandleHit game passive coordinates  
                | false -> { game with Passive = PlayerShot game.Active coordinates;Active = passive ;  Message = "miss;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}