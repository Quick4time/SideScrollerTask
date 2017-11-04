using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(Rigidbody2D))]
[RequireComponent(typeof(NetworkTransform))]
public class MoveAsteroidNet : NetworkBehaviour
{
    private float moveSpeed;
    private float rotateSpeed;
    [SerializeField]
    private Transform startPos;

    private void Start()
    {
        SetStartPos(startPos);
    }

    [ServerCallback]
    void FixedUpdate()
    {
        transform.position += Vector3.down * Time.deltaTime * moveSpeed;
        transform.rotation = Quaternion.Euler(0, 0, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    }

    public void SetStartPos(Transform StartPos)
    {
        this.startPos = StartPos;
        transform.position = StartPos.position;
    }
}
