using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserControllerNet : MonoBehaviour {

    [SerializeField]
    private float laserSpeed;

    void FixedUpdate () {
        transform.position = new Vector3(transform.position.x, transform.position.y + (laserSpeed * Time.deltaTime));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
