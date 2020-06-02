using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PaletteWindow : MonoBehaviour
{
    [SerializeField] private GameObject content = null;
    [SerializeField] private Button closeButton = null;
    [SerializeField] private Transform elementsParent = null;
    [SerializeField] private PaletteElementButton elementPrefab = null;
    [SerializeField] private Color[] colors = null;
    private PlayersWindow playersWindow = null;

    private int playerIndex = 0;

    private void Start()
    {
        playersWindow = FindObjectOfType<PlayersWindow>();

        PopulateButtons();

        closeButton.onClick.AddListener(() => SetState(false));

        SetState(false);
    }

    private void PopulateButtons()
    {
        //delete existing
        for (int i = 0; i < elementsParent.childCount; i++)
        {
            var child = elementsParent.GetChild(i);
            Destroy(child.gameObject);
        }

        //create new
        for (int i = 0, length = colors.Length; i < length; i++)
        {
            var element = Instantiate(elementPrefab, elementsParent);
            element.UpdateVisual(false, colors[i]);

            int k = i;
            element.OnClick = () => OnColorClick(k);
        }

        Canvas.ForceUpdateCanvases();
    }

    public void OnPlayerClick(int _playerIndex)
    {
        SetState(true);
        playerIndex = _playerIndex;
    }

    private void OnColorClick(int colorIndex)
    {
        var playerElement = playersWindow.Players[playerIndex];
        var newColor = colors[colorIndex];
        playerElement.SetColor(newColor);
        //Debug.Log(colorIndex);
    }

    private void SetState(bool active)
    {
        content.SetActive(active);
    }
}
