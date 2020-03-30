using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField] private bool handlePosition = true;

    [Header("Properties")]
    [SerializeField, Range(0, 1)] private float sensitivity = 1;
    [SerializeField, Range(0, 1)] private float dampSpeed = 1;
    [SerializeField, Range(0, 1)] private float turnSpeed = 1;

    [SerializeField] private KeyCode key = KeyCode.Mouse1;
    [SerializeField] private Vector2 minMax = Vector2.zero;

    private bool isDragging = false;
    private Vector3 clickedPosition = Vector3.zero;
    private Vector3 draggingPosition = Vector3.zero;
    private Vector3 desiredPosition = Vector3.zero;

    private Vector3 smoothVelocity = Vector3.zero;
    private Plane plane = new Plane(Vector3.up, Vector3.zero);
    private Camera cam = null;

    private float yaw = 0;
    private float pitch = 0;

    private void Start()
    {
        cam = Camera.main;

        yaw = transform.eulerAngles.y;
        pitch = transform.eulerAngles.x;
    }

    private void Update()
    {
        if (handlePosition) HandlePosition();
        else HandleRotation();

        if (Input.GetKeyDown(KeyCode.Tab)) handlePosition = !handlePosition;
    }

    private void FixedUpdate()
    {
        Quaternion euler = Quaternion.Euler(pitch, yaw, 0);
        Quaternion smoothEuler = Quaternion.Lerp(transform.rotation, euler, turnSpeed);
        transform.rotation = smoothEuler;

        Vector3 finalPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, dampSpeed);

        float radius = 5;
        Vector3 middle = Vector3.zero;
        Vector3 dir = finalPosition - middle;

        if (dir.magnitude > radius)
            finalPosition = dir.normalized * radius;

        transform.position = finalPosition;
    }

    private void HandlePosition()
    {
        if (Input.GetKeyDown(key))
        {
            isDragging = true;

            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var enter)) clickedPosition = ray.GetPoint(enter);
        }
        else if (Input.GetKeyUp(key))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            if (plane.Raycast(ray, out var enter))
            {
                draggingPosition = ray.GetPoint(enter);
                desiredPosition = transform.position + clickedPosition - draggingPosition;
            }
        }
    }

    private void HandleRotation()
    {
        if (Input.GetKeyDown(key))
        {
            isDragging = true;

            clickedPosition = Input.mousePosition;
        }
        else if (Input.GetKeyUp(key))
        {
            isDragging = false;
        }

        if (isDragging)
        {
            draggingPosition = Input.mousePosition;

            Vector3 diff = clickedPosition - draggingPosition;
            clickedPosition = draggingPosition;

            yaw -= diff.x * sensitivity;
            yaw %= 360;

            pitch += diff.y * sensitivity;
            pitch = Mathf.Clamp(pitch, minMax.x, minMax.y);
        }
    }
}
