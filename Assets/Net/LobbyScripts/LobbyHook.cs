using UnityEngine.Networking;
using UnityEngine;

public abstract class LobbyHook : MonoBehaviour
{
    // Subclass this and redefine the function you want
    // then add it to the lobby prefab
    public virtual void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer) { }
}
