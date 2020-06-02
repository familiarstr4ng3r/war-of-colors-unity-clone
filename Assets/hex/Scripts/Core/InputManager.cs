using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputManager : MonoBehaviour
{
    [SerializeField] private bool isCameraMode = false;
    private WOC.CameraController cameraController = null;

    private void Start()
    {
        cameraController = FindObjectOfType<WOC.CameraController>();
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
