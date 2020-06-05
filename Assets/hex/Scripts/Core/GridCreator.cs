using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOC
{
    public class GridCreator : MonoBehaviour
    {
        public int Width = 0;
        public int Height = 0;
        public HexTile[,] Grid => grid;

        [SerializeField] private int startAmount = 20;
        [SerializeField] private GridProperties gridProperties = null;
        [SerializeField] private Transform textParent = null;
        [SerializeField] private TileText textPrefab = null;

        private Vector3 startPos = Vector3.zero;
        private Vector2 hexSize = Vector2.zero;
        private HexTile[,] grid = null;

        private CameraController cameraController = null;

        public void Create()
        {
            cameraController = FindObjectOfType<CameraController>();

            SpawnGrid();
            SetNeighbours();

            cameraController.Init(startPos, -startPos);
        }

        public void SetPlayers(List<Player> players)
        {
            //prevent max players count (fool-proof)

            int maxCount = Mathf.Min(players.Count, Width * Height);

            while (players.Count > maxCount)
            {
                players.RemoveAt(players.Count - 1);
            }

            //calculation unique position for each player

            List<Vector2Int> positions = new List<Vector2Int>();
            Vector2Int candidate = Vector2Int.zero;

            for (int i = 0, length = players.Count; i < length; i++)
            {
                while (positions.Contains(candidate))
                {
                    candidate.x = Random.Range(0, Width);
                    candidate.y = Random.Range(0, Height);
                }

                positions.Add(candidate);

                var tile = grid[candidate.x, candidate.y];

                tile.Player = players[i];
                tile.Amount = startAmount;

                players[i].TilesCount++;
            }
        }

        public void Load(SaveData saveData)
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = y * Width + x;
                    
                    var data = saveData.Tiles[index];
                    bool hasPlayer = data.PlayerIndex != -1;

                    grid[x, y].Player = hasPlayer ? saveData.Players[data.PlayerIndex] : null;
                    grid[x, y].Amount = data.Amount;
                }
            }
        }

        private void LateUpdate()
        {
            if (grid == null) return;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    grid[x, y].UpdateTextPosition(cameraController.Camera);
                }
            }
        }

        //

        private void SpawnGrid()
        {
            hexSize = gridProperties.Tile.Size;
            hexSize += hexSize * gridProperties.Gap;

            startPos = CalculateStartPos();

            grid = new HexTile[Width, Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    Vector3 worldPos = Convert(new Vector2Int(x, y));
                    var hexTile = Instantiate(gridProperties.Tile.Prefab, worldPos, Quaternion.identity, transform);

                    Vector3 screenPos = cameraController.Camera.WorldToScreenPoint(worldPos);
                    TileText text = Instantiate(textPrefab, screenPos, Quaternion.identity, textParent);
                    hexTile.SetText(text);

                    int index = y * Width + x;

                    hexTile.name = $"{index}: Tile {x}/{y}";
                    grid[x, y] = hexTile;
                }
            }
        }

        private Vector3 Convert(Vector2Int gridPos)
        {
            float offset = 0;

            if (gridPos.y % 2 != 0) offset = hexSize.x * 0.5f;

            float x = startPos.x + gridPos.x * hexSize.x + offset;
            float y = startPos.y + gridPos.y * hexSize.y * gridProperties.Percentage;

            return new Vector3(x, y);
        }

        private Vector3 CalculateStartPos()
        {
            float offset = 0;
            if (Height / 2 % 2 != 0) offset = hexSize.x * 0.5f;

            float x = -hexSize.x * (Width / 2) - offset;
            float y = -hexSize.y * gridProperties.Percentage * (Height / 2);

            return new Vector3(x, y);
        }

        //

        private void SetNeighbours()
        {
            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
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
            bool inside = x >= 0 && y >= 0 && x < Width && y < Height;
            return inside;
        }
    }
}
