using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridCreator : MonoBehaviour
{
    [SerializeField] private TileProperties tile = null;
    [SerializeField] private Vector2Int gridSize = Vector2Int.one;
    [SerializeField, Range(0, 0.2f)] private float gap = 0;
    [SerializeField, Range(0, 1)] private float percentage = 0;
    [Space]
    [SerializeField] private Transform tilesParent = null;
    [SerializeField] private Transform textParent = null;
    [SerializeField] private TileText textPrefab = null;

    //0.8 0.92 ui
    //0.88 1 world

    private Vector3 startPos = Vector3.zero;
    private HexTile[,] grid = null;
    private Vector2 hexSize = Vector2.zero;

    private Camera cam = null;

    public void Create(Vector2Int gridSize)
    {
        cam = Camera.main;

        SpawnGrid(gridSize);
        SetNeighbours();

        var min = Vector2.zero;
        var max = Vector2.zero;

        min.x = startPos.x;
        min.y = -startPos.y;

        max.x = -startPos.x;
        max.y = startPos.y;

        FindObjectOfType<WOC.CameraController>().Init(min, max);
    }

    public void SetPlayers(List<Player> players, int startAmount)
    {
        int maxCount = Mathf.Min(players.Count, gridSize.x * gridSize.y);

        while(players.Count > maxCount)
        {
            players.RemoveAt(players.Count - 1);
        }

        List<Vector2Int> positions = new List<Vector2Int>();
        Vector2Int candidate = Vector2Int.zero;

        for (int i = 0, length = players.Count; i < length; i++)
        {
            while(positions.Contains(candidate))
            {
                candidate.x = Random.Range(0, gridSize.x);
                candidate.y = Random.Range(0, gridSize.y);
            }

            positions.Add(candidate);

            var tile = grid[candidate.x, candidate.y];

            tile.Player = players[i];
            tile.Amount = startAmount;

            players[i].TilesCount++;
        }
    }

    private void LateUpdate()
    {
        if (grid == null) return;

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                grid[x, y].UpdateTextPosition(cam);
            }
        }
    }

    //

    private void SpawnGrid(Vector2Int gridSize)
    {
        this.gridSize = gridSize;

        hexSize = tile.Size;
        hexSize += hexSize * gap;

        startPos = CalculateStartPos();

        grid = new HexTile[gridSize.x, gridSize.y];

        for (int y = 0; y < gridSize.y; y++)
        {
            for (int x = 0; x < gridSize.x; x++)
            {
                Vector3 pos = Convert(new Vector2Int(x, y));
                var hexTile = Instantiate(tile.Prefab, pos, Quaternion.identity, tilesParent);

                var p = Camera.main.WorldToScreenPoint(pos);
                TileText text = Instantiate(textPrefab, p, Quaternion.identity, textParent);
                hexTile.SetText(text);

                hexTile.name = $"Tile {x}/{y}";
                grid[x, y] = hexTile;
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
