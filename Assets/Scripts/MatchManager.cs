using System.Collections;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class MatchManager : NetworkBehaviour
{
    #region Singleton

    public static MatchManager instance;

    private void Awake()
    {
        instance = this;
    }

    #endregion

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

    public void RegisterPlayerScore(Player player)
    {
        player.OnPlayerScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(Player player)
    {
        if (player.Score >= scoreValueToWin)
        {
            string resultText = player.isLocalPlayer ? "You won!" : $"Player [{player.playerNumber}] has won. You lose :(";
            if (!CanvasUI.instance.IsMatchPanelActive())
            {
                StartCoroutine(RestartMatch(resultText, player.playerColor));
            }
        }
    }

    private IEnumerator RestartMatch(string resultText, Color color)
    {
        CanvasUI.instance.SetAndShowResultText(resultText, color);

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
