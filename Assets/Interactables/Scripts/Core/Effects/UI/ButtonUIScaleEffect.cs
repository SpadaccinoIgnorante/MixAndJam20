using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonUIScaleEffect : ButtonUI
{
    [SerializeField]
    protected float scaleEnd;

    protected float originalScale;

    protected override void Awake()
    {
        base.Awake();

#if INTERACTABLES_EXAMPLE
        this.gameObject.name = this.animationEffect.ToString();
#endif
        this.originalScale = this.transform.localScale.x;
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        base.OnPointerEnter(eventData);

        if (this.duration != -1)
            this.transform.DOScale(this.scaleEnd, this.duration).SetEase(this.animationEffect);
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        base.OnPointerExit(eventData);

        if (this.duration != -1)
            this.transform.DOScale(this.originalScale, this.duration).SetEase(this.animationEffect);
    }
}
