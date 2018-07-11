# Navy Sharp

Navy Sharp is a classic naval battle game playable in the browser.

* The backend was implemented using F# and the Suave framework.
* The frontend was implemented using Elm.

It was made in 3 days by Benoit Gidon, Florian Cuchot, Thibaud Pernet-Bissiere, Damien Escande, all students at [IN'TECH](https://www.intechinfo.fr "IN'TECH")

### Rules : 
Some of these were decided so as to focus on actual game logic
* No login/password, only a player name
* Game starts as soon as there are two players connected
* Players ships are put on the grid automatically in a random way
* No timer to end a turn

### API
/login POST body: username -> OK
/getGame GET -> object game 
/getPassive body:  GET -> object game
/endTurn body: cell A5 -> object game






