using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerElement : MonoBehaviour
{
    [SerializeField] private Image colorImage = null;
    [SerializeField] private Button deleteButton = null;
    [SerializeField] private InputField inputField = null;

    private void Start()
    {
        colorImage.color = Random.ColorHSV();

        var window = FindObjectOfType<PlayersWindow>();
        deleteButton.onClick.AddListener(() => window.DeletePlayer(transform.GetSiblingIndex()));

        //inputField.onValueChanged.AddListener((text) => window.ValidateNames());
    }

    public void UpdateVisual(bool isLast)
    {
        deleteButton.interactable = !isLast;
    }

    //public bool HasName()
    //{
    //    return inputField.text != string.Empty;
    //}

    public Player CreatePlayer()
    {
        var player = new Player(inputField.text, colorImage.color);

        return player;
    }
}
