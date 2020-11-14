using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float destroyTime = 5f;
    public float damage;

    private bool canHarm;

    private void Start()
    {
        canHarm = true;
        Destroy(gameObject, destroyTime);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.GetComponent<SuckableObject>() != null)
        {
            if (canHarm)
            {
                Destroy(gameObject);
                collision.gameObject.GetComponent<SuckableObject>().SetDamage(damage);
            }
        }

        if (collision.gameObject.layer == LayerMask.NameToLayer("Floor"))
        {
            canHarm = false;
        }
    }
}
