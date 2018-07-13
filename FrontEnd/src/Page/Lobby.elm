
import Html exposing (..)
import Http
import Task exposing (Task)
import Time
import Main as Main


httpGet t =
  (Http.getString "localhost://8080/game?username=" ++ Main.UserName
    |> Task.toMaybe)
    `Task.andThen`
    (\maybeString -> Signal.send maybeString)

port periodicTasks : Signal (Task () ())
port periodicTasks = Signal.map httpGet <| Time.every (1*Time.second)


view address model =
  div [] [text model]

