using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using Mirror.Examples.Basic;

[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkTransform))]
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : NetworkBehaviour
{
    public CharacterController characterController;

    [Header("Movement Settings")]
    private float defaultMoveSpeed = 8f;
    public float moveSpeed = 8f;
    public float turnSensitivity = 100f;
    public float maxTurnSpeed = 100f;
    public Vector3 moveDir;
    public float turnSmoothTime = 0.1f;
    float turnSmoothVelocity;
    public Transform cam;

    [Header("Diagnostics")]
    public float horizontal;
    public float vertical;
    public float turn;
    public float jumpSpeed;
    public bool isGrounded = true;
    public bool isFalling;
    public Vector3 velocity;


    [Header("Dash")]
    [SerializeField] private float maxDashDistant = 5.0f;
    private float _dashSpeed;
    private float _dashTime = 0.2f;
    [SyncVar] private bool isDashing = false;
    [SyncVar] private int _score = 0;

    private void Start()
    {
        Cursor.visible = false;
        _dashSpeed = maxDashDistant / _dashTime;
    }

    void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();

        characterController.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        GetComponent<NetworkTransform>().clientAuthority = true;
    }

    public override void OnStartLocalPlayer()
    {
        characterController.enabled = true;
    }

    void Update()
    {
        if (!isLocalPlayer || characterController == null || !characterController.enabled)
            return;

        transform.rotation = Quaternion.Euler(0f, cam.eulerAngles.y, 0f);

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + cam.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveDir = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        if (dir.magnitude >= 0.1f)
        {
            
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        }

        //turn = Mathf.MoveTowards(turn, Input.GetAxis("Mouse X") * maxTurnSpeed, turnSensitivity);

        //if (Input.GetMouseButtonDown(0))
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isDashing)
            {
                SetIsDashing(true);
                StartCoroutine(Dash());
            }
        }

        /*if (!isLocalPlayer || characterController == null || !characterController.enabled)
            return;

        transform.Rotate(0f, turn * Time.fixedDeltaTime, 0f);

        Vector3 direction = new Vector3(horizontal, 0, vertical);
        direction = Vector3.ClampMagnitude(direction, 1f);
        direction = transform.TransformDirection(direction);
        direction *= moveSpeed;
        */



        /*if (jumpSpeed > 0)
            characterController.Move(direction * Time.deltaTime);
        else if (isDashing)
        {
            characterController.Move(direction * Time.deltaTime);
        }
        else
            characterController.SimpleMove(direction);*/
        
        
        /*CmdMovePlayer(direction);

        isGrounded = characterController.isGrounded;
        velocity = characterController.velocity;*/
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
            characterController.Move(moveDir * _dashSpeed * Time.deltaTime);
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
            /*_score++;
            ScoreManager.Instance.UpdateScore(NetworkClient.localPlayer.netId.ToString(), _score);*/
            GetComponent<Player>().UpdateScore();
            SendMessage();
            collidePlayer.ProcessDashCollide();
            NetworkIdentity dashingPlayer = collidePlayer.GetComponent<NetworkIdentity>();
            //RpcProcessDash(dashingPlayer.connectionToClient);
            RpcProcessDash();
        }
        isDashing = false;
    }

    private void SendMessage()
    {
        Debug.Log("SendMessage");
        //ScoreManager.Instance.CmdSend("" + Time.time);
    }

    [ClientRpc]
    public void RpcProcessDash()
    {
        Debug.Log("You have been dashed");
    }
}
