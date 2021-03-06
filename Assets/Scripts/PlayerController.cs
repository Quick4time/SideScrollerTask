﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    [SerializeField]
    private float moveSpeed;
    private Transform ThisTransform = null;
    private float limitMovementShipX = 3.4f;
    private float limitMovementShipY = 4.4f;

    private Animator myAnimator;

    [SerializeField]
    private GameObject[] lasers;
    [SerializeField]
    public Transform[] lasersPoint;
    [SerializeField]
    private float fireRate = 5;
    private float timeToFire = 0;
    GM gm;
    AudioManager audioManager;


	void Start () {
        ThisTransform = GetComponent<Transform>();
        myAnimator = GetComponent<Animator>();
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

	void Update () {
       
        float Horz = Input.GetAxisRaw("Horizontal");
        float Vert = Input.GetAxisRaw("Vertical");

        ThisTransform.position += transform.right * Horz * Time.deltaTime * moveSpeed;
        ThisTransform.position += transform.up * Vert * Time.deltaTime * moveSpeed;

        ThisTransform.position = new Vector3(Mathf.Clamp(transform.position.x, -limitMovementShipX, limitMovementShipX), 
            Mathf.Clamp(transform.position.y, -limitMovementShipY, limitMovementShipY), transform.position.z);
        myAnimator.SetFloat("Horz", Horz);


        if (Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            Instantiate(lasers[0], lasersPoint[0].position, lasersPoint[0].rotation);
            audioManager.PlaySound(4);
            if (gm.Boost)
            {
                Instantiate(lasers[1], lasersPoint[1].position, lasersPoint[1].rotation);
                Instantiate(lasers[1], lasersPoint[2].position, lasersPoint[2].rotation);
                audioManager.PlaySound(4);
            }
        }
    }
}
