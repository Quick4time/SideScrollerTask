using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserController : MonoBehaviour {

    [SerializeField]
    private float laserSpeed;
    [SerializeField]
    private GameObject laserImpacts;
    [SerializeField]
    private string Tag;
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
        
        if (other.CompareTag(Tag))
        {
            if (explosion != null)
            {
                Instantiate(explosion, transform.position, transform.rotation);
            }
            GM.Instance.SpawnPowerUp(transform.position);
            Destroy(other.gameObject);
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
