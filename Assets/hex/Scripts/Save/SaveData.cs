using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOC
{
    [System.Serializable]
    public class SaveData : ISaveable
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

        public string Serialize()
        {
            return JsonUtility.ToJson(this, true);
        }
    }
}
