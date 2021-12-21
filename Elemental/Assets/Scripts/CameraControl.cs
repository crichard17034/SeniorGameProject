using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{
    public float mouseSensitivity = 550f; //how quick the player can move the mouse and the camera
    public Transform playerBody;
    float xRotation = 0f;

    void Start() 
    {
        Cursor.lockState = CursorLockMode.Locked; //This lock the cursor to the middle of the screen
        Cursor.visible = false;
    }

    /* This code allows the camera movement to be mapped to mouse movement while also
     * preventing the camera from bending beyound 180 degrees and clip behind the player.
     * The Time.deltaTime prevents the camera from scrolling faster than intended on 
     * higher framerates.
     */

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime; 
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
