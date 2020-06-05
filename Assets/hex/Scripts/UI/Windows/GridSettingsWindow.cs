using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class GridSettingsWindow : MonoBehaviour
    {
        [SerializeField] private InputField widthField = null;
        [SerializeField] private InputField heightField = null;
        [SerializeField] private Text label = null;

        private GridCreator grid = null;

        private void Awake()
        {
            grid = FindObjectOfType<GridCreator>();

            widthField.onValueChanged.AddListener((text) => OnChange(text, true));
            heightField.onValueChanged.AddListener((text) => OnChange(text, false));

            UpdateLabel();
        }

        private void OnChange(string text, bool isWidth)
        {
            int.TryParse(text, out int value);

            if (isWidth) grid.Width = value;
            else grid.Height = value;

            UpdateLabel();
        }

        private void UpdateLabel()
        {
            label.text = $"Ширина: {grid.Width} / Высота: {grid.Height}";
        }
    }
}
