using UnityEngine;
using Mirror;

[RequireComponent(typeof(CharacterController))]
[RequireComponent(typeof(NetworkTransform))]
public class MoveController : NetworkBehaviour
{

    [SerializeField] private float moveSpeed = 8.0f;

    public Transform playerCamera;

    private Vector3 moveDirection;
    public Vector3 MoveDirection => moveDirection;

    private bool isMoving;
    public bool IsMoving => isMoving;

    private float turnSmoothTime = 0.1f;
    private float turnSmoothVelocity;
    private float horizontal;
    private float vertical;
    private CharacterController characterController;

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        GetComponent<NetworkTransform>().clientAuthority = true;
    }

    private void Update()
    {
        if (!isLocalPlayer || characterController == null || !characterController.enabled)
            return;

        transform.rotation = Quaternion.Euler(0f, playerCamera.eulerAngles.y, 0f);

        horizontal = Input.GetAxisRaw("Horizontal");
        vertical = Input.GetAxisRaw("Vertical");

        Vector3 dir = new Vector3(horizontal, 0, vertical).normalized;

        float targetAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg + playerCamera.eulerAngles.y;
        float angle = Mathf.SmoothDampAngle(transform.eulerAngles.y, targetAngle, ref turnSmoothVelocity, turnSmoothTime);
        transform.rotation = Quaternion.Euler(0f, angle, 0f);

        moveDirection = Quaternion.Euler(0, targetAngle, 0) * Vector3.forward;

        if (dir.magnitude >= 0.1f)
        {
            isMoving = true;
            characterController.Move(moveSpeed * Time.deltaTime * moveDirection.normalized);
        } else
        {
            isMoving = false;
        }
    }

}
