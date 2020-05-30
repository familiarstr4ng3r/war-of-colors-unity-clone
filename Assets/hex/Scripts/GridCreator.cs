using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private HexTile tilePrefab = null;
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
    [SerializeField] private Vector2 hexSize = Vector2.zero;
    [SerializeField] private float gap = 0;
    [SerializeField, Range(0, 1)] private float percentage = 0;
    [Space]
    [SerializeField] private Transform parent = null;
    [SerializeField] private Transform textParent = null;
    [SerializeField] private int size = 1;
    [SerializeField] private TileText textPrefab = null;

    //0.8 0.92 ui
    //0.88 1 world

    private Vector3 startPos = Vector3.zero;
    private HexTile[,] grid = null;

    public void Create()
    {
        SpawnGrid(parent);
        SetNeighbours();
    }

    public void SetPlayers(List<Player> players, int startAmount)
    {
        for (int i = 0, length = players.Count; i < length; i++)
        {
            int x = Random.Range(0, gridSize.x);
            int y = Random.Range(0, gridSize.y);

            var tile = grid[x, y];
            tile.Amount = startAmount;

            players[i].AddTile(tile);
        }
    }

    //

    private void SpawnGrid(Transform parent)
    {
        hexSize += hexSize * gap;
        hexSize *= size;

        startPos = CalculateStartPos();

        grid = new HexTile[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 pos = Convert(new Vector2Int(x, y));
                var tile = Instantiate(tilePrefab, pos, Quaternion.identity, parent);
                tile.name = $"Tile {x}/{y}";

                var p = Camera.main.WorldToScreenPoint(pos);
                TileText text = Instantiate(textPrefab, p, Quaternion.identity, textParent);
                tile.SetText(text);

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

    //

    private void SetNeighbours()
    {
        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                var list = GenerateNeighbours(new Vector2Int(x, y));
                grid[x, y].SetNeighbours(list);
            }
        }
    }

    private List<HexTile> GenerateNeighbours(Vector2Int gridPos)
    {
        List<HexTile> neighbours = new List<HexTile>();

        if (IsInside(gridPos.x - 1, gridPos.y)) neighbours.Add(grid[gridPos.x - 1, gridPos.y]);//left
        if (IsInside(gridPos.x + 1, gridPos.y)) neighbours.Add(grid[gridPos.x + 1, gridPos.y]);//right

        if (gridPos.y % 2 != 0)
        {
            //bottom
            if (IsInside(gridPos.x + 1, gridPos.y + 1)) neighbours.Add(grid[gridPos.x + 1, gridPos.y + 1]);
            if (IsInside(gridPos.x + 0, gridPos.y + 1)) neighbours.Add(grid[gridPos.x + 0, gridPos.y + 1]);

            //top
            if (IsInside(gridPos.x + 1, gridPos.y - 1)) neighbours.Add(grid[gridPos.x + 1, gridPos.y - 1]);
            if (IsInside(gridPos.x + 0, gridPos.y - 1)) neighbours.Add(grid[gridPos.x + 0, gridPos.y - 1]);
        }
        else
        {
            //bottom
            if (IsInside(gridPos.x + 0, gridPos.y + 1)) neighbours.Add(grid[gridPos.x + 0, gridPos.y + 1]);
            if (IsInside(gridPos.x - 1, gridPos.y + 1)) neighbours.Add(grid[gridPos.x - 1, gridPos.y + 1]);

            //top
            if (IsInside(gridPos.x + 0, gridPos.y - 1)) neighbours.Add(grid[gridPos.x + 0, gridPos.y - 1]);
            if (IsInside(gridPos.x - 1, gridPos.y - 1)) neighbours.Add(grid[gridPos.x - 1, gridPos.y - 1]);
        }

        return neighbours;
    }

    private bool IsInside(int x, int y)
    {
        bool inside = x >= 0 && y >= 0 && x < gridSize.x && y < gridSize.y;
        return inside;
    }
}
