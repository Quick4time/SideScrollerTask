﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Networking;

public class LobbyPlayer : NetworkLobbyPlayer
{
    static Color[] Colors = new Color[] { Color.magenta, Color.red, Color.cyan, Color.blue, Color.green, Color.yellow };
    //используется для того что-бы у игроков небыло одинаковых цветов.
    static List<int> colorInUse = new List<int>();

    public Button colorButton;
    public TMP_InputField nameInput;
    public Button readyButton;
    public GameObject waitingPlayerGo;
    public Button removePlayerButton;

    public GameObject localIcone;
    public GameObject remoteIcone;


    //OnMyName function will be invoked on clients when server change the value of playerName
    [SyncVar(hook = "OnMyName")]
    public string playerName = string.Empty;
    [SyncVar(hook = "OnMyColor")]
    public Color playerColor = Color.white;

    public Color OddRowColor = new Color(250.0f / 255.0f, 250.0f / 255.0f, 250.0f / 255.0f, 1.0f);
    public Color EvenRowColor = new Color(180.0f / 255.0f, 180.0f / 255.0f, 180.0f / 255.0f, 1.0f);

    static Color JoinColor = new Color(255.0f / 255.0f, 0.0f, 101.0f / 255.0f, 1.0f);
    static Color NotReadyColor = new Color(34.0f / 255.0f, 44 / 255.0f, 55.0f / 255.0f, 1.0f);
    static Color ReadyColor = new Color(0.0f, 204.0f / 255.0f, 204.0f / 255.0f, 1.0f);
    static Color TransparentColor = new Color(0, 0, 0, 0);

    public override void OnClientEnterLobby()
    {
        base.OnClientEnterLobby();

        if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(1);
        LobbyPlayerList.Instance.AddPlayer(this);
        if (isServer){
            LobbyPlayerList.Instance.DisplayDirectWarning(true);
        }else{
            LobbyPlayerList.Instance.DisplayDirectWarning(false);
        }
        if (isLocalPlayer)
        {
            SetupLocalPlayer();
        }
        else
        {
            SetupOtherPlayer();
            //OnMyName(playerName);
            //OnMyColor(playerColor);
        }

        //setup the player data on UI. The value are SyncVar so the player
        //will be created with the right value currently on server
        OnMyName(playerName);
        OnMyColor(playerColor);
    }

    public override void OnStartAuthority()
    {
        base.OnStartAuthority();

        //if we return from a game, color of text can still be the one for "Ready"
        readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().color = Color.white;

        SetupLocalPlayer();
    }

    void ChangeReadyButtonColor(Color c)
    {
        ColorBlock b = readyButton.colors;
        b.normalColor = c;
        b.pressedColor = c;
        b.disabledColor = c;
        readyButton.colors = b;
    }

    void SetupOtherPlayer()
    {
        nameInput.interactable = false;
        removePlayerButton.interactable = NetworkServer.active;

        ChangeReadyButtonColor(NotReadyColor);

        readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "...";
        readyButton.interactable = false;

        OnClientReady(false);
    }

    void SetupLocalPlayer()
    {
        nameInput.interactable = true;
        remoteIcone.gameObject.SetActive(false);
        localIcone.gameObject.SetActive(true);

        CheckRemoveButton();

        if (playerColor == Color.white)
            CmdColorChange();

        ChangeReadyButtonColor(JoinColor);

        readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = "JOIN";
        readyButton.interactable = true;

        //have to use child count of player prefab already setup as "this.slot" is not set yet
        if (playerName == "")
            CmdNameChanged("Player " + (LobbyPlayerList.Instance.playerListContentTransform.childCount - 1));

        // we switch from simple name display to name input
        colorButton.interactable = true;
        nameInput.interactable = true;

        nameInput.onEndEdit.RemoveAllListeners();
        nameInput.onEndEdit.AddListener(OnNameChanged);

        colorButton.onClick.RemoveAllListeners();
        colorButton.onClick.AddListener(OnColorClicked);

        readyButton.onClick.RemoveAllListeners();
        readyButton.onClick.AddListener(OnReadyClicked);

        //when OnClientEnterLobby is called, the loval PlayerController is not yet created, so we need to redo that here to disable
        //the add button if we reach maxLocalPlayer. We pass 0, as it was already counted on OnClientEnterLobby
        if (LobbyManager.s_Singleton != null)
            LobbyManager.s_Singleton.OnPlayersNumberModified(0);

    }

