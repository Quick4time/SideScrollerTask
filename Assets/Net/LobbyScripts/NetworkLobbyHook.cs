using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

// используем этот код для переноски параметров из лобби в игру
public class NetworkLobbyHook : LobbyHook
{
    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        LobbyPlayer lobby = lobbyPlayer.GetComponent<LobbyPlayer>();
        /// EXAMPLE
        PlayerControllerNet spaceship = gamePlayer.GetComponent<PlayerControllerNet>();

        //spaceship.name = lobby.name;
        //spaceship.color = lobby.playerColor;
        //spaceship.score = 0;
        spaceship.lifeCount = 3;
    }
}
