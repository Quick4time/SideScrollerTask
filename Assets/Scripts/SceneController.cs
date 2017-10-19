using UnityEngine;
using UnityEngine.SceneManagement;


public class SceneController : SingletoneAsComponent<SceneController> {

    public static SceneController Instance
    {
        get { return ((SceneController)_Instance); }
        set { _Instance = value; }
    }

    public bool isShowing = true;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Update()
    {
        if (isShowing)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        else
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
