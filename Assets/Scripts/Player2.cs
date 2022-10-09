using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player2 : NetworkBehaviour
{
    // Players List to manage playerNumber
    static readonly List<Player2> playersList = new List<Player2>();

    /// <summary>
    /// This is appended to the player name text, e.g. "Player 01"
    /// </summary>
    [SyncVar(hook = nameof(PlayerNumberChanged))]
    public int playerNumber = 0;

    [SerializeField]
    [SyncVar(hook = nameof(PlayerScoreChanged))]
    private int playerScore = 0;
    public int score {
        get => playerScore;
        set
        {
            playerScore = value;
            OnPlayerScoreChanged?.Invoke(this);
        }
    }

    [SyncVar(hook = nameof(PlayerColorChanged))]
    public Color playerColor = Color.white;

    public event System.Action<int> OnPlayerNumberChanged;
    public event System.Action<Player2> OnPlayerScoreChanged;
    public event System.Action<Color> OnPlayerColorChanged;

    [Header("Player UI")]
    public GameObject playerUIPrefab;

    GameObject playerUIObject;
    PlayerUI playerUI = null;

    private Color defaultColor;
    private MeshRenderer renderer;

    private void Awake()
    {
        renderer = GetComponent<MeshRenderer>();
    }

    private void Start()
    {
        Debug.Log("Start");
        CmdSetPlNum();
        defaultColor = renderer.material.color;
        FindObjectOfType<MatchManager>().RegisterPlayerScore(this);
    }

    [Command]
    private void CmdSetPlNum()
    {
        int idx = playersList.Count + 1;
        playerNumber = idx;
        playersList.Add(this);
    }

    public override void OnStartLocalPlayer()
    {
        playerUI.SetLocalPlayer();

        CanvasUI2.instance.mainPanel.gameObject.SetActive(true);
    }

    public override void OnStopLocalPlayer()
    {
        CanvasUI2.instance.mainPanel.gameObject.SetActive(false);
    }

    // This is called by the hook of playerNumber SyncVar above
    void PlayerNumberChanged(int _, int newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }

    private void PlayerScoreChanged(int _, int newValue)
    {
        score = newValue;
        if (isLocalPlayer)
        {
            CanvasUI2.instance.RefreshPlayerScore(playerScore);
        }
    }

    void PlayerColorChanged(Color _, Color newPlayerColor)
    {
        renderer.material.color = newPlayerColor;
        OnPlayerColorChanged?.Invoke(newPlayerColor);
    }

    [ClientRpc]
    public void RpcChangeColor(Color color)
    {
        renderer.material.color = color;
    }

    [ClientRpc]
    public void RpcSetDefaultColor()
    {
        renderer.material.color = defaultColor;
    }

    public override void OnStartClient()
    {
        // Instantiate the player UI as child of the Players Panel
        playerUIObject = Instantiate(playerUIPrefab, CanvasUI2.instance.playersPanel);
        playerUI = playerUIObject.GetComponent<PlayerUI>();

        // wire up all events to handlers in PlayerUI
        OnPlayerNumberChanged = playerUI.OnPlayerNumberChanged;
        OnPlayerColorChanged = playerUI.OnPlayerColorChanged;
        OnPlayerScoreChanged += playerUI.OnPlayerScoreChanged;

        // Invoke all event handlers with the initial data from spawn payload
        OnPlayerNumberChanged.Invoke(playerNumber);
        OnPlayerColorChanged.Invoke(playerColor);
        OnPlayerScoreChanged.Invoke(this);
    }

    public override void OnStopClient()
    {
        // disconnect event handlers
        OnPlayerNumberChanged = null;
        OnPlayerColorChanged = null;
        OnPlayerScoreChanged = null;

        // Remove this player's UI object
        Destroy(playerUIObject);
    }

    public override void OnStopServer()
    {
        CancelInvoke();
        playersList.Remove(this);
    }

    public override void OnStartServer()
    {
        base.OnStartServer();

        // set the Player Color SyncVar
        playerColor = Random.ColorHSV(0f, 1f, 0.9f, 0.9f, 1f, 1f);
        renderer.material.color = playerColor;
    }

}
