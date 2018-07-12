
import Html
import Page.Home as Home
import Page.Login as Login

type Page
    = Blank
    | Login Login.Model

type alias Model =
    { userName : String
    }