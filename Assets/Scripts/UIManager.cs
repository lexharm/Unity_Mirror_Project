using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    #region Singleton
    private static UIManager _inscance;
    public static UIManager Instance => _inscance;
    #endregion

    [SerializeField] private GameObject scorePanel;
    [SerializeField] private Text scoreText;

    private void Awake()
    {
        _inscance = this;
    }

    public void ToggleScore()
    {
        scorePanel.SetActive(!scorePanel.activeSelf);
    }

    public void UpdateScoreText(string newValue)
    {
        scoreText.text = newValue;
    }
}
