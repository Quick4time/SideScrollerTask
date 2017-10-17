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

    GM gm;
    AudioManager audioManager;

    private void Start()
    {
        randomlyMoveSpeedX = Random.Range(-2.0f, 2.0f);
        gm = GM.Instance;
        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
        }
        audioManager = AudioManager.Instance;
        if (audioManager == null)
        {
            Debug.LogError("AudioManager not found!");
        }
    }

    private void Update()
    {
        transform.position = new Vector3(transform.position.x + (randomlyMoveSpeedX * Time.deltaTime), transform.position.y - (moveSpeedY * Time.deltaTime));
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            audioManager.PlaySound(3);
            int i = (int)type;
            switch(i)
            {
                case 0:
                    StartCoroutine(gm.AddShield());
                    break;
                case 1:
                    if (gm.Boost){
                        StartCoroutine(gm.AddShield());
                    }else{
                        gm.Boost = true;
                    }
                    break;
                case 2:
                    gm.addLife();
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
