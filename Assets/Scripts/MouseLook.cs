using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour { 

    public enum RotationAxes {
        MouseXandY = 0,
        MouseX = 1,
        MouseY = 2
    }

    public RotationAxes axes = RotationAxes.MouseXandY;
    
    public float sensivityHor = 9.0f;
    public float sensivityVer = 9.0f;

    public float minimumVert = -45.0f;
    public float maximumVert = 45.0f;

    private float _rotationX = 0;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        switch (axes)
        {
            case RotationAxes.MouseX:
                transform.Rotate(0, Input.GetAxis("Mouse X") * sensivityHor, 0);
                break;
            case RotationAxes.MouseY:
                _rotationX -= Input.GetAxis("Mouse Y") * sensivityVer;
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

                float rotationY = transform.localEulerAngles.y;

                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);
                break;
            default:
                _rotationX -= Input.GetAxis("Mouse Y") * sensivityVer;
                _rotationX = Mathf.Clamp(_rotationX, minimumVert, maximumVert);

                float delta = Input.GetAxis("Mouse X") * sensivityHor;
                rotationY = transform.localEulerAngles.y + delta;

                transform.localEulerAngles = new Vector3(_rotationX, rotationY, 0);

                break;
        }
    }
}
