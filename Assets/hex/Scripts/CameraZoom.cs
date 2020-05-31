using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraZoom : MonoBehaviour
{
    [SerializeField] private Button addButton = null;
    [SerializeField] private Button substractButton = null;
    [SerializeField] private Vector2 minMaxZoom = new Vector2(3, 5);
    private Camera cam = null;

    private float targetZoom = 0;

    private void Start()
    {
        cam = Camera.main;
        targetZoom = cam.orthographicSize;
        UpdateButtons();

        addButton.onClick.AddListener(() => ChangeZoom(1));
        substractButton.onClick.AddListener(() => ChangeZoom(-1));
    }

    private void ChangeZoom(float value)
    {
        targetZoom += value;
        UpdateButtons();
    }

    private void UpdateButtons()
    {
        addButton.interactable = !Mathf.Approximately(targetZoom, minMaxZoom.y);
        substractButton.interactable = !Mathf.Approximately(targetZoom, minMaxZoom.x);
    }

    private void FixedUpdate()
    {
        cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, 0.2f);
    }
}
