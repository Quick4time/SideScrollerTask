using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum TypePowerUp { Shield, Power, Health, None }

public class PowerUp : MonoBehaviour {

    [SerializeField]
    private TypePowerUp type = TypePowerUp.None;

    [SerializeField]
    private float moveSpeedY;
    [SerializeField]
    private float randomlyMoveSpeedX;

    private GameObject goPlayer;
    private PlayerController scrPlayer;

    private void Start()
    {
        goPlayer = GameObject.FindGameObjectWithTag("Player");
        scrPlayer = (PlayerController)goPlayer.GetComponent(typeof(PlayerController));
        randomlyMoveSpeedX = Random.Range(-2.0f, 2.0f);
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + (randomlyMoveSpeedX * Time.deltaTime), transform.position.y - (moveSpeedY * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            int i = (int)type;
            switch(i)
            {
                case 0:
                    for (int s = 0; s < scrPlayer.shields.Length; s++)
                    {
                        if (scrPlayer.shields[i].activeInHierarchy == false)
                        {
                            scrPlayer.shields[i].SetActive(true);
                        }
                    }
                    break;
                case 1:
                    if (scrPlayer.Boost == true)
                    {
                        Debug.Log("lol");
                    }
                    scrPlayer.Boost = true;
                    break;
                case 2:
                    Debug.Log("Health");
                    break;
            }
            Destroy(gameObject);
        }
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
