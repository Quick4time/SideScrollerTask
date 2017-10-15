using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollingBackground : MonoBehaviour {

    [SerializeField]
    private float speed;
    Vector3 startPos;
	void Start () {
        startPos = transform.position;
	}
	
	void Update () {
        transform.position += Vector3.down * Time.deltaTime * speed;
        if (transform.position.y < -10.108f)
            transform.position = startPos;
	}
}
