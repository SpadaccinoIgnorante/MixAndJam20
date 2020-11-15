using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using DG.Tweening;

[RequireComponent(typeof(Button))]
[RequireComponent(typeof(Shadow))]
public class ButtonUI : AInteractableUI
{
    public Button Button { get { return this.button; } }

    public Image Icon 
    { 
        get 
        {
            var icon = GetComponent<Image>();

            if (icon != null) return icon;

            icon = GetComponentInChildren<Image>();

            return icon;
        }
    }

    [SerializeField]
    protected Shadow shadow;
    protected Button button;

    [SerializeField]
    protected Ease animationEffect = Ease.Flash;
    [SerializeField]
    protected bool shadowAlwaysVisible = false;

    protected override void Awake()
    {
        if (this.button == null)
            this.button = this.gameObject.GetComponent<Button>();
        if (this.shadow == null)
            this.shadow = this.gameObject.GetComponent<Shadow>();

        if (this.shadow != null && !this.shadowAlwaysVisible)
            this.shadow.enabled = false;
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (this.OnClick != null)
        {
            this.OnClick.Invoke();
        }        
    }

    public override void OnPointerDown(PointerEventData eventData){}

    public override void OnPointerEnter(PointerEventData eventData)
    {
        // Enable shadow
        if (this.shadow != null && !this.shadowAlwaysVisible)
            this.shadow.enabled = true;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        // Disable shadow
        if (this.shadow != null && !this.shadowAlwaysVisible)
            this.shadow.enabled = false;
    }

    public override void OnUpdate()
    {

    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        
    }
}
