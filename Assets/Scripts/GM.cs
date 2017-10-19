using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GM : SingletoneAsComponent<GM>
{
    public static GM Instance
    {
        get { return ((GM)_Instance); }
        set { _Instance = value; }
    }

    [SerializeField]
    private Transform waveSpawnPoint;

    [SerializeField]
    private GameObject[] waves;
    [SerializeField]
    private float[] waveDelays;
    [SerializeField]
    private bool spawningWaves;
    [SerializeField]
    private int waveTracker;

    [SerializeField]
    private GameObject[] powerUp;
    public GameObject[] Shields;
    [HideInInspector]
    public bool Boost = false;

    [SerializeField]
    private GameObject goBoss;
    [SerializeField]
    private Transform spawnPointBoss;

    [SerializeField]
    private int playerLives;
    [SerializeField]
    private GameObject[] symbolicLives;
    private GameObject goPlayer;

    [SerializeField]
    private GameObject gameOver;
    [SerializeField]
    private GameObject finTheGame;
    [SerializeField]
    private TextMeshProUGUI finScore;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    private int currentScore;

    AudioManager audioManager;
    SceneController sceneController;

    private void Awake()
    {
        Shields = GameObject.FindGameObjectsWithTag("Shield");
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        playerLives = symbolicLives.Length;
        updateScoreCounter();
        Time.timeScale = 1;
    }

    private void Start()
    {
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
        sceneController = SceneController.Instance;
        if (sceneController == null)
        {
            Debug.LogError("SceneController not found!");
        }
        audioManager.PlaySound(1);
        sceneController.isShowing = false;
    }

    void Update()
    {
        if (spawningWaves)
        {
            waveDelays[waveTracker] -= Time.deltaTime;

            if (waveDelays[waveTracker] < 0)
            {
                Instantiate(waves[Random.Range(0, waves.Length)], waveSpawnPoint.position, waveSpawnPoint.rotation);

                waveTracker++;

                if (waveTracker >= waveDelays.Length)
                {
                    spawningWaves = false;
                    Instantiate(goBoss, spawnPointBoss.position, Quaternion.identity);
                    audioManager.StopSound(1);
                    audioManager.PlaySound(2);
                }
            }
        }

        if (!sceneController.isShowing)
        {
            Cursor.visible = false;
        }
    }

    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        updateScoreCounter();
    }

    private void updateScoreCounter()
    {
        scoreText.text = string.Format(" Score: {0}", currentScore);
    }

    public bool addLife()
    {
        if (playerLives < symbolicLives.Length)
        {
            playerLives++;
            updateLiveCounter();
            return true;
        }
        return false;
    }

    public bool loseLife()
    {
        playerLives--;
        Boost = false;
        if (playerLives > 0)
        {
            updateLiveCounter();
            return false;
        }
        playerLives = 0;
        GameOver();
        updateLiveCounter();
        return true;
    }

    private void updateLiveCounter()
    {
        for (int i = 0; i < symbolicLives.Length; i++)
        {
            if (i < playerLives)
            {
                symbolicLives[i].SetActive(true);
            }
            else
            {
                symbolicLives[i].SetActive(false);
            }
        }
    }

    public void SpawnPowerUp(Vector3 spawnPos)
    {
        if (Random.Range(0, 10) == 1)
        {
            GameObject toInstantiate = powerUp[Random.Range(0, powerUp.Length)];
            Instantiate(toInstantiate, spawnPos, Quaternion.identity);
        }
    }

    public IEnumerator AddShield()
    {
        int index = 0;
        while (index <= Shields.Length)
        {
            for (int i = 0; i < Shields.Length; i++)
            {
                Shields[i].SetActive(true);
            }
            yield return new WaitForSeconds(0.2f);
            index++;
        }
    }

    private void GameOver()
    {
        sceneController.isShowing = true;
        AudioManager.Instance.PlaySound(5);
        goPlayer.SetActive(false);
        Time.timeScale = 0;
        gameOver.SetActive(true);
    }

    private void FinishGame()
    {
        sceneController.isShowing = true;
        Time.timeScale = 0;
        finTheGame.SetActive(true);
        finScore.text = string.Format("You're Final Score: {0}", currentScore);
    }
}
