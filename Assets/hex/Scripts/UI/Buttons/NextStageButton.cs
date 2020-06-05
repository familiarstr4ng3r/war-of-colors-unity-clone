using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class NextStageButton : MonoBehaviour
    {
        [SerializeField] private Text label = null;

        private Button button = null;
        private MovesManager movesManager = null;

        private void Awake()
        {
            button = GetComponent<Button>();
            movesManager = FindObjectOfType<MovesManager>();
            button.onClick.AddListener(movesManager.NextStage);
        }

        private void OnEnable()
        {
            movesManager.OnMoveEnd += OnMoveEnd;
        }

        private void OnDisable()
        {
            movesManager.OnMoveEnd -= OnMoveEnd;
        }

        private void OnMoveEnd(Player currentPlayer, bool isFirstStage, List<Player> players, GridCreator grid)
        {
            label.text = isFirstStage ? "Завершить ход" : "Расставьте армии";
        }
    }
}
