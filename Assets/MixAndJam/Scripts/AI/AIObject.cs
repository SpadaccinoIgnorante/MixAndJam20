using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.AI;
using DG.Tweening;

// TODO : Controllare che l'escape point successivo non sia stato già preso

public enum AIState
{
    Idle,
    Alerted,
    Walk,
    IsStunned
}

public class AIObject : BehaviourBase
{
    public BoolEvent OnAlertChanged;
    public AIStateEvent OnStateChanged;

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
    [SerializeField]
    private Animator aiAnimation;
    [SerializeField]
    [Range(1, 100)]
    private float _idlePercentage = 25;


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
    [SerializeField]
    private bool _isThinking;
    private Vector3 _prevLocation;

    private float _originalAcceleration;

    protected override void Awake()
    {
        base.Awake();

        if (_agent)
            _agent = GetComponent<NavMeshAgent>();
        if (_playerTrigger == null)
            _playerTrigger = FindObjectOfType<AITrigger>();

        _originalAcceleration = _agent.acceleration;

        foreach (var point in FindObjectsOfType<EscapePoint>())
            _escapePoints.Add(point);
    }

    public virtual void OnStun(float value)
    {
        _isStunned = value <= 0;

        if (_isStunned)
        {
            OnStateChanged?.Invoke(AIState.IsStunned);

            aiAnimation.SetBool("isStunned", true);
            aiAnimation.SetBool("isRunning", false);
            aiAnimation.SetBool("isRunning", false);
            aiAnimation.SetBool("isIdle", false);
            _agent.isStopped = true;
            //_agent.enabled = false;
        }
        else if (_agent.isStopped && !_isStunned)
        {
            //_agent.enabled = true;
            _agent.isStopped = false;
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
        if (_isThinking) return;
        if (_isStunned) return;

        if (IsRunCompleted() && _currentPoint != null)
        {
            _agent.isStopped = true;
            _agent.velocity = Vector3.zero;
            _agent.acceleration = 0;

            aiAnimation.SetBool("isRunning", false);
            aiAnimation.SetBool("isRunning", false);
            aiAnimation.SetBool("isIdle", false);
            aiAnimation.SetBool("isStunned", false);

            _previousPoint = _currentPoint;
            _currentPoint = null;
        }

        Vector3 dir = GetPlayerDirection();

        if (IsAlerted && _currentPoint == null)
        {
            aiAnimation.SetBool("isRunning", true);
            aiAnimation.SetBool("isIdle", false);
            aiAnimation.SetBool("isWalking", false);

            OnStateChanged?.Invoke(AIState.Alerted);

            SetPoint(GetRandomPoint(GetPointsByDistance(_escapePointRadius), dir));
        }
        else if (!IsAlerted && _currentPoint == null)
        {
            // Idle
            if (Random.Range(0, 100) <= _idlePercentage)
            {
                aiAnimation.SetBool("isIdle",true);
                aiAnimation.SetBool("isRunning", false);
                aiAnimation.SetBool("isWalking", false);
                aiAnimation.SetBool("isStunned", false);

                OnStateChanged?.Invoke(AIState.Idle);

                StartCoroutine(PerformIdle());
            }
            else
            {
                // Run
                aiAnimation.SetBool("isWalking", true); 
                aiAnimation.SetBool("isRunning", false);
                aiAnimation.SetBool("isIdle", false);
                aiAnimation.SetBool("isStunned", false);

                OnStateChanged?.Invoke(AIState.Walk);

                var list = GetPointsByDistance(_escapePointRadius);

                SetPoint(list[Random.Range(0, list.Count)]);
            }
        }

        aiAnimation.speed = _agent.speed / 2;
    }

    private IEnumerator PerformIdle()
    {
        _isThinking = true;

        yield return new WaitForSeconds(5);
        
        _isThinking = false;
    }

    private void SetPoint(EscapePoint point)
    {
        _agent.isStopped = false;
        _agent.acceleration = _originalAcceleration;

        _currentPoint = point;

        transform.DOLookAt(_currentPoint.transform.position, _rotationSpeed);
        _agent.SetDestination(_currentPoint.transform.position);
    }

    protected virtual bool IsRunCompleted()
    {
        if (_isStunned) return false; 

        return _agent.remainingDistance <= _agent.stoppingDistance;
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

        if (!IsAlerted)
        {
            _isThinking = false;
            IsAlerted = true;
        }
    }

    protected virtual EscapePoint GetRandomPoint(List<EscapePoint> points,Vector3 playerDir)
    {
        List<EscapePoint> possiblePoints = new List<EscapePoint>();

        foreach (var p in points)
        {
            if (_previousPoint == p) continue;

            var pointDir = (p.transform.position - transform.position).normalized;

            if (Vector3.Dot(playerDir, pointDir) > _thresholdDirection)
            {
                Debug.DrawRay(transform.position, pointDir, Color.yellow);
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

    protected override void PauseChanged(bool pause)
    {
        _agent.isStopped = pause;
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
