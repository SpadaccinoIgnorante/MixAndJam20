﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using DG.Tweening;

// TODO : Controllare che l'escape point successivo non sia stato già preso

public class AIObject : BehaviourBase
{
    public BoolEvent OnAlertChanged;

    public bool IsAlerted
    {
        get { return _isAlerted; }
        set { _isAlerted = value; OnAlertChanged?.Invoke(_isAlerted); }
    }

    [Header("AI Settings")]
    [SerializeField]
    private NavMeshAgent _agent;
    [SerializeField]
    private AITrigger _playerTrigger;
    [SerializeField]
    private float _alertRadius = 0;
    [SerializeField]
    private float _escapePointRadius = 0;
    [SerializeField]
    private LayerMask _pMask;
    [SerializeField]
    private float _rotationSpeed = 0;
    [SerializeField]
    private float _thresholdDirection;

    [Space(10)]
    [Header("Debug")]
    [SerializeField]
    private List<EscapePoint> _escapePoints = new List<EscapePoint>();
    [SerializeField]
    private bool _isAlerted;
    [SerializeField]
    private EscapePoint _currentPoint;
    [SerializeField]
    private EscapePoint _previousPoint;
    [SerializeField]
    private bool _drawGizmos;
    [SerializeField]
    private bool _isStunned;

    private Vector3 _prevLocation;

    protected override void Awake()
    {
        if(_agent)
            _agent = GetComponent<NavMeshAgent>();
        if (_playerTrigger == null)
            _playerTrigger = FindObjectOfType<AITrigger>();

        base.Awake();

        foreach (var point in FindObjectsOfType<EscapePoint>())
            _escapePoints.Add(point);
    }

    public virtual void OnStun(float value)
    {
        _isStunned = value <= 0;

        if (_isStunned)
        {
            _agent.isStopped = true;
            _agent.enabled = false;
        }
        else if (_agent.isStopped && !_isStunned)
        {
            _agent.isStopped = false;
            _agent.enabled = true;
        }
    }

    protected override void CustomFixedUpdate()
    {
        if (_isStunned) return;

        var layer = _pMask.value;

        // Area di allerta per il giocatore
        var triggerColls = Physics.OverlapSphere(transform.position, _alertRadius, layer);

        PerformAlertRadius(triggerColls);
    }

    protected override void CustomUpdate()
    {
        if (_isStunned) return;

        if (IsRunCompleted() && _currentPoint != null)
        {
            _previousPoint = _currentPoint;
            _currentPoint = null;
        }

        Vector3 dir = GetPlayerDirection();

        if (IsAlerted && _currentPoint == null)
        {
            _currentPoint = GetRandomPoint(GetPointsByDistance(_escapePointRadius), dir);

            _agent.SetDestination(_currentPoint.transform.position);

            Debug.DrawRay(_playerTrigger.transform.position, -dir * 5, Color.green);

            Debug.Log($"Punto scelto {_currentPoint.name} ");
            var pointDir = (_currentPoint.transform.position - transform.position).normalized;

            Debug.DrawRay(transform.position, pointDir, Color.red);
            Debug.Break();
        }
    }

    protected virtual bool IsRunCompleted()
    {
        return _agent.remainingDistance <= _agent.stoppingDistance + 0.1f;
    }

    protected List<EscapePoint> GetPointsByDistance(float distance)
    {
        var points = new List<EscapePoint>();

        foreach (var point in _escapePoints)
        {
            if (point == null) continue;

            if (point == _currentPoint || Vector3.Distance(transform.position, point.transform.position) > distance)
                continue;

            points.Add(point);
        }

        return points;
    }

    protected virtual void PerformAlertRadius(Collider[] colliders)
    {
        var alert = colliders.ToList().Find(c => LayerUtils.IsInLayerMask(c.gameObject.layer,_pMask));

        if (alert == null)
        {
            if(IsAlerted)
                IsAlerted = false;
            return;
        }

        if(!IsAlerted)
            IsAlerted = true;
    }

    protected virtual EscapePoint GetRandomPoint(List<EscapePoint> points,Vector3 playerDir)
    {
        List<EscapePoint> possiblePoints = new List<EscapePoint>();

        foreach (var p in points)
        {
            if (_previousPoint == p) continue;

            var pointDir = (p.transform.position - transform.position).normalized;

            Debug.Log(p.name + " " + Vector3.Dot(playerDir, pointDir),p.gameObject);

            if (Vector3.Dot(playerDir, pointDir) > _thresholdDirection)
            {
                Debug.DrawRay(transform.position, pointDir, Color.yellow);
                Debug.Log("La direzione non è opposta",p.gameObject);
                continue;
            }

            Debug.DrawRay(transform.position, pointDir, Color.cyan);

            possiblePoints.Add(p);
        }

        if (possiblePoints.Count <= 0)
        {
            var tempPoints = points.FindAll(p => p != _previousPoint);
            return tempPoints.Count <= 0 ? 
                   _escapePoints.FindAll(p => p != _previousPoint && p != _currentPoint)[0] : 
                   tempPoints[Random.Range(0, tempPoints.Count)];
        }

        return possiblePoints[Random.Range(0, possiblePoints.Count)];
    }

    protected virtual Vector3 GetPlayerDirection()
    {
        var diff = (_playerTrigger.transform.position - _prevLocation).normalized;
        _prevLocation = transform.position;
        return diff;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        if(!_drawGizmos)return;

        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _escapePointRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _alertRadius);
    }
#endif

}
