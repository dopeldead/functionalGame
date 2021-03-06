module Login exposing (..)

import Html exposing (..)
import Html.Attributes exposing (..)
import Html.Events exposing (..)
import Http
import Json.Encode as Encode

import Utils

type FormField
    = UserName

-- UPDATE

--HELPERS

formUrlencoded : List (String, String) -> String
formUrlencoded object =
    object
        |> List.map
            (\( name, value ) ->
                Http.encodeUri name
                    ++ "="
                    ++ Http.encodeUri value
            )
        |> String.join "&"


postRequest : Model -> Http.Request String
postRequest model =
    let
        body =  formUrlencoded
                [ ( "", model.userName )
                ]
                |> Http.stringBody "text/plain"  
    in
    Http.request
        { method = "POST"
        , headers = []
        , url = "http://localhost:8080/Login"
        , body = body
        , expect = Http.expectString
        , timeout = Nothing
        , withCredentials = False
        }



-- VIEWS


view : Model -> Html Msg
view model =
    Utils.view model viewForm


viewForm : Model -> Html Msg
viewForm model =
    Html.form
        [ onSubmit SubmitForm
        , class "form-container"
        ]
        [ label []
            [ text "UserName"
            , input
                [ type_ "text"
                , placeholder "UserName"
                , onInput SetUserName
                , value model.userName
                ]
                []
            ]    
        , button
            []
            [ text "Submit" ]
        ]



-- MAIN


main : Program Never Model Msg
main =
    program
        { init = ( initialModel, Cmd.none )
        , view = view
        , update = update
        , subscriptions = \_ -> Sub.none
        }