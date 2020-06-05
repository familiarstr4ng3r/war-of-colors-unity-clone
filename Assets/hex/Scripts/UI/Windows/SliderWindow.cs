using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class SliderWindow : MonoBehaviour
    {
        [SerializeField] private GameObject go = null;
        [SerializeField] private Slider slider = null;
        [SerializeField] private Text label = null;
        [SerializeField] private Button addButton = null;
        [SerializeField] private Button closeButton = null;

        private Text buttonLabel = null;

        private int selectedAmount = 0;
        private bool isFirstStage = true;

        private void Start()
        {
            var manager = FindObjectOfType<MovesManager>();

            addButton.onClick.AddListener(() => manager.OnAddCLick(selectedAmount));
            closeButton.onClick.AddListener(Deactivate);
            slider.onValueChanged.AddListener((value) => OnValueChange());

            buttonLabel = addButton.GetComponentInChildren<Text>();

            Deactivate();
        }

        public void Activate(int maxValue, bool firstStage)
        {
            isFirstStage = firstStage;

            slider.wholeNumbers = true;
            slider.maxValue = maxValue;
            slider.value = slider.maxValue;

            UpdateLabel();

            go.SetActive(true);

            MovesManager.IsClickBlocked = true;
        }

        public void Deactivate()
        {
            go.SetActive(false);

            this.CallWithDelay(() => MovesManager.IsClickBlocked = false, 0.2f);// looks like КоСтЫлЬ
        }

        private void OnValueChange()
        {
            UpdateLabel();
            selectedAmount = Mathf.RoundToInt(slider.value);
        }

        private void UpdateLabel()
        {
            string text = isFirstStage ? "Выбрано для атаки" : "Выбрано для пополнения";
            label.text = $"{text}: {slider.value}/{slider.maxValue}";

            buttonLabel.text = isFirstStage ? "Атаковать" : "Пополнить";
        }
    }
}
