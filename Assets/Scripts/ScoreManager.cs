using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Mirror;

public class ScoreManager : NetworkBehaviour
{
    #region Singleton
    private static ScoreManager _inscance;
    public static ScoreManager Instance => _inscance;
    #endregion

    private SyncDictionary<string, int> playersScores = new SyncDictionary<string, int>();
    [SerializeField] private Text scoreText;

    public static ScoreManager _instance;
    void Awake()
    {
        _inscance = this;
        if (scoreText)
            Debug.Log("scoreText not null");
        else
            Debug.Log("scoreText is null");
    }

    public void UpdateScore(string playerName, int score)
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
    }


    Dictionary<NetworkConnectionToClient, string> connNames = new Dictionary<NetworkConnectionToClient, string>();

    [Command(requiresAuthority = false)]
    public void CmdSend(string message, NetworkConnectionToClient sender = null)
    {
        Debug.Log("CmdSend");
        if (!connNames.ContainsKey(sender))
        {
            Debug.Log("Add sender");
            //connNames.Add(sender, sender.identity.GetComponent<Player>().playerName);
            connNames.Add(sender, sender.identity.netId.ToString());
        }

        if (!string.IsNullOrWhiteSpace(message))
        {
            Debug.Log("RpcReceive invoke");
            RpcReceive(connNames[sender], message.Trim());
        }
    }

    [ClientRpc]
    public void RpcReceive(string playerName, string message)
    {
        StartCoroutine(SendInfo(message));
    }

    IEnumerator SendInfo(string message)
    {
        Debug.Log(message);
        scoreText.text = message;

        // it takes 2 frames for the UI to update ?!?!
        yield return null;
        yield return null;
    }
}
