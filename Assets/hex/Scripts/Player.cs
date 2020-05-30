using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string name;
    public Color color = Color.white;

    private List<HexTile> tiles = new List<HexTile>();

    public Player(string playerName, Color imageColor)
    {
        name = playerName;
        color = imageColor;
    }

    public void AddTile(HexTile tile)
    {
        tile.UpdateVisual(this);

        tiles.Add(tile);
    }

    public bool HasTile(HexTile tile)
    {
        return tiles.Contains(tile);
    }
}
