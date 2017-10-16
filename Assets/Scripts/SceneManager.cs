using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneManager : SingletoneAsComponent<SceneManager> {

    public static SceneManager Instance
    {
        get { return ((SceneManager)_Instance); }
        set { _Instance = value; }
    }

    

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    
}
