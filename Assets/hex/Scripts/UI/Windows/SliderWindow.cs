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
        [SerializeField] private Button transferButton = null;
        [SerializeField] private Button closeButton = null;

        private int selectedAmount = 0;
        private bool isFirstStage = true;
        private bool isTransfering = false;

        private string sliderText = string.Empty;

        private void Start()
        {
            var manager = FindObjectOfType<MovesManager>();

            addButton.onClick.AddListener(() => manager.OnClickAdd(selectedAmount, isTransfering));
            transferButton.onClick.AddListener(() => manager.OnClickTransfer(selectedAmount));

            closeButton.onClick.AddListener(Deactivate);
            slider.onValueChanged.AddListener((value) => OnValueChange());

            Deactivate();
        }

        //используется для передачи войск на стадии пополнения
        public void Activate(int maxValue, string text, bool transfering)
        {
            isTransfering = transfering;

            slider.wholeNumbers = true;
            slider.maxValue = maxValue;
            slider.value = slider.maxValue;

            transferButton.gameObject.SetActive(true);

            go.SetActive(true);

            //label.text = text.Replace("*text*", sliderText);
            UpdateLabel();

            MovesManager.IsClickBlocked = true;
        }

        //используется для выбора количества при атаке
        public void Activate(int maxValue, bool firstStage)
        {
            isFirstStage = firstStage;

            isTransfering = false;

            transferButton.gameObject.SetActive(false);

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
            //string text = isFirstStage ? "Выбрано для атаки" : "Выбрано для пополнения";
            //label.text = $"{text}: {slider.value}/{slider.maxValue}";

            sliderText = $"{slider.value}/{slider.maxValue}";
            label.text = sliderText;

            string t = isTransfering ? "Пополнить" : (isFirstStage ? "Атаковать" : "Пополнить");
            addButton.GetComponentInChildren<Text>().text = t;
        }
    }
}
