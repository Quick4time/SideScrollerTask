using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Audio;

public class PopupController : MonoBehaviour {

    [SerializeField]
    private GameObject popupMenu;
    [SerializeField]
    public bool activePopup;
    [SerializeField]
    private GameObject goOptions;
    [SerializeField]
    private GameObject goPause;
    private bool inOptions;

    [SerializeField]
    AudioMixer mixer;

    SceneController sceneController;

    private void Start()
    {
        sceneController = SceneController.Instance;
        if (sceneController == null)
        {
            Debug.LogError("SceneController not found!");
        }
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            activePopup = !activePopup;
            if (activePopup)
            {
                OnPopup();
            }
            else
            {
                if (inOptions)
                {
                    TransitionToPause();
                    activePopup = true;
                    return;
                }                   
                OffPopup();
            }
        }
    }

    public void OnPopup()
    {
        popupMenu.SetActive(true);
        Time.timeScale = 0;
        activePopup = true;
    }
    public void OffPopup()
    {
        popupMenu.SetActive(false);
        Time.timeScale = 1;
        activePopup = false;
    }

    public void TransitionToOptions()
    {
        goOptions.SetActive(true);
        inOptions = true;
        goPause.SetActive(false);
    }
    public void TransitionToPause()
    {
        goOptions.SetActive(false);
        inOptions = false;
        goPause.SetActive(true);
    }

    public void OnSoundToggle()
    {
        AudioManager.Instance.soundMute = !AudioManager.Instance.soundMute;
    }

    public void OnSoundValue(float value)
    {
        mixer.SetFloat("Master", value);
    }
    public void OnSfxValue(float value)
    {
        mixer.SetFloat("SFX", value);
    }
    public void OnMusicValue(float value)
    {
        mixer.SetFloat("Music", value);
    }

    public void goToMainMenu()
    {
        sceneController.GoToMainMenu();
    }

    public void Restart()
    {
        sceneController.StartGame();
    }
}
