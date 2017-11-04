using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;


sealed class GMNetwork : NetworkBehaviour
{
    static public List<PlayerControllerNet> sShips = new List<PlayerControllerNet>();
    static public GMNetwork sInstance = null;

    [SerializeField]
    private Transform waveSpawnPoint;
    [SerializeField]
    private GameObject[] enemyPrefabs;

    [SerializeField]
    private float[] waveDelays;
    [SerializeField]
    private bool spawningWaves;
    [SerializeField]
    private int waveTracker;
    private int numGoForWave;
    MoveAsteroidNet[] getMoveAsteroids;
    MoveEnemyScript[] getMoveEnemy;
    [SerializeField]
    [SyncVar(hook = "RandomNumberSyncCallback")]
    private int randomInt;
    private float timerUpdateRandomValue = 0;


    private void Awake()
    {
        sInstance = this;
    }

    [ServerCallback]
    private void Update()
    {
        if (sShips.Count == 0)
            return;

        bool allDestroyed = true;
        for (int i = 0; i < sShips.Count; i++)
        {
            allDestroyed &= (sShips[i].lifeCount == 0);
        }

        if (isServer)
        {
            timerUpdateRandomValue += Time.deltaTime;
            if (timerUpdateRandomValue >= 2)
            {
                randomInt = Random.Range(1, 5); // от 1 до 4-х
                timerUpdateRandomValue = 0;
            }
        }

        if (allDestroyed)
        {
            StartCoroutine(ReturnToLobby());
        }

        if (spawningWaves)
        {
            waveDelays[waveTracker] -= Time.deltaTime;

            if (waveDelays[waveTracker] < 0)
            {
                SpawnWaves(randomInt);
                waveTracker++;

                if (waveTracker >= waveDelays.Length)
                {
                    spawningWaves = false;
                }
            }
        }

    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        foreach (GameObject obj in enemyPrefabs)
        {
            ClientScene.RegisterPrefab(obj);
        }
    }

    void RandomNumberSyncCallback(int newNumber)
    {
        if (isServer) return;
        randomInt = newNumber;
    }

