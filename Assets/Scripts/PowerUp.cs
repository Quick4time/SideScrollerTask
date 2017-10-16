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

    private void Start()
    {
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
                    StartCoroutine(GM.Instance.AddShield());
                    break;
                case 1:
                    if (GM.Instance.Boost){
                        StartCoroutine(GM.Instance.AddShield());
                    }else{
                        GM.Instance.Boost = true;
                    }
                    break;
                case 2:
                    GM.Instance.addLife();
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
