module Models

    type Position = { 
        X : int
        Y : int
    }
    type Ship = {
        StartCell : Position
        EndCell : Position
        Length : int
    }
    type Player = {
        Name : string
        Grid : string
    }
    type Game = {
        Active : Player
        Passive : Player
        Message : string
        IsFinished : bool
        WinnerName : string
    }