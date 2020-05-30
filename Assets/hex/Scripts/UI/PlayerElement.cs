using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerElement : MonoBehaviour
{
    [SerializeField] private Image colorImage = null;
    [SerializeField] private Button deleteButton = null;

    private void Start()
    {
        colorImage.color = Random.ColorHSV();

        var window = FindObjectOfType<PlayersWindow>();
        deleteButton.onClick.AddListener(() => window.DeletePlayer(transform.GetSiblingIndex()));
    }
}
