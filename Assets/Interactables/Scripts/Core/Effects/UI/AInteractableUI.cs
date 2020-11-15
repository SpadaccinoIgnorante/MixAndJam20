using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public abstract class AInteractableUI : AInteractable,IPointerUpHandler, IPointerClickHandler, IPointerDownHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField]
    protected float duration;

    protected override void Start()
    {
        base.Start();
        // force to false for Interactable UI elements,
        // because already implements interfaces which works better for UI  
        this.useUnityEvents = false;
    }

    protected virtual void Awake() { }

    public abstract void OnPointerClick(PointerEventData eventData);

    public abstract void OnPointerDown(PointerEventData eventData);

    public abstract void OnPointerEnter(PointerEventData eventData);

    public abstract void OnPointerExit(PointerEventData eventData);

    public abstract void OnPointerUp(PointerEventData eventData);

}
