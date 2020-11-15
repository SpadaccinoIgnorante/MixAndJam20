using UnityEngine;
using System;
using UnityEngine.Events;
using DG.Tweening;

public abstract class AInteractable : MonoBehaviour
{
    public Guid ID
    {
        get { return this._id; }
    }

    public Sequence sequence;
    protected Tweener _currentTween;

    [SerializeField]
    protected bool useUnityEvents = true;
    
    [SerializeField]
    private Guid _id;

    public UnityEvent OnClick;
    public UnityEvent OnHover;
    public UnityEvent OnExit;
    public UnityEvent OnEnter;

    [SerializeField]
    private string idString;

    public abstract void OnUpdate();

    public virtual void StopSequence(bool waitCompletation, Action onKill)
    {
        if (this.sequence != null)
        {
            this.sequence.onKill += () =>
            {
                this.sequence = null;
                if (onKill != null)
                    onKill();
            };
            this.sequence.Kill(waitCompletation);
        }
    }

    public virtual Sequence RunSequence()
    {
        if (this.sequence != null)
        {
            //if (!this.sequence.IsPlaying())
            //{
            return this.sequence.Play();
            //}
        }
        return null;
    }

    public virtual void StopTweener(bool waitEnd, Action onKill)
    {
        if (this._currentTween != null)
        {
            this._currentTween.onKill += () => { if (onKill != null) onKill(); };
            this._currentTween.Kill(waitEnd);
        }
    }

    protected virtual void Start()
    {
        this._id = Guid.NewGuid();
        idString = ID.ToString();

        this.sequence = DOTween.Sequence();
        InteractableManager.Instance.AddInteractable(this);
    }

    public virtual void Stop()
    {
        DOTween.Kill(this);
    }

    // NOTE: If is required custom checks, override these methods or not invoke here the events
    protected virtual void OnMouseDown()
    {
        if (this.useUnityEvents)
            this.OnClick.Invoke();
    }

    protected virtual void OnMouseEnter()
    {
        if (this.useUnityEvents)
            this.OnEnter.Invoke();
    }

    protected virtual void OnMouseOver()
    {
        if (this.useUnityEvents)
            this.OnHover.Invoke();
    }

    protected virtual void OnMouseExit()
    {
        if (this.useUnityEvents)
            this.OnExit.Invoke();
    }

    protected virtual void OnDisable()
    {
        if (InteractableManager.IsInstanced())
            InteractableManager.Instance.RemoveInteractable(ID);
    }

    protected virtual void OnEnable()
    {
        if (InteractableManager.IsInstanced())
            InteractableManager.Instance.AddInteractable(this);
    }

    protected virtual void OnDestroy()
    {
        if (InteractableManager.IsInstanced())
            InteractableManager.Instance.RemoveInteractable(ID);
    }
}
