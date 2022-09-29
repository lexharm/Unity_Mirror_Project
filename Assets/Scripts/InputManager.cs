using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    #region Singleton
    private static InputManager _inscance;
    public static InputManager Instance => _inscance;
    #endregion

    private Vector3 _movementVector = new Vector3();
    [SerializeField] private Player playerObject;

    private void Awake()
    {
        _inscance = this;
    }

    private void Update()
    {
        if (playerObject)
        {
            MoveInput();
        }
    }

    public void SetPlayer(Player player)
    {
        playerObject = player;
    }

    private void MoveInput()
    {
        _movementVector.x = Input.GetAxis("Horizontal");
        _movementVector.z = Input.GetAxis("Vertical");
        
        playerObject.CmdMovePlayer(_movementVector);
        playerObject.MovePlayer(_movementVector);
    }
}
