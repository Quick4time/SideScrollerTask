using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

[RequireComponent(typeof(NetworkTransform))]
public class PlayerControllerNet : NetworkBehaviour {

    [SerializeField]
    private float moveSpeed;
    private Transform ThisTransform = null;
    private float limitMovementShipX = 3.4f;
    private float limitMovementShipY = 4.4f;
    [SerializeField]
    private float fireRate = 5;
    private float timeToFire = 0;
    [SerializeField]
    private GameObject[] lasers;
    [SerializeField]
    public Transform[] lasersPoint;

    protected Collider2D col;

    float Horz;
    float Vert;

    [SyncVar(hook = "OnLifeChanged")]
    public int lifeCount;

    void Awake()
    {
        GMNetwork.sShips.Add(this);
    }

    void Start()
    {
        ThisTransform = GetComponent<Transform>();
        col = GetComponent<Collider2D>();
        // We don't want to handle collision on client, so disable collider there
        col.enabled = isServer;

        if (GMNetwork.sInstance != null)
        {
            Init();
        }
    }

    public void Init()
    {
        UpdateLifeText();
    }

    void OnLifeChanged(int newValue)
    {
        lifeCount = newValue;
        UpdateLifeText();
    }

    void UpdateLifeText()
    {
        Debug.Log("Life is " + lifeCount);
    }

   [ClientCallback] // обновление для сервера
    void Update()
    {
        if (!isLocalPlayer)
            return;

        Horz = Input.GetAxisRaw("Horizontal");
        Vert = Input.GetAxisRaw("Vertical");  

        if (Time.time > timeToFire)
        {
            timeToFire = Time.time + 1 / fireRate;
            CmdFire();
        }
    }

    [ClientCallback]
    private void FixedUpdate()
    {
        if (!hasAuthority)
            return;

        ThisTransform.position += transform.right * Horz * Time.deltaTime * moveSpeed;
        ThisTransform.position += transform.up * Vert * Time.deltaTime * moveSpeed;

        ThisTransform.position = new Vector3(Mathf.Clamp(transform.position.x, -limitMovementShipX, limitMovementShipX),
            Mathf.Clamp(transform.position.y, -limitMovementShipY, limitMovementShipY), transform.position.z);
    }

    private void OnDestroy()
    {
        GMNetwork.sShips.Remove(this);
    }

    private void CreateLaser()
    {
        Instantiate(lasers[0], lasersPoint[0].position, lasersPoint[0].rotation);
    }

    [Command]
    public void CmdFire()
    {
        if (!isClient)
            CreateLaser();

        RpcFire();
    }

    [ClientRpc]
    public void RpcFire()
    {
        CreateLaser();
    }
}
