using System.Collections;
using UnityEngine;

public class SuckableObject : BehaviourBase
{
    public FloatEvent OnHealthChanged;
    public BoolEvent OnStun;

    public float Health 
    {
        get => _health;
        set { _health = value;  OnHealthChanged?.Invoke(_health); }
    }
    public bool IsStunned { get; set; }
    public float StunTime => _stunTime;
    public bool IsCatching { get; set; }
    [SerializeField]
    private float _health;
    [SerializeField]
    private float _stunTime;

    private float _originalHealth;

    protected override void Awake()
    {
        base.Awake();

        _originalHealth = _health;
    }

    public void SetDamage(float damage)
    {
        // Already stunned do nothing
        if (_health <= 0)
            return;

        Health -= damage;

        // is stunned so start a routine
        if (_health <= 0)
        {
            IsStunned = true;

            OnStun?.Invoke(IsStunned);

            StartCoroutine(HealthRoutine());
        }
    }
    
    private IEnumerator HealthRoutine()
    {
        while (IsCatching)
        {
            yield return null;
        }

        yield return new WaitForSeconds(_stunTime);

        Health = _originalHealth;

        IsStunned = false;

        OnStun?.Invoke(IsStunned);
    }

    protected override void CustomUpdate() { }

    protected override void CustomFixedUpdate() { }
}
