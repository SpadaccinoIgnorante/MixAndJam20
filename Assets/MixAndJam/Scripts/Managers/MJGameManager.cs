using System;
using UnityEngine;

public class MJGameManager : Singleton<MJGameManager>
{
    public Action OnUpdate;
    public Action OnFixedUpdate;
    
    private void Update()
    {
        OnUpdate?.Invoke();
    }

    private void FixedUpdate()
    {
        OnFixedUpdate?.Invoke();   
    }

    public override void OnDestroy()
    {
        OnFixedUpdate = null;
        OnUpdate = null;

        base.OnDestroy();
    }
}