    private void SpawnWaves(int numWaves)
    {
        GameObject[] selectorArr;
        GameObject go = null;
        switch (numWaves)
        {
            case 1:
                selectorArr = new GameObject[5];
                getMoveEnemy = new MoveEnemyScript[selectorArr.Length];
                for (int i = 0; i < 5; i++)
                {
                    go = Instantiate(enemyPrefabs[4], waveSpawnPoint.position, Quaternion.identity);
                    selectorArr[i] = go;
                    NetworkServer.Spawn(selectorArr[i]);
                    getMoveEnemy[i] = selectorArr[i].GetComponent<MoveEnemyScript>();
                }
                getMoveEnemy[0].SetStartPosition(new Vector3(0.0f, 6.2f, 0.0f));
                getMoveEnemy[0].SetEnemyMovement(0.0f, 2.5f, 1.5f, 4.0f, 0.0f, false, 2);
                getMoveEnemy[1].SetStartPosition(new Vector3(1.5f, 7.5f, 0.0f));
                getMoveEnemy[1].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, -2.0f, false, 2);
                getMoveEnemy[2].SetStartPosition(new Vector3(3.0f, 8.5f, 0.0f));
                getMoveEnemy[2].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, -3.0f, false, 2);
                getMoveEnemy[3].SetStartPosition(new Vector3(-1.5f, 7.5f, 0.0f));
                getMoveEnemy[3].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, 2.0f, true, 2);
                getMoveEnemy[4].SetStartPosition(new Vector3(-3.0f, 8.5f, 0.0f));
                getMoveEnemy[4].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, 3.0f, true, 2);
                break;
            case 2:
                selectorArr = new GameObject[8];
                getMoveEnemy = new MoveEnemyScript[selectorArr.Length];
                for (int i = 0; i < 8; i++)
                {
                    if (i < 4)
                        go = Instantiate(enemyPrefabs[6], waveSpawnPoint.position, Quaternion.identity);
                    else if (i >= 4)
                        go = Instantiate(enemyPrefabs[4], waveSpawnPoint.position, Quaternion.identity);

                    selectorArr[i] = go;
                    NetworkServer.Spawn(selectorArr[i]);
                    getMoveEnemy[i] = selectorArr[i].GetComponent<MoveEnemyScript>();
                }
                getMoveEnemy[0].SetStartPosition(new Vector3(-3.0f, 6.0f, 0.0f));
                getMoveEnemy[0].SetEnemyMovement(0.0f, 2.0f, 2.0f, 2.0f, 3.0f, true, 2);
                getMoveEnemy[1].SetStartPosition(new Vector3(-1.0f, 6.6f, 0.0f));
                getMoveEnemy[1].SetEnemyMovement(0.0f, 2.0f, 2.0f, 3.0f, 1.0f, true, 2);
                getMoveEnemy[2].SetStartPosition(new Vector3(3.0f, 7.6f, 0.0f));
                getMoveEnemy[2].SetEnemyMovement(0.0f, 2.0f, 2.0f, 4.0f, -3.0f, false, 2);
                getMoveEnemy[3].SetStartPosition(new Vector3(-1.0f, 8.6f, 0.0f));
                getMoveEnemy[3].SetEnemyMovement(0.0f, 2.0f, 2.0f, 5.0f, 1.0f, true, 2);
                getMoveEnemy[4].SetStartPosition(new Vector3(3.0f, 6.0f, 0.0f));
                getMoveEnemy[4].SetEnemyMovement(0.0f, 2.0f, 2.0f, 2.0f, -3.0f, false, 2);
                getMoveEnemy[5].SetStartPosition(new Vector3(1.0f, 6.6f, 0.0f));
                getMoveEnemy[5].SetEnemyMovement(0.0f, 2.0f, 2.0f, 3.0f, -1.0f, false, 2);
                getMoveEnemy[6].SetStartPosition(new Vector3(-3.0f, 7.6f, 0.0f));
                getMoveEnemy[6].SetEnemyMovement(0.0f, 2.0f, 2.0f, 4.0f, 3.0f, true, 2);
                getMoveEnemy[7].SetStartPosition(new Vector3(1.0f, 8.6f, 0.0f));
                getMoveEnemy[7].SetEnemyMovement(0.0f, 2.0f, 2.0f, 5.0f, -1.0f, false, 2);
                break;
            case 3:
                selectorArr = new GameObject[7];
                getMoveEnemy = new MoveEnemyScript[selectorArr.Length];
                for (int i = 0; i < 7; i++)
                {
                    go = Instantiate(enemyPrefabs[5], waveSpawnPoint.position, Quaternion.identity);
                    selectorArr[i] = go;
                    NetworkServer.Spawn(selectorArr[i]);
                    getMoveEnemy[i] = selectorArr[i].GetComponent<MoveEnemyScript>();
                }
                getMoveEnemy[0].SetStartPosition(new Vector3(3.5f, 11.0f, 0.0f));
                getMoveEnemy[0].SetEnemyMovement(25.0f, 2.0f, 3.0f, 3.0f, 0.0f, false, 1);
                getMoveEnemy[1].SetStartPosition(new Vector3(-2.5f, 10.0f, 0.0f));
                getMoveEnemy[1].SetEnemyMovement(25.0f, 2.0f, 3.0f, 2.0f, 0.0f, true, 1);
                getMoveEnemy[2].SetStartPosition(new Vector3(2.5f, 9.0f, 0.0f));
                getMoveEnemy[2].SetEnemyMovement(25.0f, 2.0f, 3.0f, 1.0f, 0.0f, false, 1);
                getMoveEnemy[3].SetStartPosition(new Vector3(-1.5f, 8.0f, 0.0f));
                getMoveEnemy[3].SetEnemyMovement(25.0f, 2.0f, 3.0f, 0.0f, 0.0f, true, 1);
                getMoveEnemy[4].SetStartPosition(new Vector3(1.5f, 7.0f, 0.0f));
                getMoveEnemy[4].SetEnemyMovement(25.0f, 2.0f, 3.0f, -1.0f, 0.0f, false, 1);
                getMoveEnemy[5].SetStartPosition(new Vector3(-0.5f, 6.0f, 0.0f));
                getMoveEnemy[5].SetEnemyMovement(25.0f, 2.0f, 3.0f, -2.0f, 0.0f, true, 1);
                getMoveEnemy[6].SetStartPosition(new Vector3(-3.5f, 12.0f, 0.0f));
                getMoveEnemy[6].SetEnemyMovement(25.0f, 2.0f, 3.0f, 4.0f, 0.0f, true, 1);
                break;
            case 4:
                selectorArr = new GameObject[8];
                getMoveEnemy = new MoveEnemyScript[selectorArr.Length];
                for (int i = 0; i < 8; i++)
                {
                    go = Instantiate(enemyPrefabs[6], waveSpawnPoint.position, Quaternion.identity);
                    selectorArr[i] = go;
                    NetworkServer.Spawn(selectorArr[i]);
                    getMoveEnemy[i] = selectorArr[i].GetComponent<MoveEnemyScript>();
                }
                getMoveEnemy[0].SetStartPosition(new Vector3(-3.0f, 8.5f, 0.0f));
                getMoveEnemy[0].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, true, 0);
                getMoveEnemy[1].SetStartPosition(new Vector3(-3.0f, 7.5f, 0.0f));
                getMoveEnemy[1].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, true, 0);
                getMoveEnemy[2].SetStartPosition(new Vector3(-3.0f, 6.5f, 0.0f));
                getMoveEnemy[2].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, true, 0);
                getMoveEnemy[3].SetStartPosition(new Vector3(-3.0f, 9.5f, 0.0f));
                getMoveEnemy[3].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, true, 0);
                getMoveEnemy[4].SetStartPosition(new Vector3(3.0f, 8.5f, 0.0f));
                getMoveEnemy[4].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, false, 0);
                getMoveEnemy[5].SetStartPosition(new Vector3(3.0f, 7.5f, 0.0f));
                getMoveEnemy[5].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, false, 0);
                getMoveEnemy[6].SetStartPosition(new Vector3(3.0f, 6.5f, 0.0f));
                getMoveEnemy[6].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, false, 0);
                getMoveEnemy[7].SetStartPosition(new Vector3(3.0f, 9.5f, 0.0f));
                getMoveEnemy[7].SetEnemyMovement(0.0f, 2.5f, 2.0f, 3.0f, 0.0f, false, 0);
                break;
            case 5:

                break;
            case 6:

                break;
            case 7:

                break;
        }
    }

    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }
}

