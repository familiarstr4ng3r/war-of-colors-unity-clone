using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteElementButton : MonoBehaviour
{
    public System.Action OnClick;

    private Image image = null;

    private void Awake()
    {
        image = GetComponent<Image>();
    }

    private void Start()
    {
        var button = GetComponent<Button>();
        button.onClick.AddListener(() => OnClick());
    }

    public void UpdateVisual(bool selected, Color color)
    {
        image.color = color;
    }
}
