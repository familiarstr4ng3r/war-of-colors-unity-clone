using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderWindow : MonoBehaviour
{
    [SerializeField] private GameObject go = null;
    [SerializeField] private Slider slider = null;
    [SerializeField] private Text label = null;
    [SerializeField] private Button addButton = null;
    [SerializeField] private Button closeButton = null;

    private int selectedAmount = 0;

    private void Start()
    {
        var manager = FindObjectOfType<MovesManager>();

        addButton.onClick.AddListener(() => manager.OnAddCLick(selectedAmount));
        closeButton.onClick.AddListener(Deactivate);
        slider.onValueChanged.AddListener((value) => OnValueChange());

        Deactivate();
    }

    public void Activate(int maxValue)
    {
        slider.wholeNumbers = true;
        slider.maxValue = maxValue;
        slider.value = slider.maxValue;

        UpdateLabel();

        go.SetActive(true);
    }

    public void Deactivate()
    {
        go.SetActive(false);

        MovesManager.IsClickBlocked = false;
    }

    private void OnValueChange()
    {
        UpdateLabel();
        selectedAmount = Mathf.RoundToInt(slider.value);
    }

    private void UpdateLabel()
    {
        label.text = $"{slider.value}/{slider.maxValue}";
    }
}
