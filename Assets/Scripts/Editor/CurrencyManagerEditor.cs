using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(CurrencyManager))]
public class CurrencyManagerEditor : Editor
{
    private CurrencyManager cm = null;

    private void OnEnable()
    {
        cm = target as CurrencyManager;
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        //if (GUILayout.Button("Add")) cm.Add();
        //else if (GUILayout.Button("Buy")) cm.Buy();

        if (GUILayout.Button("Convert"))
        {
            cm.Convert();
        }
    }
}
