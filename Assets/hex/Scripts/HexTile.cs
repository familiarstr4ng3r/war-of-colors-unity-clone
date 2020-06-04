using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class HexTile : MonoBehaviour
{
    [SerializeField] private Color defaultColor = Color.white;

    private SpriteRenderer spriteRenderer = null;
    private TileText tileText = null;
    private List<HexTile> neighbours = new List<HexTile>();

    private TileData tileData = new TileData();
    public TileData Data => tileData;

    public int Amount
    {
        get => tileData.Amount;
        set
        {
            tileData.Amount = value;

            UpdateVisual();
        }
    }

    private Player player = null;

    public Player Player
    {
        get => player;
        set
        {
            player = value;

            UpdateVisual();
        }
    }

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();

        ChangeColor(defaultColor);
    }

    public void SetText(TileText text)
    {
        tileText = text;
    }

    public void UpdateTextPosition(Camera cam)
    {
        var pos = cam.WorldToScreenPoint(transform.position);
        tileText.SetPosition(pos);
    }

    public void SetNeighbours(List<HexTile> neighbors)
    {
        //neighbours = new List<HexTile>(neighbors);
        neighbours = neighbors;

        UpdateVisual();
    }

    //public void UpdateVisual(Player p)
    //{
    //    Color c = p != null ? p.Color : defaultColor;
    //    ChangeColor(c);
    //    tileText.UpdateText(Amount.ToString());
    //}

    private void ChangeColor(Color newColor)
    {
        spriteRenderer.color = newColor;
    }

    public bool HasNeighbour(HexTile tile)
    {
        return neighbours.Contains(tile);
    }

    //private void OnDrawGizmosSelected()
    //{
    //    Gizmos.color = Color.red;

    //    for (int i = 0, length = neighbours.Count; i < length; i++)
    //    {
    //        Gizmos.DrawLine(transform.position, neighbours[i].transform.position);
    //    }

    //    Gizmos.DrawWireSphere(transform.position, 0.1f);
    //}

    public bool IsEmpty()
    {
        return Player == null;// || tileData.PlayerIndex == -1 || Amount == 0;
    }

    private void UpdateVisual()
    {
        Color newColor = IsEmpty() ? defaultColor : player.Color;
        ChangeColor(newColor);
        tileText.UpdateText(Amount.ToString());

        tileData.PlayerIndex = IsEmpty() ? -1 : player.Index;
    }
}

[Serializable]
public class TileData
{
    public int Amount = 0;
    public int PlayerIndex = 0;
}
