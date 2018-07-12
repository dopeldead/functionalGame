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
* /login POST body: username -> OK
* /game GET optional param: cell int (0..99) -> object game (voir fichier JSON a la racine)

la game a un champ message contenant le statut du dernier tir : "miss;4;6" 
																"hit;4;6"
Ou des informations sur un tire deja fait ans la case : "error;4;6"
Ou des info sur le gagnant "won" et le WinnerName contient le nom du gagnant