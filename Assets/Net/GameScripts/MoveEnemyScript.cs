using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class MoveEnemyScript : NetworkBehaviour {

    [SerializeField]
    private Vector3 startPos;
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

    private void Start()
    {
        SetStartPosition(startPos);
    }

    [ServerCallback]
    void FixedUpdate()
    {
        SetEnemyMovement(rotateSpeed, moveSpeedX, moveSpeedY, yTarget, xTarget, moveRight, moveType);
    }

    public void SetStartPosition(Vector3 StartPos)
    {
        this.startPos = StartPos;
        transform.position = StartPos;
    }

    public void SetEnemyMovement(float RotateSpeed, float MoveSpeedX, float MoveSpeedY, float YTarget, float XTarget, bool MoveRight, int MoveType)
    {
        this.rotateSpeed = RotateSpeed;
        this.moveSpeedX = MoveSpeedX;
        this.moveSpeedY = MoveSpeedY;
        this.yTarget = YTarget;
        this.xTarget = XTarget;
        this.moveRight = MoveRight;
        this.moveType = MoveType;

        switch (MoveType)
        {
            case 0:
                if (transform.position.y < YTarget)
                {
                    if (MoveRight)
                    {
                        transform.position += new Vector3(MoveSpeedX, -MoveSpeedY) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(-MoveSpeedX, -MoveSpeedY) * Time.deltaTime;
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -MoveSpeedY) * Time.deltaTime;
                }
                break;
            case 1:
                if (transform.position.y < YTarget)
                {
                    if (MoveRight)
                    {
                        transform.position += new Vector3(MoveSpeedX, 0.0f) * Time.deltaTime;
                    }
                    else
                    {
                        transform.position += new Vector3(-MoveSpeedX, 0.0f) * Time.deltaTime;
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -MoveSpeedY) * Time.deltaTime;
                }
                break;
            case 2:
                if (transform.position.y < YTarget)
                {
                    if (MoveRight)
                    {
                        if (transform.position.x > XTarget)
                        {
                            transform.position += new Vector3(0.0f, -MoveSpeedY) * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(MoveSpeedX, -MoveSpeedY) * Time.deltaTime;
                        }
                    }
                    else
                    {
                        if (transform.position.x < XTarget)
                        {
                            transform.position += new Vector3(0.0f, -MoveSpeedY) * Time.deltaTime;
                        }
                        else
                        {
                            transform.position += new Vector3(-MoveSpeedX, -MoveSpeedY) * Time.deltaTime;
                        }
                    }
                }
                else
                {
                    transform.position += new Vector3(0.0f, -MoveSpeedY) * Time.deltaTime;
                }
                break;
        }
        transform.rotation = Quaternion.Euler(0.0f, 0.0f, transform.rotation.eulerAngles.z + (RotateSpeed * Time.deltaTime));
    }
    
    [ServerCallback]
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
}
