using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    [Header("Movement")]
    public float moveSpeed;
    public float jumpForce;

    [Header("Camera")]
    public float lookSensitivity;       // mouse look sensitivity
    public float maxLookX;              // lowest down we can look
    public float minLookX;              // highest up we can look
    private float rotX;                 // current x rotation of the camera

    private Camera cam;
    private Rigidbody rig;

    void Awake()
    {
        //get the components
        cam = Camera.main;
        rig = GetComponent<Rigidbody>();

        // disable cursor
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        Move();
        CamLook();
    }

    void Move()
    {
        //gets the x an z axis inputs
        float x = Input.GetAxis("Horizontal") * moveSpeed;
        float z = Input.GetAxis("Vertical") * moveSpeed;

        //adds inputs to body velocity
        rig.velocity = new Vector3(x, rig.velocity.y, z);
    }

    // rotate the camera based on mouse movement
    void CamLook ()
    {
        // get mouse inputs
        float y = Input.GetAxis("Mouse X") * lookSensitivity;
        rotX += Input.GetAxis("Mouse Y") * lookSensitivity;

        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minLookX, maxLookX);

        // rotate the camera and player
        cam.transform.localRotation = Quaternion.Euler(-rotX, 0, 0);
        transform.eulerAngles += Vector3.up * y;
    }

}
