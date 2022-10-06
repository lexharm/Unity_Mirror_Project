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

    [Tooltip("Delay in seconds before restart match.")]
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

    private void Start()
    {
        //playerUI = FindObjectOfType<PlayerUI2>();
        //playerUI = PlayerUI2.Instance;
    }

    public void RegisterPlayerScore(PlayerScore playerScore)
    {
        playerScore.OnScoreChanged += OnScoreChanged;
    }

    private void OnScoreChanged(PlayerScore playerScore)
    {
        Debug.Log("OnScoreChanged");
        if (playerScore.score >= scoreValueToWin)
        {
            string resultText = playerScore.isLocalPlayer ? "You win!" : "You lose :(";
            //playerScore.playerUI.SetAndShowResultText(resultText);
            //matchResultText.text = resultText;
            playerUI.SetAndShowResultText(resultText);

            StartCoroutine(RestartMatch());
        }
    }

    private IEnumerator RestartMatch()
    {
        Debug.Log("StartCoroutine");
        for (int i = restartMatchDelay; i > 0; i--)
        {
            yield return new WaitForSeconds(1);
            playerUI.SetAndShowRestartText("Next match starts in " + i + "...");
            //matchRestartText.text = "Next match starts in " + i + "...";
            Debug.Log("Next match starts in " + i + "...");
        }
        if (isServer)
        {
            Debug.Log("isServer");
            NetworkManager.singleton.ServerChangeScene(SceneManager.GetActiveScene().name);
        }
    }
}
