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
    MoveEnemyScript[] getMS;


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

        if (allDestroyed)
        {
            StartCoroutine(ReturnToLobby());
        }

        if (spawningWaves)
        {
            waveDelays[waveTracker] -= Time.deltaTime;

            if (waveDelays[waveTracker] < 0)
            {
                SpawnWaves(1);
                waveTracker++;
            }
            else
            {
                spawningWaves = false;
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

    private void SpawnWaves(int numWaves)
    {
        GameObject[] selectorArr;
        switch (numWaves)
        {
            case 1:
                selectorArr = new GameObject[5];
                getMS = new MoveEnemyScript[selectorArr.Length];
                for (int i = 0; i < 5; i++)
                {
                    GameObject go = Instantiate(enemyPrefabs[4], waveSpawnPoint.position, Quaternion.identity);
                    selectorArr[i] = go;
                    NetworkServer.Spawn(selectorArr[i]);
                    getMS[i] = selectorArr[i].GetComponent<MoveEnemyScript>();
                }
                getMS[0].SetStartPosition(new Vector3(0.0f, 6.2f, 0.0f));
                getMS[0].SetEnemyMovement(0.0f, 2.5f, 1.5f, 4.0f, 0.0f, false, 2);
                getMS[1].SetStartPosition(new Vector3(1.5f, 7.5f, 0.0f));
                getMS[1].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, -2.0f, false, 2);
                getMS[2].SetStartPosition(new Vector3(3.0f, 8.5f, 0.0f));
                getMS[2].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, -3.0f, false, 2);
                getMS[3].SetStartPosition(new Vector3(-1.5f, 7.5f, 0.0f));
                getMS[3].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, 2.0f, true, 2);
                getMS[4].SetStartPosition(new Vector3(-3.0f, 8.5f, 0.0f));
                getMS[4].SetEnemyMovement(0.0f, 3.5f, 1.5f, 4.0f, 3.0f, true, 2);
                break;
            case 2:

                break;
            case 3:

                break;
        }
    }

    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }
}
