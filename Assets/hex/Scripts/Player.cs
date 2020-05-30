using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Player
{
    public string name;
    public Color color = Color.white;

    public Player(string playerName, Color imageColor)
    {
        name = playerName;
        color = imageColor;
    }
}
