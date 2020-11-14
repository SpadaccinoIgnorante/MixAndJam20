using System;
using UnityEngine;

public class MJGameManager : Singleton<MJGameManager>
{
    public bool IsPaused { get; private set; }

    public Action OnPauseUpdate;
    public Action OnUpdate;
    public Action OnFixedUpdate;
    
    private void Update()
    {
        // if premo start IsPaused = !IsPaused;

        if (!IsPaused)
            OnUpdate?.Invoke();
        else
            OnPauseUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        if(!IsPaused)
            OnFixedUpdate?.Invoke();   
    }

    public override void OnDestroy()
    {
        OnFixedUpdate = null;
        OnUpdate = null;

        base.OnDestroy();
    }

    public void PauseUnpause()
    {
        IsPaused = !IsPaused;
    }
}
