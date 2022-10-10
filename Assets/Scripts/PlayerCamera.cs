using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

[RequireComponent(typeof(MoveController))]
public class PlayerCamera : NetworkBehaviour
{
    private Camera playerCamera;
    private float mouseSensitivity = 2.0f;
    private float rotationX;
    private float rotationY;
    private float distanceFromTarget;
    private Vector3 currentRotation;
    private Vector3 smoothVelocity = Vector3.zero;
    private float smoothTime = 0.2f;
    private Vector2 rotationXMinMax = new Vector2(-10, 60);
    private MoveController moveController;

    private void Awake()
    {
        moveController = GetComponent<MoveController>();
    }

    private void Start()
    {
        if (isClient && isLocalPlayer)
        {
            playerCamera = Camera.main;
            moveController.playerCamera = playerCamera.transform;

            playerCamera.orthographic = false;
            playerCamera.transform.SetParent(transform);
            playerCamera.transform.localPosition = new Vector3(0f, 5f, 3f);

            distanceFromTarget = playerCamera.transform.localPosition.magnitude;

            playerCamera.transform.SetParent(null);
        }
    }

    public override void OnStopLocalPlayer()
    {
        if (playerCamera != null)
        {
            playerCamera.transform.SetParent(null);
            moveController.playerCamera = null;
            SceneManager.MoveGameObjectToScene(playerCamera.gameObject, SceneManager.GetActiveScene());
            playerCamera.orthographic = true;
            playerCamera.transform.localPosition = new Vector3(0f, 70f, 0f);
            playerCamera.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }

    private void Update()
    {
        rotationY += Input.GetAxis("Mouse X") * mouseSensitivity;
        rotationX -= Input.GetAxis("Mouse Y") * mouseSensitivity;
        rotationX = Mathf.Clamp(rotationX, rotationXMinMax.x, rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(rotationX, rotationY);

        currentRotation = Vector3.SmoothDamp(currentRotation, nextRotation, ref smoothVelocity, smoothTime);
        if (playerCamera)
        {
            playerCamera.transform.eulerAngles = currentRotation;
            playerCamera.transform.position = transform.position - playerCamera.transform.forward * distanceFromTarget;
        }
    }

}
