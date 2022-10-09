using UnityEngine;
using UnityEngine.UI;

public class CanvasUI : MonoBehaviour
{
    #region Singleton

    public static CanvasUI instance;

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
    private GameObject endMatchPanel;

    [SerializeField]
    private Text matchResultText;

    [SerializeField]
    private Text matchRestartText;

    private void Start()
    {
        endMatchPanel.SetActive(false);
    }

    public void RefreshPlayerScore(int value)
    {
        playerScoreText.text = "Your score: " + value;
    }

    public void SetAndShowResultText(string value, Color color)
    {
        matchResultText.text = value;
        matchResultText.color = color;
        endMatchPanel.SetActive(true);
    }

    public void SetAndShowRestartText(string value)
    {
        matchRestartText.text = value;
    }

}