    public void CheckRemoveButton()
    {
        if (!isLocalPlayer)
            return;

        int localPlayerCount = 0;
        foreach (UnityEngine.Networking.PlayerController p in ClientScene.localPlayers)
            localPlayerCount += (p == null || p.playerControllerId == -1) ? 0 : 1;

        removePlayerButton.interactable = localPlayerCount > 1;
    }

    public override void OnClientReady(bool readyState)
    {
        if (readyState)
        {
            ChangeReadyButtonColor(TransparentColor);

            TextMeshProUGUI textComponent = readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            textComponent.text = "READY";
            textComponent.color = ReadyColor;
            readyButton.interactable = false;
            colorButton.interactable = false;
            nameInput.interactable = false;
        }
        else
        {
            ChangeReadyButtonColor(isLocalPlayer ? JoinColor : NotReadyColor);

            TextMeshProUGUI textComponent = readyButton.transform.GetChild(0).GetComponent<TextMeshProUGUI>();
            textComponent.text = isLocalPlayer ? "Join" : "...";
            textComponent.color = Color.white;
            readyButton.interactable = isLocalPlayer;
            colorButton.interactable = isLocalPlayer;
            nameInput.interactable = isLocalPlayer;
        }
    }

    public void OnPlayerListChanged(int idx)
    {
        GetComponent<Image>().color = (idx % 2 == 0) ? EvenRowColor : OddRowColor;
    }

    ///===== callback from sync var

    public void OnMyName(string newName)
    {
        playerName = newName;
        nameInput.text = playerName;
    }

    public void OnMyColor(Color newColor)
    {
        playerColor = newColor;
        gameObject.GetComponent<Image>().color = newColor;
    }

    //===== UI Handler

    //Note that those handler use Command function, as we need to change the value on the server not locally
    //so that all client get the new value throught syncvar
    public void OnColorClicked()
    {
        CmdColorChange();
    }

    public void OnReadyClicked()
    {
        SendReadyToBeginMessage();
    }

    public void OnNameChanged(string str)
    {
        CmdNameChanged(str);
    }

    public void OnRemovePlayerClick()
    {
        if (isLocalPlayer)
        {
            RemovePlayer();
        }
        else if (isServer) { }
            LobbyManager.s_Singleton.KickPlayer(connectionToClient);
    }

    public void ToggleJoinButton(bool enabled)
    {
        readyButton.gameObject.SetActive(enabled);
        waitingPlayerGo.gameObject.SetActive(!enabled);
    }

    [ClientRpc]
    public void RpcUpdateCountdown(int countdown)
    {
        LobbyManager.s_Singleton.countdownPanel.text.text = "Match Starting in " + countdown;
        LobbyManager.s_Singleton.countdownPanel.gameObject.SetActive(countdown != 0);
    }

    [ClientRpc]
    public void RpcUpdateRemoveButton()
    {
        CheckRemoveButton();
    }

    //====== Server Command

    // Полезный код проверяющий наличие использующихся цветов
    [Command]
    public void CmdColorChange()
    {
        int idx = System.Array.IndexOf(Colors, playerColor);

        int inUseidx = colorInUse.IndexOf(idx);

        if (idx < 0) idx = 0;

        idx = (idx + 1) % Colors.Length;

        bool alreadyInUse = false;
        do
        {
            alreadyInUse = false;
            for (int i = 0; i < colorInUse.Count; ++i)
            {
                if (colorInUse[i] == idx)
                {// этот цвет уже используется
                    alreadyInUse = true;
                    idx = (idx + 1) % Colors.Length;
                }
            }
        }
        while (alreadyInUse);

        if (inUseidx >= 0)
        {//if we already add an entry in the colorTabs, we change it
            colorInUse[inUseidx] = idx;
        }
        else
        {// else we add it
            colorInUse.Add(idx);
        }
        playerColor = Colors[idx];
    }

    [Command]
    public void CmdNameChanged(string name)
    {
        playerName = name;
    }

    // Cleanup thing when get destroy (which happen when client kick or disconnect)
    public void OnDestroy()
    {
        LobbyPlayerList.Instance.RemovePlayer(this);
        if (LobbyManager.s_Singleton != null) LobbyManager.s_Singleton.OnPlayersNumberModified(-1);

        int idx = System.Array.IndexOf(Colors, playerColor);

        if (idx < 0)
            return;

        for(int i = 0; i < colorInUse.Count; ++i)
        {
            if (colorInUse[i] == idx)
            {//that color is already in use
                colorInUse.RemoveAt(i);
                break;
            }
        }
    }
}
