using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerElement : MonoBehaviour
{
    [SerializeField] private Image colorImage = null;
    [SerializeField] private Button deleteButton = null;
    [SerializeField] private InputField inputField = null;

    private PlayersWindow playersWindow = null;

    private void Start()
    {
        colorImage.color = Random.ColorHSV();

        playersWindow = FindObjectOfType<PlayersWindow>();
        deleteButton.onClick.AddListener(() => playersWindow.DeletePlayer(transform.GetSiblingIndex()));

        //inputField.onValueChanged.AddListener((text) => window.ValidateNames());
    }

    public void UpdateVisual(bool isLast)
    {
        deleteButton.interactable = !isLast;
    }

    public void SetColor(Color newColor)
    {
        colorImage.color = newColor;
    }

    //public bool HasName()
    //{
    //    return inputField.text != string.Empty;
    //}

    public Player CreatePlayer(int index)
    {
        var player = new Player(inputField.text, colorImage.color, index);

        return player;
    }
}
