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
        Ships : Ship[]
        Shots : Position[]
    }
    type Game( active : Player, passive : Player, message : string, isFinished : bool, winnerName : string) = 
       member this.Active = active
       member this.Passive = passive
       member this.Message  = message
       member this.IsFinished = isFinished
       member this.WinnerName =winnerName
    