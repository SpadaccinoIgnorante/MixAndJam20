using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAlert : MonoBehaviour
{
    [SerializeField]
    private float _viewSeconds;
    
    private SpriteRenderer _sprite;

    private void Start()
    {
        _sprite = GetComponent<SpriteRenderer>();
    }

    public void Alerted(bool alerted)
    {
        if (!alerted) return;

        StartCoroutine(Routine(_viewSeconds));
    }

    private IEnumerator Routine(float seconds)
    {
        _sprite.enabled = true;

        yield return new WaitForSeconds(seconds);

        _sprite.enabled = false;
    }
}
