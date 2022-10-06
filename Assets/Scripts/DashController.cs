using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;

public class DashController : NetworkBehaviour
{
    [SerializeField] private KeyCode activationButton = KeyCode.Mouse0;

    #region Invulnerability params
    [Header("Invulnerability params")]
    [SerializeField] private float invulnerabilityTime = 3.0f;
    [SerializeField] private Color invulnerabilityColor = Color.red;
    [SyncVar] private float startChangeColorTime = 0;
    #endregion

    [Header("Dash")]
    [SerializeField] private float maxDashDistant = 5.0f;
    private float _dashSpeed;
    private float _dashTime = 0.2f;
    [SyncVar] private bool isDashing = false;

    [SyncVar] private bool _isDashed = false;
    public bool isDashed => _isDashed;
    private Color defaultColor;
    private MeshRenderer renderer;

    public CharacterController characterController;
    public PlayerController playerController;

    private void Start()
    {
        renderer = GetComponent<MeshRenderer>();
        defaultColor = renderer.material.color;
        _dashSpeed = maxDashDistant / _dashTime;
    }

    public void ProcessDashCollide()
    {
        if (!_isDashed)
        {
            _isDashed = true;
            startChangeColorTime = Time.time;
            RpcChangeColor(invulnerabilityColor);
        }        
    }

    [ClientRpc]
    private void RpcChangeColor(Color color)
    {
        renderer.material.color = color;
    }

    private void Update()
    {
        if (Input.GetKeyDown(activationButton))
        {
            if (playerController.isMoving && !isDashing)
            {
                SetIsDashing(true);
                StartCoroutine(Dash());
            }
        }
        if (_isDashed && Time.time - startChangeColorTime >= invulnerabilityTime)
        {
            CmdCheckAndSetIsDashed(false);
        }
    }

    [Command]
    private void CmdCheckAndSetIsDashed(bool value)
    {
        if (Time.time - startChangeColorTime >= invulnerabilityTime)
        {
            _isDashed = value;
            RpcChangeColor(defaultColor);
        }
    }

    [Command]
    private void SetIsDashing(bool value)
    {
        isDashing = value;
    }

    private IEnumerator Dash()
    {
        float startTime = Time.time;

        while (Time.time < startTime + _dashTime)
        {
            characterController.Move(playerController.moveDir * _dashSpeed * Time.deltaTime);
            yield return null;
        }

        SetIsDashing(false);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        GameObject target = hit.transform.gameObject;
        DashController collidePlayer = target.GetComponent<DashController>();
        ProcessDash(collidePlayer);
    }

    [Command]
    private void ProcessDash(DashController collidePlayer)
    {
        if (collidePlayer && isDashing && !collidePlayer.isDashed)
        {
            //GetComponent<Player>().UpdateScore();
            GetComponent<PlayerScore>().score++;
            collidePlayer.ProcessDashCollide();
            NetworkIdentity dashingPlayer = collidePlayer.GetComponent<NetworkIdentity>();
        }
        isDashing = false;
    }

    private void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
        if (playerController == null)
            playerController = GetComponent<PlayerController>();
    }
}
