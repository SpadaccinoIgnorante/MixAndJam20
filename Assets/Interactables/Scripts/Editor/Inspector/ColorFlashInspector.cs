using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(ColorFlash))]
public class ColorFlashInspector : Editor
{
    ColorFlash _colorFlash;

    public override void OnInspectorGUI()
    {
        GUILayout.BeginVertical(GUI.skin.box);
        GUILayout.Space(10);
        base.OnInspectorGUI();
        GUILayout.EndVertical();
        this._colorFlash = (ColorFlash)this.target;

        GUILayout.Space(20);
        if (this._colorFlash != null)
        {
            GUILayout.BeginVertical(GUI.skin.box);
            GUILayout.Space(10);
            this._colorFlash._endValue = EditorGUILayout.ColorField(new GUIContent("End Value",
                                                                                    "Material's color will be lerped with the original color and this"),
                                                                                    this._colorFlash._endValue);
            GUILayout.Space(10);
            this._colorFlash._colorShaderProp = EditorGUILayout.TextField("Color Shader Property", this._colorFlash._colorShaderProp);
            GUILayout.Space(10);
            this._colorFlash._frequency = EditorGUILayout.FloatField("Frequency", this._colorFlash._frequency);
            GUILayout.Space(10);
            this._colorFlash.materialToFlash = (Material)EditorGUILayout.ObjectField("Target material", this._colorFlash.materialToFlash, typeof(Material), false);

            GUILayout.Space(10);

            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Stop Sequence"))
            {
                this._colorFlash.StopSequence(false, null);
            }
            if (GUILayout.Button("Stop Tweener"))
            {
                this._colorFlash.StopTweener(false, null);
            }
            GUILayout.EndHorizontal();
            GUILayout.EndVertical();
        }

        if (GUI.changed)
        {
            EditorUtility.SetDirty(_colorFlash);
            //EditorSceneManager.MarkSceneDirty(castedTarget.gameObject.scene);
        }

       
    }
}
