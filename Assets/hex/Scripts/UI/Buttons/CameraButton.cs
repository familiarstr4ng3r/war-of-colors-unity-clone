using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class CameraButton : MonoBehaviour
    {
        private void Start()
        {
            var inputManager = FindObjectOfType<InputManager>();
            var button = GetComponent<Button>();
            button.onClick.AddListener(inputManager.ChangeMode);
        }
    }
}
