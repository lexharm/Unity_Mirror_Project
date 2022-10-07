using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using System;

public class PlayerScore : NetworkBehaviour
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

    public event Action<PlayerScore> OnScoreChanged;

    [Header ("Debug variables")] 
    [SerializeField]
    public PlayerUI2 playerUI;

    void Start()
    {
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
}
