using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class GMNetwork : NetworkBehaviour {

    static public List<PlayerControllerNet> sShips = new List<PlayerControllerNet>();
    static public GMNetwork sInstance = null;


    private void Awake()
    {
        sInstance = this;
    }

    [ServerCallback]
    private void Update()
    {
        if (sShips.Count == 0)
            return;

        bool allDestroyed = true;
        for (int i = 0; i < sShips.Count; i++)
        {
            allDestroyed &= (sShips[i].lifeCount == 0);
        }

        if (allDestroyed)
        {
            StartCoroutine(ReturnToLobby());
        }
    }

    public override void OnStartClient()
    {
        base.OnStartClient();
    }

    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }
}
