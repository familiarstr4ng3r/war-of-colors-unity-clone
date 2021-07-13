using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

#pragma warning disable CS0414

public class SelectIconManager : MonoBehaviour
{
    private static GameObject ModalPanel = null;
    private static string SelectedIcon = string.Empty;

    private static readonly string[] ArrayIcons = new string[]
    {
        "Sprites/sprite_01",
        "Sprites/sprite_02",
        "Sprites/sprite_03"
    };

    private static bool IsRendered = false;
    private static bool IsInit = false;//??

    public static void Init()
    {
        SelectedIcon = string.Empty;
        IsRendered = false;

        ModalPanel = GameObject.Find("ModalPanel");//if gameobject is not active - returns null

        ShowModal();
        CloseModal();
    }

    //on click
    private static void SetTitle(string title)
    {
        SelectedIcon = title;
        CloseModal();

        //Debug.Log(title);
    }

    public static void ShowModal()
    {
        ModalPanel.SetActive(true);
        RenderIcons();
    }

    public static void CloseModal()
    {
        ModalPanel.SetActive(false);
    }

    private static void RenderIcons()
    {
        if (!IsRendered)
        {
            SpawnLogic();

            IsRendered = true;
        }
    }

    private static void SpawnLogic()
    {
        //destroying all existing childs

        for (int i = 0; i < ModalPanel.transform.childCount; i++)
        {
            var child = ModalPanel.transform.GetChild(0);
            Destroy(child.gameObject);
        }

        //actual spawning new buttons

        string buttonPrefabPath = "ButtonPrefab";
        Button buttonPrefab = Resources.Load<Button>(buttonPrefabPath);

        for (int i = 0, length = ArrayIcons.Length; i < length; i++)
        {
            var b = Instantiate(buttonPrefab, ModalPanel.transform);
            b.name = "Button " + i;

            var loadedSprite = Resources.Load<Sprite>(ArrayIcons[i]);
            if (loadedSprite) b.GetComponent<Image>().sprite = loadedSprite;
            int k = i;//its important https://stackoverflow.com/questions/271440/captured-variable-in-a-loop-in-c-sharp
            b.onClick.AddListener(() => SetTitle(ArrayIcons[k]));
        }
    }
}
