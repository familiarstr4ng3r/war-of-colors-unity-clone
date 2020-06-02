using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerColorButton : MonoBehaviour
{
    private void Start()
    {
        var paletteWindow = FindObjectOfType<PaletteWindow>();
        var button = GetComponent<Button>();
        var index = transform.parent.GetSiblingIndex();
        //Debug.Log(index);
        button.onClick.AddListener(() => paletteWindow.OnPlayerClick(index));
    }
}
