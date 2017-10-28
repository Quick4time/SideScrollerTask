using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class LobbyInfoPanel : MonoBehaviour
{
    public TextMeshProUGUI infoText;
    public TextMeshProUGUI buttonText;
    public Button singleButton;

    public void Display(string info, string buttonInfo, UnityEngine.Events.UnityAction buttonClbk)
    {
        infoText.text = info;

        buttonText.text = buttonInfo;

        singleButton.onClick.RemoveAllListeners();

        if (buttonClbk != null)
        {
            singleButton.onClick.AddListener(buttonClbk);
        }

        singleButton.onClick.AddListener(() => { gameObject.SetActive(false); });

        gameObject.SetActive(true);
    }
}
