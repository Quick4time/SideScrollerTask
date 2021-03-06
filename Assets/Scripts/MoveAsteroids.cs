﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveAsteroids : MonoBehaviour {

    private float moveSpeed;
    private float rotateSpeed;
    [SerializeField]
    private GameObject asteroidImpacts;
    GM gm;


    private void Start()
    {
        var random = new[]
        {
            ProportionValue.Create(0.2f, 25.0f),
            ProportionValue.Create(0.2f, -40.0f),
            ProportionValue.Create(0.2f, 35.0f),
            ProportionValue.Create(0.2f, -20.0f),
            ProportionValue.Create(0.2f, 50.0f)
        };
        rotateSpeed = random.ChoseByRandom();

        moveSpeed = Random.Range(1.0f, 3.0f);
        gm = GM.Instance;
        if (gm == null)
        {
            Debug.LogError("GameManager not found!");
        }
    }

    void Update () {
        transform.position += Vector3.down * Time.deltaTime * moveSpeed;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            gm.loseLife();
            AudioManager.Instance.PlaySound(5);
            Destroy(gameObject);
        }
        if (collision.CompareTag("Shield"))
        {
            collision.gameObject.SetActive(false);
            AudioManager.Instance.PlaySound(5);
            Destroy(gameObject);
        }
        Instantiate(asteroidImpacts, transform.position, transform.rotation);
    }
}
