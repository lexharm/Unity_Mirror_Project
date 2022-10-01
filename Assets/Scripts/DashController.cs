using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DashController : NetworkBehaviour
{
    [SerializeField] private float changeColorTime = 3.0f;
    [SyncVar] private float startChangeColorTime = 0;
    [SyncVar] private bool _isDashed = false;
    public bool isDashed => _isDashed;
    private Color defaultColor = Color.white;
    private Color changeColor = Color.red;
    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void ProcessDashCollide()
    {
        if (!_isDashed)
        {
            _isDashed = true;
            startChangeColorTime = Time.time;
            RpcChangeColor(changeColor);
        }        
    }

    [ClientRpc]
    private void RpcChangeColor(Color color)
    {
        renderer.material.color = color;
    }

    private void Update()
    {
        if (_isDashed && Time.time - startChangeColorTime >= changeColorTime)
        {
            CmdCheckAndSetIsDashed(false);
        }
    }

    [Command]
    private void CmdCheckAndSetIsDashed(bool value)
    {
        if (Time.time - startChangeColorTime >= changeColorTime)
        {
            _isDashed = value;
            RpcChangeColor(defaultColor);
        }
    }
}
