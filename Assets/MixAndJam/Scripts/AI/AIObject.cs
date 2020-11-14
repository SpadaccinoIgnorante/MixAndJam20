using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;

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

    [Space(10)]
    [Header("Debug")]
    [SerializeField]
    private List<EscapePoint> _escapePoints = new List<EscapePoint>();
    [SerializeField]
    private bool _isAlerted;
    [SerializeField]
    private EscapePoint _currentPoint;

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

    protected override void CustomFixedUpdate()
    {
        //if (!IsRunCompleted()) return;

        var layer = _pMask.value;

        // Area di allerta per il giocatore
        var triggerColls = Physics.OverlapSphere(transform.position, _alertRadius, layer);

        PerformAlertRadius(triggerColls);
    }

    protected override void CustomUpdate()
    {
        if (IsRunCompleted())
            _currentPoint = null;

        if (IsAlerted)
        {
            if (_currentPoint != null) return;

            Vector3 dir = GetPlayerDirection();

            var angle = Vector3.Angle(_playerTrigger.transform.position, dir);

            _currentPoint = GetRandomPoint(GetPointsByDistance(_escapePointRadius));
            _agent.SetDestination(_currentPoint.transform.position);
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

    protected virtual EscapePoint GetRandomPoint(List<EscapePoint> points)
    {
        List<EscapePoint> possiblePoints = new List<EscapePoint>();

        foreach (var p in points)
        {
            //var angle = Vector3.Angle(_playerTrigger.transform.position, dir);

            var pointDistance = Vector3.Distance(transform.position, p.transform.position);

            Debug.Log(p.name + " " + pointDistance);

            if (pointDistance < 3.25f) continue;

            possiblePoints.Add(p);
        }

        return possiblePoints[UnityEngine.Random.Range(0, possiblePoints.Count)];
    }

    protected virtual Vector3 GetPlayerDirection()
    {
        var diff = (_playerTrigger.transform.position - _prevLocation).normalized;
        _prevLocation = transform.position;
        return -diff;
    }

#if UNITY_EDITOR
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, _escapePointRadius);
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(transform.position, _alertRadius);
    }
#endif

}
