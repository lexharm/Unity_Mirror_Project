using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Mirror;

public class PlayerCamera : NetworkBehaviour
{
    private Camera mainCam;
    private float _mouseSensitivity = 2.0f;
    private float _rotationY;
    private float _rotationX;
    private float _distanceFromTarget;
    private Vector3 _currentRotation;
    private Vector3 _smoothVelocity = Vector3.zero;
    private float _smoothTime = 0.2f;
    private Vector2 _rotationXMinMax = new Vector2(-5, 45);

    void Start()
    {
        if (isClient && isLocalPlayer)
        {
            mainCam = Camera.main;
            GetComponent<PlayerController>().cam = mainCam.transform;

            mainCam.orthographic = false;
            mainCam.transform.SetParent(transform);
            mainCam.transform.localPosition = new Vector3(0f, 3f, -5f);
            mainCam.transform.localEulerAngles = new Vector3(10f, 0f, 0f);

            _distanceFromTarget = mainCam.transform.localPosition.magnitude;

            mainCam.transform.SetParent(null);
        }
    }

    public override void OnStopLocalPlayer()
    {
        if (mainCam != null)
        {
            mainCam.transform.SetParent(null);
            GetComponent<PlayerController>().cam = null;
            SceneManager.MoveGameObjectToScene(mainCam.gameObject, SceneManager.GetActiveScene());
            mainCam.orthographic = true;
            mainCam.transform.localPosition = new Vector3(0f, 70f, 0f);
            mainCam.transform.localEulerAngles = new Vector3(90f, 0f, 0f);
        }
    }

    private void Update()
    {
        _rotationY += Input.GetAxis("Mouse X") * _mouseSensitivity;
        _rotationX -= Input.GetAxis("Mouse Y") * _mouseSensitivity;
        _rotationX = Mathf.Clamp(_rotationX, _rotationXMinMax.x, _rotationXMinMax.y);

        Vector3 nextRotation = new Vector3(_rotationX, _rotationY);

        _currentRotation = Vector3.SmoothDamp(_currentRotation, nextRotation, ref _smoothVelocity, _smoothTime);
        if (mainCam)
        {
            mainCam.transform.eulerAngles = _currentRotation;
            mainCam.transform.position = transform.position - mainCam.transform.forward * _distanceFromTarget;
        }
    }
}
