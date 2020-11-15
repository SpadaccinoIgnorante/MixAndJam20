using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;

public class ButtonUIRotateEffect : ButtonUI
{
    [SerializeField]
    protected Vector3 rotationEnd;

    protected Vector3 originalRotation;

    protected override void Awake()
    {
        base.Awake();

#if INTERACTABLES_EXAMPLE
        this.gameObject.name = this.animationEffect.ToString();
#endif
        this.originalRotation = this.transform.rotation.eulerAngles;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (this.duration != -1)
            this.transform.DORotate(this.rotationEnd, this.duration).SetEase(this.animationEffect);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (this.duration != -1)
            this.transform.DOScale(this.originalRotation, this.duration).SetEase(this.animationEffect);
    }
}
