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
    private GameObject[] enemyForSpawn;

    [SerializeField]
    private float[] waveDelays;
    [SerializeField]
    private bool spawningWaves;
    [SerializeField]
    private int waveTracker;
    private int numGoForWave;


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
                Instantiate(Waves(1), waveSpawnPoint.position, waveSpawnPoint.rotation);

                waveTracker++;
            }
        }

    }

    private GameObject Waves(int numWaves)
    {
        GameObject GoHolder = new GameObject("Waves" + numWaves);
        GameObject[] curGo = new GameObject[numGoForWave];
        GameObject instance;
        switch (numWaves)
        {
            case 1:
                numGoForWave = 4;
                for (int i = 0; i < 4; i++)
                {
                    curGo[i] = enemyPrefabs[i];
                    instance = Instantiate(curGo[i]);
                    instance.transform.SetParent(GoHolder.transform);

                    NetworkServer.Spawn(GoHolder);
                }
                break;
            case 2:

                break;
            case 3:

                break;
        }
        return GoHolder;
    }

    public override void OnStartClient()
    {
        base.OnStartClient();

        foreach (GameObject obj in enemyPrefabs)
        {
            ClientScene.RegisterPrefab(obj);
        }
    }


    IEnumerator ReturnToLobby()
    {
        yield return new WaitForSeconds(3.0f);
        LobbyManager.s_Singleton.ServerReturnToLobby();
    }
}
