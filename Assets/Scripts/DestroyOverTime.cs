using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOverTime : MonoBehaviour {
	
	void Update () {
        Destroy(gameObject, 0.75f);
	}
}
