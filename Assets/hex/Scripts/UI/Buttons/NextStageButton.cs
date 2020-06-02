using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NextStageButton : MonoBehaviour
{
    [SerializeField] private Text label = null;

    private Button button = null;

    public void Init(UnityEngine.Events.UnityAction call)
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(call);
    }

    public void UpdateVisual(bool isFirstStage)
    {
        label.text = isFirstStage ? "Завершить ход" : "Расставьте армии";
    }
}
