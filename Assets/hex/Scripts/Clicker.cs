using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Clicker : MonoBehaviour
{
    private Camera cam = null;
    private HexTile clickedTile = null;

    private void Start()
    {
        cam = Camera.main;
    }

    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward);

            if (hit.collider)
            {
                clickedTile?.ChangeColor(Color.grey);

                clickedTile = hit.collider.GetComponent<HexTile>();
                clickedTile?.ChangeColor(Color.red);
            }
        }
    }
}
