using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOC
{
    public class InputManager : MonoBehaviour
    {
        [SerializeField] private bool isCameraMode = false;
        private CameraController cameraController = null;

        private void Start()
        {
            cameraController = FindObjectOfType<CameraController>();
        }

        private void Update()
        {
            if (isCameraMode)
            {
                cameraController.HandleCamera();
            }
            else
            {
                HandleInput();
            }
        }

        private void HandleInput()
        {

        }

        public void ChangeMode()
        {
            isCameraMode = !isCameraMode;
        }
    }
}
