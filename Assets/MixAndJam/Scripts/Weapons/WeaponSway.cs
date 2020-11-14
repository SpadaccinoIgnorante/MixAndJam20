using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponSway : MonoBehaviour
{
    public float intensity;
    public float smooth;

    private Quaternion originRotation;

    private void Start()
    {
        originRotation = transform.localRotation;
    }

    private void Update()
    {
        UpdateSway();
    }

    private void UpdateSway()
    {
        float mouseX = InputManager.hRightAxis;
        float mouseY = InputManager.vRightAxis;
        
        Quaternion xAdj = Quaternion.AngleAxis(intensity * mouseX, Vector3.up);
        Quaternion yAdj = Quaternion.AngleAxis(intensity * mouseY, Vector3.right);
        Quaternion targetRotation = originRotation * xAdj * yAdj;

        transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, Time.deltaTime * smooth);
    }
}
