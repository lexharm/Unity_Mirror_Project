using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class OrbitCamera : NetworkBehaviour
{
    private Camera mainCam;
    private Player player;

    void Awake()
    {
        mainCam = Camera.main;
        player = GetComponent<Player>();
    }

    public override void OnStartLocalPlayer()
    {
        if (mainCam != null)
        {
            // configure and make camera a child of player with 3rd person offset
            mainCam.orthographic = false;
            mainCam.transform.SetParent(transform);
            mainCam.transform.localPosition = new Vector3(0f, 3f, -8f);
            mainCam.transform.localEulerAngles = new Vector3(10f, 0f, 0f);
        }
    }

    public override void OnStopLocalPlayer()
    {
        if (mainCam != null)
        {
            mainCam.transform.SetParent(null);
            SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
            mainCam.orthographic = true;
            mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }

    /*-----------------*/

    //[SerializeField] private Transform target;

    private Transform target;
    public float rotSpeed = 1.5f;


    private float _rotY;
    private Vector3 _offset;

    void Start()
    {
        /*if (isClient && isLocalPlayer)
        {
            mainCam = Camera.main;
            player = GetComponent<Player>();
        }*/
        //target = GetComponent<Transform>();
        _rotY = mainCam.transform.eulerAngles.y;
        _offset = transform.position - mainCam.transform.position;
    }

    private void Update()
    {
        float turn = Input.GetAxis("Mouse X") * rotSpeed * 3;
        _rotY += turn;
        player.CmdRotatePlayer(turn);
    }

    void LateUpdate()
    {
        Quaternion rotation = Quaternion.Euler(0, _rotY, 0);
        mainCam.transform.position = transform.position - (rotation * _offset);
        mainCam.transform.LookAt(transform);
    }
}
