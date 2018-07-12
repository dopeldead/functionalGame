import Html exposing (..)
import Http
import Task exposing (Task)
import Time
import Main as Main


type alias Model = String

type Action = LoadReadings (Maybe String) 

init = "Start"

httpGet t =
  (Http.getString "localhost://8080/game?username=" ++ Main.UserName
    |> Task.toMaybe)
    `Task.andThen`
    (\maybeString -> Signal.send maybeString)

port periodicTasks : Signal (Task () ())
port periodicTasks = Signal.map httpGet <| Time.every (1*Time.second)

readingsMailbox : Signal.Mailbox (Maybe String)
readingsMailbox = Signal.mailbox Nothing 

update action model =
  case action of 
    LoadReadings m ->
      case m of 
        Nothing -> (model, Effects.none)
        Just someString -> (someString, Effects.none) 

view address model =
  div [] [text model]

import Main as Main
 Http

getGameState : String -> Cmd Msg
getGameState topic = 
    let 
        url =
            "localhost://8080/game?username=" ++ Main.UserName
    in


