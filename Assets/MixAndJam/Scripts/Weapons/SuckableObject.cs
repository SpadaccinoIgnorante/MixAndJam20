using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckableObject : MonoBehaviour
{

    public float health;

    private bool isBeingSucked = false;

    private GameObject sucker;

    private void Update()
    {
        if (isBeingSucked)
        {

            GetComponent<Rigidbody>().isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, sucker.transform.parent.position, Time.deltaTime);
            transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);

            if (transform.localScale.x <= 0.01f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = false;
            transform.localScale = Vector3.one;
            weaponsManager = null;
        }
    }

    public void SetDamage(float damage)
    {
        health -= damage;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "SuckTriggerObject")
        {
            if (health <= 0)
            {
                sucker = other.gameObject;
                isBeingSucked = true;
            }
        }
    }
}
