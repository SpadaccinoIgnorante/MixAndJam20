using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : MonoBehaviour
{
    public float mouseSensitivityX = 10f;
    public float mouseSensitivityY = 10f;

    public float verticalViewClamp = 75;

    public Transform player;

    float xRotation = 0f;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void Update()
    {
        float mouseX = InputManager.hRightAxis * mouseSensitivityX * Time.deltaTime;
        float mouseY = InputManager.vRightAxis * mouseSensitivityY * Time.deltaTime;

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -verticalViewClamp, verticalViewClamp);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseX);
    }
}
