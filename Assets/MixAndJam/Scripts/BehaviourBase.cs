using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        MJGameManager.Instance.OnUpdate += CustomUpdate;
        MJGameManager.Instance.OnFixedUpdate += CustomFixedUpdate;
    }

    protected virtual void OnDestroy()
    {
        MJGameManager.Instance.OnUpdate -= CustomUpdate;
        MJGameManager.Instance.OnFixedUpdate -= CustomFixedUpdate;
    }

    protected abstract void CustomUpdate();
    protected abstract void CustomFixedUpdate();
}
