import Html
import Page.Login as Login

type Page
    = Blank
    | Login Login.Model

type alias Model =
{ userName : String
, response : Maybe String
, statusGame : String
, opponent : String
}


initialModel : Model
initialModel =
    { userName = ""
    , response = Nothing
    , statusGame = ""
    , opponent = ""
    }


type Msg
    = NoOp
    | SubmitForm
    | SetUserName String
    | Response (Result Http.Error String)

    update : Msg -> Model -> ( Model, Cmd Msg )
update msg model =
    case Debug.log "msg" msg of
        NoOp ->
            ( model, Cmd.none )

        SubmitForm ->
            ( { model | response = Nothing }
            , Http.send Response (postRequest model)
            )

        SetUserName userName ->
            ( { model | userName = userName }, Cmd.none )

        Response (Ok response) ->
            ( { model | response = Just response }, Cmd.none )

        Response (Err error) ->
            ( { model | response = Just (toString error ++ " - See the Console for more details.") }, Cmd.none )

        Nothing -> (model, Effects.none)
        
        Just someString -> (someString, Effects.none) 
