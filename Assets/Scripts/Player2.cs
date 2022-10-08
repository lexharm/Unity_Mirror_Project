using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class Player2 : NetworkBehaviour
{
    [SerializeField]
    [SyncVar(hook = nameof(SetScore))]
    private int _score = 0;
    public int score {
        get => _score;
        set
        {
            _score = value;
            OnScoreChanged?.Invoke(this);
        }
    }

    public event Action<Player2> OnScoreChanged;

    [Header ("Debug variables")] 
    [SerializeField]
    public PlayerUI2 playerUI;

    private Color defaultColor;
    private MeshRenderer renderer;

    void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        defaultColor = renderer.material.color;
        FindObjectOfType<MatchManager>().RegisterPlayerScore(this);
    }

    public override void OnStartLocalPlayer()
    {
        playerUI = FindObjectOfType<PlayerUI2>();
    }

    private void SetScore(int _, int newValue)
    {
        score = newValue;
        if (isLocalPlayer)
        {
            playerUI.RefreshPlayerScore(score);
        }
    }

    [ClientRpc]
    public void RpcChangeColor(Color color)
    {
        renderer.material.color = color;
    }

    [ClientRpc]
    public void RpcSetDefaultColor()
    {
        renderer.material.color = defaultColor;
    }
}
