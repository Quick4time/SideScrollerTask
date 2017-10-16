using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class PopupController : MonoBehaviour {

    [SerializeField]
    private GameObject popupMenu;
    [SerializeField]
    public bool activePopup;
    private Animator animator;
    [SerializeField]
    private GameObject goOptions;
    [SerializeField]
    private GameObject goPause;
    private bool inOptions;

    private void Start()
    {
        animator = popupMenu.GetComponent<Animator>();
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
}
