using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class HierarchQuickSetActive
{
    private static int size = 16;

    static HierarchQuickSetActive()
    {
        EditorApplication.hierarchyWindowItemOnGUI += HierarchyWindowItem;
    }

    static void HierarchyWindowItem(int instanceID, Rect selectionRect)
    {
        Object o = EditorUtility.InstanceIDToObject(instanceID);
        if (o is GameObject)
        {
            GameObject gameObject = o as GameObject;

            float x = selectionRect.x - size * 1.75f;
            Rect rect = new Rect(x, selectionRect.y, size, size);

            //DrawRect(selectionRect, Color.yellow);
            //DrawRect(rect, Color.red);

            EditorGUI.BeginChangeCheck();

            bool value = GUI.Toggle(rect, gameObject.activeSelf, string.Empty);

            if (EditorGUI.EndChangeCheck())
            {
                Undo.RecordObject(gameObject, $"Active state - {gameObject.name}");
                gameObject.SetActive(value);
            }
        }
    }

    static void DrawRect(Rect rect, Color color, float alpha = 0.2f, string text = "")
    {
        color.a = alpha;
        GUI.color = color;
        GUI.Box(rect, text);
    }
}
