using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    [SerializeField]
    private float laserSpeed;
    [SerializeField]
    private GameObject laserImpacts;
    [SerializeField]
    private GameObject explosion;
    GM gm;

    private void Start()
    {
        gm = GM.Instance;
        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    void Update ()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (laserSpeed * Time.deltaTime));
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Enemy"))
        {
            AudioManager.Instance.PlaySound(5);
            Instantiate(explosion, transform.position, transform.rotation);
            gm.SpawnPowerUp(transform.position);
            gm.AddScore(other.GetComponent<EnemyShoot>().Value);
            Destroy(other.gameObject);
        } 

        if (other.CompareTag("Player"))
        {
            gm.loseLife();
        }

        if (other.CompareTag("Shield"))
        {
            other.gameObject.SetActive(false);
        }
        if (other.CompareTag("Boss"))
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().loseBossLife();
        }
        if (other.CompareTag("BossShield"))
        {
            GameObject.FindGameObjectWithTag("Boss").GetComponent<BossController>().loseDurabilityShiel();
        }
        Instantiate(laserImpacts, transform.position, transform.rotation);
        Destroy(gameObject);
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
