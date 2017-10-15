using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Update () {
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
                }
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
}
