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
    private bool _isMoving;
    public bool isMoving => _isMoving;

    /*[Header("Dash")]
    [SerializeField] private float maxDashDistant = 5.0f;
    private float _dashSpeed;
    private float _dashTime = 0.2f;
    [SyncVar] private bool isDashing = false;*/

    private void Start()
    {
        Cursor.visible = false;
        //_dashSpeed = maxDashDistant / _dashTime;
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
            _isMoving = true;
            characterController.Move(moveDir.normalized * moveSpeed * Time.deltaTime);
        } else
        {
            _isMoving = false;
        }

        /*if (Input.GetMouseButtonDown(0))
        {
            if (!isDashing)
            {
                SetIsDashing(true);
                StartCoroutine(Dash());
            }
        }*/
    }
    
    /*[Command]
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
            //GetComponent<Player>().UpdateScore();
            GetComponent<PlayerScore>().score++;
            collidePlayer.ProcessDashCollide();
            NetworkIdentity dashingPlayer = collidePlayer.GetComponent<NetworkIdentity>();
        }
        isDashing = false;
    }*/
}
