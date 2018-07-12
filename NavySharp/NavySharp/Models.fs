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
       IsFinished : bool
       WinnerName : string
    }

    let PlayerShot player coordinates = 
        { player with Shots = coordinates :: player.Shots}
    
    let GameShot game coordinates = 
        let passive = game.Passive
        //check if shot valid, succed or miss
        if List.contains(coordinates) game.Active.Shots
            then { game with  Message = "cannot shoot twice at the same place;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}
        else 
            let potentialHits = List.map(fun s -> s.StartCell.X <= coordinates.X && s.EndCell.X >= coordinates.X && s.StartCell.Y <= coordinates.Y && s.EndCell.Y >= coordinates.Y    ) game.Passive.Ships
            let isHit = List.exists(fun b -> b) potentialHits
            match isHit with
                //ifhit have to check if game is won
                | true -> { game with Passive = PlayerShot game.Active coordinates;Active=passive ;  Message = "hit;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}
                | false -> { game with Passive = PlayerShot game.Active coordinates;Active = passive ;  Message = "miss;"+coordinates.X.ToString()+";"+coordinates.Y.ToString()}