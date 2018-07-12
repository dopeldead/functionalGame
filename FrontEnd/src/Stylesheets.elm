module Stylesheets exposing (..)

import Css.File exposing (..)
import Game


port files : CssFileStructure
port files =
    toFileStructure
        [ ( "Game.css", compile Game.css ) ]