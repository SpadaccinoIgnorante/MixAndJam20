using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Events;
public class DraggableUI : AInteractableUI
{
    public ObjectMoveEvent OnMove;

    [SerializeField]
    protected bool isDragging;
    [SerializeField]
    protected bool isDraggable = true;

    protected RectTransform rectTransform;

    protected Vector2 offset;

    protected override void Start()
    {
        base.Start();

        this.rectTransform = GetComponent<RectTransform>();
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (!isDraggable) return;

        this.isDragging = true;

        var inputPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
        offset = new Vector2(this.transform.localPosition.x - inputPos.x, this.transform.localPosition.y - inputPos.y);

        EventSystem.current.SetSelectedGameObject(this.gameObject);
    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        
    }

    public override void OnPointerUp(PointerEventData eventData)
    {
        if (!isDraggable) return;

        this.isDragging = false;

        offset = Vector2.zero;

        EventSystem.current.SetSelectedGameObject(null);
    }

    public override void OnUpdate()
    {
        if (this.isDragging)
        {
            var inputPos = new Vector2(Input.mousePosition.x, Input.mousePosition.y);
            
            this.transform.localPosition = new Vector2(offset.x + inputPos.x,offset.y + inputPos.y);

            if (this.OnMove != null)
                this.OnMove.Invoke(this.transform.localPosition);
        }
    }
}
