using UnityEngine;
using UnityEngine.UI;

namespace Mirror.Examples.Basic
{
    public class PlayerUI : MonoBehaviour
    {
        [Header("Player Components")]
        public Image image;

        [Header("Child Text Objects")]
        public Text playerNameText;
        public Text playerDataText;

        // Sets a highlight color for the local player
        public void SetLocalPlayer()
        {
            // add a visual background for the local player in the UI
            image.color = new Color(1f, 1f, 1f, 0.1f);
        }

        // This value can change as clients leave and join
        public void OnPlayerNumberChanged(byte newPlayerNumber)
        {
            playerNameText.text = string.Format("Player", newPlayerNumber);
        }

        // Random color set by Player::OnStartServer
        public void OnPlayerColorChanged(Color32 newPlayerColor)
        {
            playerNameText.color = newPlayerColor;
        }

        // This updates from Player::UpdateData via InvokeRepeating on server
        public void OnPlayerScoreChanged(int newPlayerScore)
        {
            // Show the data in the UI
            playerDataText.text = string.Format("{0}", newPlayerScore);
        }

        public void OnPlayerWin(bool isPlayerWins)
        {
            // Show the data in the UI
            if (isPlayerWins)
                playerDataText.text = "WINNER!!!";
        }

    }
}
