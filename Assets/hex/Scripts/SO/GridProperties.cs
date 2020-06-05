using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New grid", menuName = "SO/New grid")]
public class GridProperties : ScriptableObject
{
    [SerializeField] private TileProperties tile = null;
    [SerializeField, Range(0, 0.2f)] private float gap = 0;
    [SerializeField, Range(0, 1)] private float percentage = 0;

    public TileProperties Tile => tile;
    public float Gap => gap;
    public float Percentage => percentage;
}
