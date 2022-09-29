using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] [SerializeField] private float speed;
    private Rigidbody _rb;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();

        if (isClient && isLocalPlayer)
        {
            SetInputManagerPlayer();
        }

        if (isServer)
        {
            speed = 3;
        }
    }

    private void SetInputManagerPlayer()
    {
        InputManager.Instance.SetPlayer(this);
    }
    
    [Command]
    public void CmdMovePlayer(Vector3 movement)
    {
        _rb.AddForce(movement.normalized * speed);
    }
    
    public void MovePlayer(Vector3 movement)
    {
        _rb.AddForce(movement.normalized * speed);
    }
}
