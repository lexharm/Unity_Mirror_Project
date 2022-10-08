using System.Collections;
using UnityEngine;
using Mirror;

public class DashController : NetworkBehaviour
{
    #region Dash params
    [Header("Dash params")]
    [SerializeField] private KeyCode activationButton = KeyCode.Mouse0;

    [Tooltip("Distant which player dashes.")]
    [SerializeField] private float dashDistant = 10.0f;

    [Tooltip("Time during which player dashes.")]
    [SerializeField] private float dashTime = 0.2f;
    private float dashSpeed => dashDistant / dashTime;
    [SyncVar] private bool isDashing = false;
    #endregion

    #region Invulnerability params
    [Header("Invulnerability params")]
    [Tooltip("During this time player will not react to the dash.")]
    [SerializeField] private float invulnerabilityTime = 3.0f;
    
    [Tooltip("Player's color during he is dashed by other player.")]
    [SerializeField] private Color invulnerabilityColor = Color.red;
    
    [SyncVar] private float startInvulnerabilityTime = 0;
    [SyncVar] private bool isDashed = false;
    public bool IsDashed => isDashed;
    #endregion

    private Player2 player;
    private MoveController moveController;
    private CharacterController characterController;

    private void Start()
    {
        player = GetComponent<Player2>();
        moveController = GetComponent<MoveController>();
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationButton))
        {
            if (moveController.isMoving && !isDashing)
            {
                CmdSetIsDashing(true);
                StartCoroutine(Dash());
            }
        }
        if (isDashed && Time.time - startInvulnerabilityTime >= invulnerabilityTime)
        {
            CmdCheckAndSetIsDashed(false);
        }
    }

    [Command]
    private void CmdSetIsDashing(bool value)
    {
        isDashing = value;
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + dashTime)
        {
            characterController.Move(dashSpeed * Time.deltaTime * moveController.moveDir);
            yield return null;
        }

        CmdSetIsDashing(false);
    }

    [Command]
    private void CmdCheckAndSetIsDashed(bool value)
    {
        if (Time.time - startInvulnerabilityTime >= invulnerabilityTime)
        {
            isDashed = value;
            player.RpcSetDefaultColor();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.transform.gameObject;
        DashController collidePlayer = target.GetComponent<DashController>();
        CmdProcessDash(collidePlayer);
    }

    [Command]
    private void CmdProcessDash(DashController collidePlayer)
    {
        if (collidePlayer && !collidePlayer.IsDashed && isDashing)
        {
            //GetComponent<Player>().UpdateScore();
            player.score++;
            collidePlayer.ProcessDashCollide();
        }
        isDashing = false;
    }

    public void ProcessDashCollide()
    {
        if (!isDashed)
        {
            isDashed = true;
            startInvulnerabilityTime = Time.time;
            player.RpcChangeColor(invulnerabilityColor);
        }        
    }
}
