using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasUI2 : MonoBehaviour
{
    #region Singleton
    public static CanvasUI2 instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion

    [Tooltip("Assign Main Panel so it can be turned on from Player:OnStartClient")]
    public RectTransform mainPanel;

    [Tooltip("Assign Players Panel for instantiating PlayerUI as child")]
    public RectTransform playersPanel;

    [SerializeField]
    private Text playerScoreText;

    [SerializeField]
    private Text matchResultText;

    [SerializeField]
    private Text matchRestartText;

    private void Start()
    {
        matchResultText.gameObject.SetActive(false);
        matchRestartText.gameObject.SetActive(false);
    }

    public void RefreshPlayerScore(int value)
    {
        playerScoreText.text = "Your score: " + value;
    }

    public void SetAndShowResultText(string value, Color color)
    {
        matchResultText.text = value;
        matchResultText.color = color;
        matchResultText.gameObject.SetActive(true);
    }

    public void SetAndShowRestartText(string value)
    {
        matchRestartText.text = value;
        if (!matchRestartText.gameObject.activeSelf)
        {
            matchRestartText.gameObject.SetActive(true);
        }
    }
}
