using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//Main menu, mainly only a bunch of callback called by the UI (setup throught the Inspector)
public class LobbyMainMenu : MonoBehaviour
{
    public LobbyManager lobbyManager;
    public RectTransform lobbyPanel;
    public TMP_InputField ipInput;

    public void OnEnable()
    {
        lobbyManager.topPanel.ToggleVisibility(true);

        ipInput.onEndEdit.RemoveAllListeners();
        ipInput.onEndEdit.AddListener(onEndEditIP);
    }

    public void OnClickHost()
    {
        lobbyManager.StartHost();
    }

    public void OnClickJoin()
    {
        lobbyManager.ChangeTo(lobbyPanel);

        lobbyManager.networkAddress = ipInput.text;
        lobbyManager.StartClient();

        lobbyManager.backDelegate = lobbyManager.StopClientClbk;
        //lobbyManager.DisplayIsConnecting();

        lobbyManager.SetServerInfo("Conecting...", lobbyManager.networkAddress);
    }

    void onEndEditIP(string text)
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            OnClickJoin();
        }
    }
}
