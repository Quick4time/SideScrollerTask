using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyShoot : MonoBehaviour {

    [SerializeField]
    private GameObject laser;
    [SerializeField]
    private Transform[] laserPoint;
    [SerializeField]
    private GameObject impactEnemy;

    public int Value;

    [SerializeField]
    private float shotDelay;
    private float shotCounter = 1.0f;
	
	void Update () {
        shotCounter -= Time.deltaTime;
        if (shotCounter <= 0)
        {
            for (int i = 0; i < laserPoint.Length; i++)
            {
                Instantiate(laser, laserPoint[i].position, laserPoint[i].rotation);
            }
            shotCounter = shotDelay;
        }
	}

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GM.Instance.loseLife();
            Destroy(gameObject);
            Instantiate(impactEnemy, transform.position, transform.rotation);
        }
    }
}
