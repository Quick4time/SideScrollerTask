using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : SingletoneAsComponent<SceneController> {

    public static SceneController Instance
    {
        get { return ((SceneController)_Instance); }
        set { _Instance = value; }
    }

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ExitGame()
    {
        Application.Quit();
    }

    public void GoToMainMenu()
    {
        SceneManager.LoadScene(0); 
    }


}
