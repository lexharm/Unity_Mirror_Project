using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DashController : NetworkBehaviour
{
    [SerializeField] private float changeColorTime = 3.0f;
    private float startChangeColorTime = 0;
    private bool isDashed;
    private Color defaultColor = Color.white;
    private Color changeColor = Color.red;
    private MeshRenderer renderer;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    public void ProcessDashCollide()
    {
        if (!isDashed)
        {
            isDashed = true;
            startChangeColorTime = Time.time;
            renderer.material.color = changeColor;
        }        
    }

    private void Update()
    {
        if (isDashed && Time.time - startChangeColorTime >= changeColorTime)
        {
            renderer.material.color = defaultColor;
            isDashed = false;
        }
    }
}
