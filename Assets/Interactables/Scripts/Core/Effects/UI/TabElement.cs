using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class TabElement : ButtonUI
{
    
    public static Dictionary<string, List<TabElement>> tabs = new Dictionary<string, List<TabElement>>();

    public bool IsSelected { get => _isSelected; }
    public string Group
    {
        get
        {
            return this._group;
        }
        set
        {
            this._group = value;

            if (!tabs.ContainsKey(this._group))
            {
                tabs.Add(value, new List<TabElement> { this });
            }
        }
    }

    [Header("Tab element properties")]

    [SerializeField]
    private Color selectedColor;
    [SerializeField]
    private Color notSelectedColor;

    public TextMeshProUGUI textMeshPro;
    
    [SerializeField]
    private string _group;

    private bool _isSelected;

    protected override void Awake()
    {
        base.Awake();

        // If group is set from inspector
        if (!string.IsNullOrEmpty(this._group))
            AddTab(this.Group,this);
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        base.OnPointerClick(eventData);

        var elements = this.GetElementsFromGroup(this._group);

        if (elements == null)
        {
            Debug.LogWarning("No elements found!!!", this);
            return;
        }

        foreach (var element in elements)
        {
            if (element == null) continue;

            if (element == this)
                this.SelectTab();
            else
                element.DeselectTab();
        }
    }

    public static void AddTab(string group, TabElement element, bool selected = false)
    {
        element._group = group;

        if (!tabs.ContainsKey(group))
            tabs.Add(group, new List<TabElement> { element });
        else
        {
            var elements = tabs[group];

            if (elements != null && !elements.Contains(element))
                elements.Add(element);

            if (selected)
                element.SelectTab();
            else
                element.DeselectTab();
        }
    }

    public virtual void SelectTab()
    {
        if (button != null && button.targetGraphic != null)
        {
            button.targetGraphic.color = selectedColor;
            _isSelected = true;
        }
    }

    public virtual void DeselectTab()
    {
        if (button != null && button.targetGraphic != null)
        {
            button.targetGraphic.color = notSelectedColor;
            _isSelected = false;
        }
    }

    protected virtual List<TabElement> GetElementsFromGroup(string group)
    {
        if (!tabs.ContainsKey(group))
            return null;

        var element = tabs[group];

        return element;
    }
}
