using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : SingletoneAsComponent<AudioManager> {

    public static AudioManager Instance
    {
        get { return ((AudioManager)_Instance); }
        set { _Instance = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }
}
