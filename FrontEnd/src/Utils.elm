module Utils exposing (urlMirrorService, view, viewUtils)

import Dict
import Html exposing (..)
import Html.Attributes exposing (..)

urlMirrorService : String
urlMirrorService =
    "https://httpbin.org/post"

viewSimple : String -> Html msg -> Html msg
viewSimple exampleVersion viewForm =
    div []
        [ viewForm
        ]


viewUtils :
    { a | response : Maybe String }
    -> ({ a | response : Maybe String } -> Html msg)
    -> Html msg
viewUtils model viewForm =
    div []
        [viewForm model
        , case model.response of
            Just response ->
                viewResponse response

            Nothing ->
                text ""
        ]


view :
    { a | response : Maybe String }
    -> ({ a | response : Maybe String } -> Html msg)
    -> Html msg
view =
    viewUtils


viewResponse : String -> Html msg
viewResponse response =
    div [ class "response-container" ]
        [ h2 [] [ text "Response" ]
        , textarea []
            [ text response ]
        ]


(=>) : a -> b -> ( a, b )
(=>) =
    (,)
