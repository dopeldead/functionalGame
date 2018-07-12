module Main exposing (..)




import Array exposing (..)
import Html exposing (..)
import Html.Attributes exposing (..)
-- import Css exposing (..)
-- import Css.Elements as Css
-- import Css.Namespace exposing (namespace)
-- import Html.CssHelpers
-- import ColorScheme exposing (..)

-- cssNamespace : String
-- cssNamespace =
--     "Game"


-- { class, classList, id } =
--     Html.CssHelpers.withNamespace cssNamespace


-- type CssClasses
--     = Content
--     | Input
--     | Output
--     | Alias


-- css : Stylesheet
-- css =
--     (stylesheet << namespace cssNamespace)
--         [ Css.body
--             [ backgroundColor accent1 ]
--         , (.) Content
--             [ Css.width (px 960)
--             , margin2 zero auto
--             ]
--         , each [ (.) Input, (.) Output ]
--             [ Css.width (pct 40)
--             , Css.height (px 500)
--             , fontFamily monospace
--             ]
--         , aliasCss
--         ]
main =
    Html.program
        { init = init
        , view = view
        , update = update
        , subscriptions = subscriptions
        }


type alias Model =
    { test : Int
    }


init : ( Model, Cmd Msg )
init =
    ( Model 1
    , Cmd.none
    )


type Msg
    = Test


update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    ( model, Cmd.none )


subscriptions : Model -> Sub Msg
subscriptions model =
    Sub.none


view : Model -> Html Msg
view model =
    div [] 
    [
      div [] [ table ]
    , button [] [ text "JOUER ! " ]
    , div [class "test"] [   
        p [ ]
    [ text "Check this site out: "
    ]
    ]
    ]
    --,   div [] [ text "Jouer" ]


cellHead : String -> Html Msg
cellHead s =
    td [ colspan 2, style [ ( "border", "2px solid black") , ("height","100px"), ("width","100px")] ] [ s |> text ]



table : Html Msg
table =
    Html.table
        [ style [ ( "border", "1px solid black" ) ] ]
        [ caption
            []
            []
        , thead
            []
            [ tr [] (Array.initialize 10 (toString >> cellHead) |> toList) ]
        , tbody
            []  
            [ tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList), 
             tr [] (Array.initialize 10 (toString >> cellHead) |> toList) ]
           
        ]
