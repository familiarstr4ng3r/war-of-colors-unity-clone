using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    private SpriteRenderer spriteRenderer = null;
    private TileText tileText = null;
    private List<HexTile> neighbours = new List<HexTile>();

    public int Amount = 0;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    public void SetText(TileText text)
    {
        tileText = text;
    }

    public void SetNeighbours(List<HexTile> neighbors)
    {
        //neighbours = new List<HexTile>(neighbors);
        neighbours = neighbors;

        tileText.UpdateText(Amount.ToString());
    }

    public void UpdateVisual(Player p)
    {
        ChangeColor(p.color);
        tileText.UpdateText(Amount.ToString());
    }

    private void ChangeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public bool HasNeighbour(HexTile tile)
    {
        return neighbours.Contains(tile);
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;

        for (int i = 0, length = neighbours.Count; i < length; i++)
        {
            Gizmos.DrawLine(transform.position, neighbours[i].transform.position);
        }

        Gizmos.DrawWireSphere(transform.position, 0.1f);
    }
}
