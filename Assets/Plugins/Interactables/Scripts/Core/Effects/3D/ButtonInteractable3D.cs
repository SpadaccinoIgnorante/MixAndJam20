using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class ButtonInteractable3D : AInteractable
{
    public override void OnUpdate() { }

    public virtual void Click()
    {
        OnClick?.Invoke();
    }
}
