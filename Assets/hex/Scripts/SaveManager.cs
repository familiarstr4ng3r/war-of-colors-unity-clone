using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace WOC
{
    public class SaveManager : MonoBehaviour
    {
        private MovesManager movesManager = null;

        private string savePath = string.Empty;
        private const string fileName = "WOC_save.json";

        private void Awake()
        {
            movesManager = FindObjectOfType<MovesManager>();

            savePath = Path.Combine(Application.isEditor ? Application.dataPath : Application.persistentDataPath, fileName);
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

        private void OnMoveEnd(Player arg1, bool arg2)
        {
            Save();
        }

        private void OnGameEnd(Player winner)
        {
            DeleteSave();
        }

        private void Save()
        {

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
            }
        }
    }
}

