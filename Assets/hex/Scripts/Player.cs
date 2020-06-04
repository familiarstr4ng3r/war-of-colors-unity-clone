using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string Name;
    public Color Color = Color.white;
    public int AvailableAmount = 0;
    public int Index = 0;
    public int TilesCount = 0;

    public Player(string playerName, Color imageColor, int index)
    {
        Name = playerName;
        Color = imageColor;
        Index = index;
    }

    public bool HasTile(HexTile tile)
    {
        return Index == tile.Data.PlayerIndex;
    }

    public bool IsLooser()
    {
        return TilesCount == 0;
    }

    public void OnSecondStage()
    {
        AvailableAmount += TilesCount;
    }

    public override string ToString()
    {
        string t = IsLooser() ? "is looser" : "has " + TilesCount + " tiles";
        return $"{Name} {t}";
    }
}
