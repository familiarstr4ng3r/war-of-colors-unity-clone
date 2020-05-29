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
    [SerializeField] private int size = 1;
    [SerializeField] private GameObject textPrefab = null;

    //0.8 0.92 ui
    //0.88 1 world

    private Vector3 startPos = Vector3.zero;
    private float screenHeight = 0;
    private HexTile[,] grid = null;

    private void Start()
    {
        screenHeight = FindObjectOfType<CanvasScaler>().referenceResolution.y;

        SpawnGrid(parent);
    }

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
                grid[x, y] = tile;

                var rect = tile.GetComponent<RectTransform>();

                if (rect)
                {
                    rect.sizeDelta *= size;
                }
                else
                {
                    var textParent = GameObject.Find("TextParent").transform;
                    var p = Camera.main.WorldToScreenPoint(pos);
                    var text = Instantiate(textPrefab, p, Quaternion.identity, textParent);
                }
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
