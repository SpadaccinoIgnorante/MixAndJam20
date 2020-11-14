using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BehaviourBase : MonoBehaviour
{
    protected virtual void Awake()
    {
        MJGameManager.Instance.OnUpdate += CustomUpdate;
        MJGameManager.Instance.OnFixedUpdate += CustomFixedUpdate;
        MJGameManager.Instance.OnPauseChanged += PauseChanged;
    }

    protected virtual void OnDestroy()
    {
        if (!MJGameManager.IsInstanced())
            return;

        MJGameManager.Instance.OnPauseChanged -= PauseChanged;
        MJGameManager.Instance.OnUpdate -= CustomUpdate;
        MJGameManager.Instance.OnFixedUpdate -= CustomFixedUpdate;
    }

    protected virtual void PauseChanged(bool pause) { }
    protected abstract void CustomUpdate();
    protected abstract void CustomFixedUpdate();
}
