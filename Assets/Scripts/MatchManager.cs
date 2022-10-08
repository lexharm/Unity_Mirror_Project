using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchManager : NetworkBehaviour
{
    #region Variables
    [Tooltip("Minimum score value to win the match.")]
    [SerializeField]
    [Range(1, 100)]
    private int scoreValueToWin = 3;

    [Tooltip("Delay in seconds before match restarting.")]
    [SerializeField]
    [Range(1, 60)]
    private int restartMatchDelay = 5;

    [SerializeField]
    private Text matchResultText;
    
    [SerializeField]
    private Text matchRestartText;

    [SerializeField]
    private PlayerUI2 playerUI;
    #endregion

    public void RegisterPlayerScore(Player2 playerScore)
    {
        playerScore.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(Player2 playerScore)
    {
        if (playerScore.score >= scoreValueToWin)
        {
            string resultText = playerScore.isLocalPlayer ? "You win!" : "You lose :(";
            playerUI.SetAndShowResultText(resultText);
            StartCoroutine(RestartMatch());
        }
    }

    private IEnumerator RestartMatch()
    {
        for (int i = restartMatchDelay; i > 0; i--)
        {
            playerUI.SetAndShowRestartText("Next match starts in " + i + "...");
            yield return new WaitForSeconds(1);
        }
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
}
