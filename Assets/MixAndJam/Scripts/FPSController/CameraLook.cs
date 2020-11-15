using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLook : BehaviourBase
{
    public float xMouseSensitivity = 10f;
    public float yMouseSensitivity = 10f;
    public float hControllerSensitivity = 10f;
    public float vControllerSensitivity = 10f;

    public Transform player;

    float xRotation = 0f;

    protected override void CustomFixedUpdate() { }

    protected override void CustomUpdate()
    {
        float mouseX = 0, mouseY = 0;
        if (InputManager.isUsingController())
        {
            mouseX = InputManager.hRightAxis * Time.deltaTime * hControllerSensitivity;
            mouseY = InputManager.vRightAxis * Time.deltaTime * vControllerSensitivity;
        }
        else
        {
            mouseX = InputManager.hRightAxis * Time.deltaTime * xMouseSensitivity;
            mouseY = InputManager.vRightAxis * Time.deltaTime * yMouseSensitivity;
        }

        xRotation -= mouseY;

        xRotation = Mathf.Clamp(xRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(xRotation, 0, 0);

        player.Rotate(Vector3.up * mouseX);
    }

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

}
