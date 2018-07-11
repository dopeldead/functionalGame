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
    