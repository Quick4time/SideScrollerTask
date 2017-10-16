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

    void Update ()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y + (laserSpeed * Time.deltaTime));
	}
    private void OnTriggerEnter2D(Collider2D other)
    {
            Instantiate(laserImpacts, transform.position, transform.rotation);
            Destroy(gameObject);
        
        if (other.CompareTag("Enemy"))
        {
            Instantiate(explosion, transform.position, transform.rotation);
            GM.Instance.SpawnPowerUp(transform.position);
            GM.Instance.AddScore(other.GetComponent<EnemyShoot>().Value);
            Destroy(other.gameObject);
        } 

        if (other.CompareTag("Player"))
        {
            GM.Instance.loseLife();
        }

        if (other.CompareTag("Shield"))
        {
            other.gameObject.SetActive(false);
        }
    }
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
