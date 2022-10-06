using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerUI2 : MonoBehaviour
{
    /*#region Singleton
    public static PlayerUI2 instance;

    private void Awake()
    {
        instance = this;
    }
    #endregion*/

    [SerializeField]
    private Text matchResultText;

    [SerializeField]
    private Text matchRestartText;

    private void Start()
    {
        matchResultText.gameObject.SetActive(false);
        matchRestartText.gameObject.SetActive(false);
    }

    public void SetAndShowResultText(string value)
    {
        matchResultText.text = value;
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
