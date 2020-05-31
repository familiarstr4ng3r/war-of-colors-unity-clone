using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridSettingsWindow : MonoBehaviour
{
    [SerializeField] private InputField width = null;
    [SerializeField] private InputField height = null;
    [SerializeField] private Text label = null;

    private Vector2Int size = Vector2Int.zero;
    public Vector2Int GridSize => size;

    private void Awake()
    {
        width.onValueChanged.AddListener((text) => OnChange(text, true));
        height.onValueChanged.AddListener((text) => OnChange(text, false));
    }

    private void OnChange(string text, bool isWidth)
    {
        if (!int.TryParse(text, out int value)) return;

        if (isWidth) size.x = value;
        else size.y = value;

        label.text = size.ToString();
    }
}
