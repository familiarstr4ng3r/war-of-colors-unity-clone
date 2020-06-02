using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOC
{
    public class CameraController : MonoBehaviour
    {
        [SerializeField, Range(0, 1)] private float dampSpeed = 0;
        [SerializeField, Range(0, 1)] private float zoomSpeed = 1;
        [SerializeField] private Vector2 minMaxZoom = new Vector2();

        private bool isDragging = false;
        private Camera cam = null;
        private Plane plane = new Plane(Vector3.back, Vector3.zero);

        private Vector3 clickedPosition = Vector3.zero;
        private Vector3 draggingPosition = Vector3.zero;
        private Vector3 desiredPosition = Vector3.zero;
        private Vector3 smoothVelocity = Vector3.zero;

        private float startZ = 0;

        private Vector3 minPosition = Vector3.zero;
        private Vector3 maxPosition = Vector3.zero;

        private float targetZoom = 0;

        private void Start()
        {
            cam = Camera.main;
            targetZoom = cam.orthographicSize;
            startZ = cam.transform.position.z;
        }

        private void FixedUpdate()
        {
            HandlePosition();
        }

        public void Init(Vector3 min, Vector3 max)
        {
            minPosition = min;
            maxPosition = max;
        }

        public void HandleCamera()
        {
            bool isZooming = Input.touchCount == 2;

            if (isZooming)
            {
                HandleZooming();
            }
            else
            {
                HandleDragging();
            }
        }

        private void HandleZooming()
        {
            var a = Input.touches[0];
            var b = Input.touches[1];

            var aPrev = a.position - a.deltaPosition;
            var bPrev = b.position - b.deltaPosition;

            float prevMagnitude = (aPrev - bPrev).magnitude;
            float currentMagnitude = (a.position - b.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            targetZoom -= difference * 0.01f;
            targetZoom = Mathf.Clamp(targetZoom, minMaxZoom.x, minMaxZoom.y);
        }

        private void HandleDragging()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;

                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                if (plane.Raycast(ray, out var enter)) clickedPosition = ray.GetPoint(enter);
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
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

        private void HandlePosition()
        {
            Vector3 finalPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref smoothVelocity, dampSpeed);
            finalPosition.z = startZ;
            finalPosition.x = Mathf.Clamp(finalPosition.x, minPosition.x, maxPosition.x);
            finalPosition.y = Mathf.Clamp(finalPosition.y, minPosition.y, maxPosition.y);
            transform.position = finalPosition;

            cam.orthographicSize = Mathf.Lerp(cam.orthographicSize, targetZoom, zoomSpeed);
        }
    }
}

