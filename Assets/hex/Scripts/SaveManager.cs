using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WOC
{
    public class SaveManager : MonoBehaviour
    {
        [SerializeField] private bool loadAtStart = false;

        private MovesManager movesManager = null;

        private string savePath = string.Empty;
        private const string fileName = "WOC_save.json";

        private void Awake()
        {
            movesManager = FindObjectOfType<MovesManager>();

            savePath = Path.Combine(Application.isEditor ? Application.dataPath : Application.persistentDataPath, fileName);
        }

        private void Start()
        {
            if (loadAtStart) Load();
        }

        private void OnEnable()
        {
            movesManager.OnMoveEnd += OnMoveEnd;
            movesManager.OnGameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            movesManager.OnMoveEnd -= OnMoveEnd;
            movesManager.OnGameEnd -= OnGameEnd;
        }

        private void OnMoveEnd(Player currentPlayer, bool isFirstStage, List<Player> players, GridCreator grid)
        {
            Save(currentPlayer, isFirstStage, players, grid);
        }

        private void OnGameEnd(Player winner)
        {
            DeleteSave();
        }

        private void Save(Player currentPlayer, bool isFirstStage, List<Player> players, GridCreator grid)
        {
            var data = new SaveData(currentPlayer, isFirstStage, players, grid);

            string json = JsonUtility.ToJson(data, true);

            File.WriteAllText(savePath, json);
        }

        private void DeleteSave()
        {
            if (File.Exists(savePath))
            {
                File.Delete(savePath);
            }
        }

        private void Load()
        {
            if (File.Exists(savePath))
            {
                string json = File.ReadAllText(savePath);
                var data = JsonUtility.FromJson<SaveData>(json);

                FindObjectOfType<MovesManager>().Load(data);
            }
        }
    }

    [System.Serializable]
    public class SaveData
    {
        public int Width = 0;
        public int Height = 0;
        public bool IsFirstStage = false;
        public int CurrentPlayerIndex = 0;
        public Player[] Players = null;
        public TileData[] Tiles = null;

        public SaveData(Player currentPlayer, bool isFirstStage, List<Player> players, GridCreator gridCreator)
        {
            Width = gridCreator.Width;
            Height = gridCreator.Height;

            IsFirstStage = isFirstStage;
            CurrentPlayerIndex = currentPlayer.Index;

            Players = players.ToArray();

            Tiles = new TileData[Width * Height];

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    int index = y * Width + x;
                    Tiles[index] = gridCreator.Grid[x, y].Data;
                }
            }
        }
    }
}

