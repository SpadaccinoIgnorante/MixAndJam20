using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponsManager : MonoBehaviour
{
    public GameObject bulletPrefab;

    public Transform firingPoint;
    public GameObject suckTrigger;

    public float shootForce = 10;
    public int ammo;
    public float firingTime;
    public float reloadTime;

    public float suckLength = 1;
    public float suckXRadius = 1;
    public float suckYRadius = 1;

    private int maxAmmo;
    private float firingTimer;
    private float reloadTimer;
    private bool isReloading;

    private void Start()
    {
        maxAmmo = ammo;
        suckTrigger.transform.localScale = Vector3.one;
    }

    private void Update()
    {

        if (firingTimer > 0)
        {
            firingTimer -= Time.deltaTime;
        }

        // Attack
        if (InputManager.rTrigger)
        {
            if (ammo > 0)
            {
                if (firingTimer <= 0)
                {
                    GameObject bullet = Instantiate(bulletPrefab, firingPoint.position, firingPoint.rotation);
                    bullet.GetComponent<Rigidbody>().velocity = firingPoint.TransformDirection(Vector3.forward * shootForce);
                    firingTimer = firingTime;
                    ammo--;
                }
            } 
            else
            {
                if (!isReloading)
                    Reload();
            }
        }

        //Suck
        if (InputManager.lTrigger)
        {
            suckTrigger.SetActive(true);
        } else
        {
            suckTrigger.SetActive(false);
        }

        suckTrigger.transform.localScale = new Vector3(suckXRadius, suckYRadius, suckLength);

        if (reloadTimer <= 0)
        {
            if (isReloading)
            {
                if (ammo <= 0)
                {
                    ammo = maxAmmo;
                    isReloading = false;
                }
            }
        }
        else
        {
            reloadTimer -= Time.deltaTime;
        }
    }

    public void Reload()
    {
        reloadTimer = reloadTime;
        isReloading = true;
    }
}
