using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New tile", menuName = "SO/New tile")]
public class TileProperties : ScriptableObject
{
    [SerializeField] private HexTile tilePrefab = null;
    [SerializeField] private Vector2 tileSize = Vector2.zero;

    public HexTile Prefab => tilePrefab;
    public Vector2 Size => tileSize;
}
