using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    [SyncVar] [SerializeField] private float speed;
    private Rigidbody _rb;
    public CharacterController characterController;

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        characterController = GetComponent<CharacterController>();

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
        //_rb.AddForce(movement.normalized * speed);
        //transform.Translate(movement.x * speed * Time.deltaTime, 0, movement.z * speed * Time.deltaTime);

        Vector3 direction = new Vector3(movement.x, 0, movement.z);
        direction = Vector3.ClampMagnitude(direction, 1f);
        direction = transform.TransformDirection(direction);
        direction *= speed;

        characterController.SimpleMove(direction);
    }

    /*public void MovePlayer(Vector3 movement)
    {
        //_rb.AddForce(movement.normalized * speed);
        //transform.Translate(movement.x * speed, 0, movement.z);
        
        Vector3 direction = new Vector3(movement.x, 0, movement.z);
        direction = Vector3.ClampMagnitude(direction, 1f);
        direction = transform.TransformDirection(direction);
        direction *= speed;

        characterController.SimpleMove(direction);
    }*/

    [Command]
    public void CmdRotatePlayer(float turn)
    {
        //transform.Rotate(0f, turn * Time.fixedDeltaTime, 0f);
        transform.Rotate(0f, turn, 0f);
    }
}
