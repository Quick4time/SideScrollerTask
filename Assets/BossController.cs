using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PhaseBoss { Start, Move, PhaseOne, PhaseTwo, PhaseThree }

public class BossController : MonoBehaviour
{

    [SerializeField]
    PhaseBoss phaseBoss;
    [SerializeField]
    private float countDownStart;
    [SerializeField]
    private float yTarget;
    [SerializeField]
    private float xTarget;
    [SerializeField]
    private float moveSpeedY;
    [SerializeField]
    private float moveSpeedX;
    [SerializeField]
    private bool moveRight;

    [SerializeField]
    private GameObject[] lasers;
    [SerializeField]
    private Transform[] lasersPoint;
    [SerializeField]
    private float shotDelay;
    private float shotCounter;

    [SerializeField]
    private int durableShield;
    [SerializeField]
    private int bossLife;

    [SerializeField]
    private GameObject Shields;

    void Start()
    {
        phaseBoss = PhaseBoss.Start;
    }

    void Update()
    {
        int index = (int)phaseBoss;
        switch (index)
        {
            case 0:
                countDownStart -= Time.deltaTime;
                if (countDownStart < 0)
                    phaseBoss = PhaseBoss.Move;
                break;
            case 1:
                if (transform.position.y > yTarget)
                    transform.position += new Vector3(0.0f, -moveSpeedY) * Time.deltaTime;
                else
                    phaseBoss = PhaseBoss.PhaseOne;
                break;
            case 2:
                shotCounter -= Time.deltaTime;
                if (!moveRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-xTarget, yTarget), Time.deltaTime * moveSpeedX);
                    if (shotCounter <= 0)
                    {
                        for (int i = 0; i < 2; i++)
                        {
                            AudioManager.Instance.PlaySound(8);
                            Instantiate(lasers[Random.Range(0,2)], lasersPoint[i].position, lasersPoint[i].rotation);
                        }
                        shotCounter = shotDelay;
                    }
                    if (transform.position.x <= -xTarget)
                        moveRight = true;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(xTarget, yTarget), Time.deltaTime * moveSpeedX);
                    if (shotCounter <= 0)
                    {
                        for (int i = 2; i < 4; i++)
                        {
                            AudioManager.Instance.PlaySound(8);
                            Instantiate(lasers[Random.Range(0, 2)], lasersPoint[i].position, lasersPoint[i].rotation);
                        }
                        shotCounter = shotDelay;
                    }
                    if (transform.position.x >= xTarget)
                        moveRight = false;
                }
                if (durableShield <= 0)
                {
                    Destroy(Shields);
                    phaseBoss = PhaseBoss.PhaseTwo;
                }
                break;
            case 3:
                shotCounter -= Time.deltaTime;
                if (!moveRight)
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(-xTarget, yTarget), Time.deltaTime * moveSpeedX * 1.8f);
                    if (transform.position.x <= -xTarget)
                        moveRight = true;
                }
                else
                {
                    transform.position = Vector3.MoveTowards(transform.position, new Vector3(xTarget, yTarget), Time.deltaTime * moveSpeedX * 1.8f);
                    if (transform.position.x >= xTarget)
                        moveRight = false;
                }
                if (shotCounter <= 0)
                {
                    for (int i = 0; i < lasersPoint.Length; i++)
                    {
                        AudioManager.Instance.PlaySound(8);
                        Instantiate(lasers[Random.Range(0, 2)], lasersPoint[i].position, lasersPoint[i].rotation);
                    }
                    shotCounter = shotDelay;
                }
                if (bossLife <= 45)
                    phaseBoss = PhaseBoss.PhaseThree;
                break;
            case 4:
                Debug.Log("Phase Three");
                break;
        }
    }

    public bool loseDurabilityShiel()
    {
        durableShield--;
        if (durableShield > 0)
            return false;

        durableShield = 0;
        return true;
    }

    public bool loseBossLife()
    {
        durableShield--;
        if (durableShield > 0)
            return false;

        durableShield = 0;
        return true;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            GM.Instance.loseLife();
        }
    }
}
