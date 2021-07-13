using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace WOC
{
    public class SaveHandler : MonoBehaviour
    {
        public const string FileName = "WOC_save.json";

        private MovesManager m_MovesManager = null;
        private GridCreator m_Grid = null;

        private void Awake()
        {
            m_MovesManager = FindObjectOfType<MovesManager>();
            m_Grid = FindObjectOfType<GridCreator>();
        }

        private void OnEnable()
        {
            m_MovesManager.OnMoveEnd += OnMoveEnd;
            m_MovesManager.OnGameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            m_MovesManager.OnMoveEnd -= OnMoveEnd;
            m_MovesManager.OnGameEnd -= OnGameEnd;
        }

        private void OnMoveEnd(OnMoveEndEventArgs args)
        {
            var data = new SaveData(args.CurrentPlayer, args.IsFirstStage, args.Players, m_Grid);
            SaveManager.Save(FileName, data);
        }

        private void OnGameEnd(OnGameEndEventArgs args)
        {
            SaveManager.DeleteSave(FileName);
        }
    }
}
