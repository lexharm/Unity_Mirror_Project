using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class Player : NetworkBehaviour
{
    // Players List to manage playerNumber
    static readonly List<Player> playersList = new List<Player>();

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
    public event System.Action<Player> OnPlayerScoreChanged;
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
        CmdSetPlayerNumber();
        defaultColor = renderer.material.color;
        FindObjectOfType<MatchManager>().RegisterPlayerScore(this);
    }

    [Command]
    private void CmdSetPlayerNumber()
    {
        int idx = playersList.Count + 1;
        playerNumber = idx;
        playersList.Add(this);
    }

    public override void OnStartLocalPlayer()
    {
        playerUI.SetLocalPlayer();

        CanvasUI.instance.mainPanel.gameObject.SetActive(true);
    }

    public override void OnStopLocalPlayer()
    {
        CanvasUI.instance.mainPanel.gameObject.SetActive(false);
    }

    void PlayerNumberChanged(int _, int newPlayerNumber)
    {
        OnPlayerNumberChanged?.Invoke(newPlayerNumber);
    }

    private void PlayerScoreChanged(int _, int newValue)
    {
        score = newValue;
        if (isLocalPlayer)
        {
            CanvasUI.instance.RefreshPlayerScore(playerScore);
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
        playerUIObject = Instantiate(playerUIPrefab, CanvasUI.instance.playersPanel);
        playerUI = playerUIObject.GetComponent<PlayerUI>();

        OnPlayerNumberChanged = playerUI.OnPlayerNumberChanged;
        OnPlayerColorChanged = playerUI.OnPlayerColorChanged;
        OnPlayerScoreChanged += playerUI.OnPlayerScoreChanged;

        OnPlayerNumberChanged.Invoke(playerNumber);
        OnPlayerColorChanged.Invoke(playerColor);
        OnPlayerScoreChanged.Invoke(this);
    }

    public override void OnStopClient()
    {
        OnPlayerNumberChanged = null;
        OnPlayerColorChanged = null;
        OnPlayerScoreChanged = null;

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

        playerColor = Random.ColorHSV(0f, 1f, 0.9f, 0.9f, 1f, 1f);
        renderer.material.color = playerColor;
    }

}
