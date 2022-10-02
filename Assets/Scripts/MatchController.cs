using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class MatchController : NetworkBehaviour
{
    public override void OnStartServer()
    {
        Debug.Log("MatchController Server Start");
    }

    public override void OnStartClient()
    {
        Debug.Log("MatchController Client Start");
    }

    [Server]
    public void RestartGame()
    {
        Debug.Log("Restart Game");
    }
}
