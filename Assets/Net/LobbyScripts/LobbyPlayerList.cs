using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LobbyPlayerList : MonoBehaviour {

    public static LobbyPlayerList Instance = null;

    public RectTransform playerListContentTransform;
    public GameObject warningDirectPlayServer;
    public Transform addButtonRow;

    protected VerticalLayoutGroup layout;
    protected List<LobbyPlayer> players = new List<LobbyPlayer>();

    public void OnEnable()
    {
        Instance = this;
        layout = playerListContentTransform.GetComponent<VerticalLayoutGroup>();
    }

    public void DisplayDirectWarning(bool enabled)
    {
        if (warningDirectPlayServer != null)
            warningDirectPlayServer.SetActive(enabled);
    }

    private void Update()
    {
        if (layout)
            layout.childAlignment = Time.frameCount % 2 == 0 ? TextAnchor.UpperCenter : TextAnchor.UpperLeft;
    }

    public void AddPlayer(LobbyPlayer player)
    {
        if (players.Contains(player))
            return;

        players.Add(player);

        player.transform.SetParent(playerListContentTransform, false);
        addButtonRow.transform.SetAsLastSibling();

        PlayerListModified();
    }

    public void RemovePlayer (LobbyPlayer player)
    {
        players.Remove(player);
        PlayerListModified();
    }

    public void PlayerListModified()
    {
        int i = 0;
        foreach (LobbyPlayer p in players)
        {
            p.OnPlayerListChanged(i);
            ++i;
        }
    }
}
