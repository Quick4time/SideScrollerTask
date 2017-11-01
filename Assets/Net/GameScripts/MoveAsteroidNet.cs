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

    [ServerCallback]
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            Destroy(gameObject);
        }
        if (collision.CompareTag("Shield"))
        {
            Destroy(gameObject);
        }
    }
}
