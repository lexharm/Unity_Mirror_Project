using System.Collections;
using UnityEngine;
using Mirror;
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

    #endregion

    public void RegisterPlayerScore(Player playerScore)
    {
        playerScore.OnPlayerScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(Player player)
    {
        if (player.score >= scoreValueToWin)
        {
            string resultText = player.isLocalPlayer ? "You won!" : $"Player [{player.playerNumber}] has won. You lose :(";
            CanvasUI.instance.SetAndShowResultText(resultText, player.playerColor);
            StartCoroutine(RestartMatch());
        }
    }

    private IEnumerator RestartMatch()
    {
        for (int i = restartMatchDelay; i > 0; i--)
        {
            CanvasUI.instance.SetAndShowRestartText("Next match starts in " + i + "...");
            yield return new WaitForSeconds(1);
        }
        if (isServer)
        {
            NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
}
