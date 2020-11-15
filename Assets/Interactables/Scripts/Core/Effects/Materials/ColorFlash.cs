using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;

public class ColorFlash : AInteractable
{
    public Color _startValue;
    [HideInInspector]
    public Color _endValue;
    [HideInInspector]
    public float _frequency;
    [HideInInspector]
    public Material materialToFlash;
    [HideInInspector]
    public string _colorShaderProp;

    public override void OnUpdate()
    {

    }

    protected override void Start()
    {
        base.Start();
#if INTERACTABLES_EXAMPLE
        this.CreateSequence(this._startValue,this._endValue,this._frequency,null);
#endif
    }

    public void CreateSequence(Color startValue, Color endValue, float frequency, Action onKill)
    {
        // Kill current tween before create new one
        this.StopTweener(false, onKill);
        // Set value
        this._endValue = endValue;
        this._frequency = frequency;
        this.materialToFlash.SetColor(_colorShaderProp, startValue);

        _currentTween = this.materialToFlash.DOColor(endValue, _colorShaderProp, frequency).SetLoops(-1, LoopType.Yoyo);
    }


}
