using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class MenuButton : MonoBehaviour
    {
        [SerializeField] private int childIndex = 0;

        private void Start()
        {
            var menu = FindObjectOfType<MenuWindow>();
            var button = GetComponent<Button>();
            button.onClick.AddListener(() => menu.OnClick(childIndex));
        }
    }
}
