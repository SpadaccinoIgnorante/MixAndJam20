using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuckableObject : MonoBehaviour
{

    public float health;

    private bool isBeingSucked = false;

    private GameObject sucker;
    private bool hasSuckStart;
    Vector3 startScale;
    Vector3 suckStartPosition;

    private void Awake()
    {
        startScale = transform.localScale;
    }

    private void Update()
    {
        if (isBeingSucked)
        {

            //GetComponent<Rigidbody>().isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, sucker.transform.parent.position, Time.deltaTime);
            float maxDistance = Vector3.Distance(sucker.transform.parent.position, suckStartPosition);
            float currentDistance = Vector3.Distance(transform.position, sucker.transform.parent.position);
            float scaleFactor = currentDistance / maxDistance;
            Vector3 scaleVector = new Vector3(scaleFactor, scaleFactor, scaleFactor);
            transform.localScale = scaleVector;

            //transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, Time.deltaTime);

            if (transform.localScale.x <= 0.05f)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            //GetComponent<Rigidbody>().isKinematic = false;
            transform.localScale = startScale;
            hasSuckStart = false;
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
                if (!hasSuckStart)
                {
                    suckStartPosition = transform.position;
                    hasSuckStart = true;
                }
                isBeingSucked = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        isBeingSucked = false;
    }
}
