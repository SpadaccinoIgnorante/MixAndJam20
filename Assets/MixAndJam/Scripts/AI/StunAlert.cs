using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StunAlert : MonoBehaviour
{
    [SerializeField]
    private GameObject _stunParticle;

    public void OnStun(float value)
    {
        if (_stunParticle.activeInHierarchy && value > 0)
            _stunParticle.SetActive(false);

        if (value <= 0)
        {
            _stunParticle.SetActive(true);
        }
    }
}
