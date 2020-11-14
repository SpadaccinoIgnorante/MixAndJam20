using System.Collections;
using UnityEngine;

public class SuckableObject : BehaviourBase
{
    public FloatEvent OnHealthChanged;
    public float Health 
    {
        get => _health;
        set { _health = value;  OnHealthChanged?.Invoke(_health); }
    }

    [SerializeField]
    private float _health;
    [SerializeField]
    private float _stunTime;

    private bool _isBeingSucked = false;

    private GameObject _sucker;
    private bool _hasSuckStart;
    private Vector3 _startScale;
    private Vector3 _suckStartPosition;
    private float _originalHealth;

    protected override void Awake()
    {
        base.Awake();

        _originalHealth = _health;
        _startScale = transform.localScale;
    }

    public void SetDamage(float damage)
    {
        // Already stunned do nothing
        if (_health <= 0)
            return;

        Health -= damage;

        // is stunned so start a routine
        if (_health <= 0)
            StartCoroutine(HealthRoutine());
    }
    
    private IEnumerator HealthRoutine()
    {
        yield return new WaitForSeconds(_stunTime);

        Health = _originalHealth;
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.name == "SuckTriggerObject")
        {
            if (_health <= 0)
            {
                _sucker = other.gameObject;
                if (!_hasSuckStart)
                {
                    _suckStartPosition = transform.position;
                    _hasSuckStart = true;
                }
                _isBeingSucked = true;
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        _isBeingSucked = false;
    }

    protected override void CustomUpdate()
    {
        Debug.Log("Test suck");

        if (_isBeingSucked)
        {

            //GetComponent<Rigidbody>().isKinematic = true;
            transform.position = Vector3.Lerp(transform.position, _sucker.transform.parent.position, Time.deltaTime);
            float maxDistance = Vector3.Distance(_sucker.transform.parent.position, _suckStartPosition);
            float currentDistance = Vector3.Distance(transform.position, _sucker.transform.parent.position);
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
            transform.localScale = _startScale;
            _hasSuckStart = false;
        }
    }

    protected override void CustomFixedUpdate() { }
}
