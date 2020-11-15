using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class MinimizerButton : ButtonUIScaleEffect
{
    public TextMeshProUGUI textMesh;

    public Image icon;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InitializeButton(string title,Sprite icon)
    {
        if (this.icon != null)
            this.icon.sprite = icon;

        if (this.textMesh != null)
            this.textMesh.text = title;
    }
}
