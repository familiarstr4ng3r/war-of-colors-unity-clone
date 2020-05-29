using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void ChangeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }
}
