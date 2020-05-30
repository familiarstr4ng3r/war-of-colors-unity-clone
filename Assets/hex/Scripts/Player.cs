using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string name;
    public Color color = Color.white;
    public int AvailableAmount = 0;

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

    public void RemoveTile(HexTile tile)
    {
        tile.UpdateVisual(null);

        tiles.Remove(tile);
    }

    public bool HasTile(HexTile tile)
    {
        return tiles.Contains(tile);
    }

    public bool IsLooser()
    {
        return tiles.Count == 0;
    }

    public void OnSecondStage()
    {
        AvailableAmount += tiles.Count;
    }

    public override string ToString()
    {
        string t = IsLooser() ? "is looser" : "has " + tiles.Count + " tiles";
        return $"{name} {t}";
    }
}
