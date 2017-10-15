using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveEnemy : MonoBehaviour
{
    [SerializeField]
    private float rotateSpeed;
    [SerializeField]
    private float moveSpeedX;
    [SerializeField]
    private float moveSpeedY;
    [SerializeField]
    private float yTarget;
    [SerializeField]
    private float xTarget;

    [SerializeField]
    private bool moveRight;

    [SerializeField]
    public int moveType; // 0 - движение диагонально, 1 - движение только по оси x, 2 - движение до точки. 

    void Update()
    {
        switch (moveType)
        {
            case 0:
                if (transform.position.y < yTarget)
                {
                    if (moveRight)
                    {
                        transform.position += new Vector3(moveSpeedX, -moveSpeedY) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(-moveSpeedX, -moveSpeedY) * Time.deltaTime;
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                }
                break;
            case 1:
                if (transform.position.y < yTarget)
                {
                    if (moveRight)
                    {
                        transform.position += new Vector3(moveSpeedX, 0.0f) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(-moveSpeedX, 0.0f) * Time.deltaTime;
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                }
                break;
            case 2:
                if (transform.position.y < yTarget)
                {
                    if (moveRight)
                    {
                        if (transform.position.x > xTarget)
                        {
                            transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(moveSpeedX, -moveSpeedY) * Time.deltaTime;
                        }
                    }
                    else
                    {
                        if (transform.position.x < xTarget)
                        {
                            transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(-moveSpeedX, -moveSpeedY) * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                }
                break;
        }
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + (rotateSpeed * Time.deltaTime));
    }

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
