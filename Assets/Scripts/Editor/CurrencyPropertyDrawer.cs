using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomPropertyDrawer(typeof(Currency))]
public class GoldPropertyDrawer : PropertyDrawer
{
    private const int height = 18;
    private const int space = 2;

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        int count = 4;
        float h = property.isExpanded ? count * height + space * 2 : height;
        return h;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        position.height = height;

        EditorGUI.BeginProperty(position, label, property);
        //position = EditorGUI.PrefixLabel(position, label);

        property.isExpanded = EditorGUI.Foldout(position, property.isExpanded, label);

        //Box(position, Color.red);

        position.x += height / 2;
        position.width -= height / 2;

        if (property.isExpanded)
        {
            var valueProperty = property.FindPropertyRelative("value");
            position.y += height + space;
            EditorGUI.PropertyField(position, valueProperty);

            //Box(position, Color.green);

            //
            string[] VALUES = Currency.GenerateArray();

            int[] optionValues = new int[VALUES.Length];
            for (int i = 0; i < optionValues.Length; i++) optionValues[i] = i;

            GUIContent[] displayedOptions = new GUIContent[VALUES.Length];
            for (int i = 0; i < displayedOptions.Length; i++) displayedOptions[i] = new GUIContent(VALUES[i]);

            var indexProperty = property.FindPropertyRelative("index");
            position.y += height + space;
            EditorGUI.IntPopup(position, indexProperty, displayedOptions, optionValues);
            //

            int index = indexProperty.intValue;
            string format = index > 0 ? $"{VALUES[index]}" : string.Empty;
            string text = $"{valueProperty.floatValue} {format}";

            position.y += height + space;
            GUI.enabled = false;
            EditorGUI.TextField(position, "Format", text);
            GUI.enabled = true;
        }

        EditorGUI.EndProperty();
    }

    private void Box(Rect rect, Color color)
    {
        var old = GUI.color;
        color.a = 0.2f;
        GUI.color = color;
        GUI.Box(rect, string.Empty);
        GUI.color = old;
    }
}
