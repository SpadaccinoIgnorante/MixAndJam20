using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IconAlert : BehaviourBase
{
    [SerializeField]
    private float _viewSeconds = 0;
    [SerializeField]
    private Camera _mainCamera;

    private SpriteRenderer _sprite;
    private AITrigger _aITrigger;

    private void Start()
    {
        if (_mainCamera == null)
            _mainCamera = FindObjectOfType<Camera>();

        _aITrigger = FindObjectOfType<AITrigger>();

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

    protected override void CustomUpdate()
    {
        if (_sprite.enabled)
        {
            transform.LookAt(_aITrigger.transform.position,Vector3.up);
        }
    }

    protected override void CustomFixedUpdate()
    {
        
    }
}
