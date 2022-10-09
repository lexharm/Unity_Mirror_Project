using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI : MonoBehaviour
{
    [Header("Player Components")]
    public Image image;

    [Header("Child Text Objects")]
    public Text playerNameText;
    public Text playerDataText;

    public void SetLocalPlayer()
    {
        image.color = new Color(1f, 1f, 1f, 0.1f);
    }

    public void OnPlayerNumberChanged(int newPlayerNumber)
    {
        playerNameText.text = string.Format("Player [{00}]", newPlayerNumber);
    }

    public void OnPlayerColorChanged(Color newPlayerColor)
    {
        playerNameText.color = newPlayerColor;
    }

    public void OnPlayerScoreChanged(Player2 player)
    {
        playerDataText.text = string.Format("Score: {0}", player.score);
    }

}
