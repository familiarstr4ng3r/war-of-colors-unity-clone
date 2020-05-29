using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private HexTile tilePrefab = null;
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
    [SerializeField] private Vector2 hexSize = Vector2.zero;
    [SerializeField] private float gap = 0;
    [SerializeField, Range(0, 1)] private float percentage = 0;

    private Vector3 startPos = Vector3.zero;

    private HexTile[,] grid = null;

    private void Start()
    {
        SpawnGrid();
    }

    private void SpawnGrid()
    {
        hexSize += hexSize * gap;

        startPos = CalculateStartPos();

        grid = new HexTile[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 pos = Convert(new Vector2Int(x, y));
                var tile = Instantiate(tilePrefab, pos, Quaternion.identity, transform);
                tile.name = $"Tile {x}/{y}";
                grid[x, y] = tile;
            }
        }
    }

    private Vector3 Convert(Vector2Int gridPos)
    {
        float offset = 0;

        if (gridPos.y % 2 != 0) offset = hexSize.x * 0.5f;

        float x = startPos.x + gridPos.x * hexSize.x + offset;
        float y = startPos.y - gridPos.y * hexSize.y * percentage;

        return new Vector3(x, y);
    }

    private Vector3 CalculateStartPos()
    {
        float offset = 0;
        if (gridSize.y / 2 % 2 != 0) offset = hexSize.x * 0.5f;

        float x = -hexSize.x * (gridSize.x / 2) - offset;
        float y = hexSize.y * percentage * (gridSize.y / 2);

        return new Vector3(x, y);
    }
}
