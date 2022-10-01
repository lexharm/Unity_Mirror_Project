using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    #region Singleton
    private static ScoreManager _inscance;
    public static ScoreManager Instance => _inscance;
    #endregion

    private SyncDictionary<string, int> playersScores = new SyncDictionary<string, int>();

    private void Awake()
    {
        _inscance = this;
    }

    [Command(requiresAuthority = false)]
    public void CmdUpdateScore(string playerName, int score)
    {
        if (playersScores.ContainsKey(playerName))
        {
            playersScores[playerName] = score;
        } else
        {
            playersScores.Add(playerName, score);
        }

        string scoreText = "";
        foreach (KeyValuePair<string, int> entry in playersScores)
        {
            scoreText += entry.Key + ": " + entry.Value + " | ";
        }

        scoreText = scoreText.Substring(0, scoreText.Length - 3);

        UIManager.Instance.UpdateScoreText(scoreText);
    }
}
