using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class PlayerInfo : MonoBehaviour
    {
        private MovesManager movesManager = null;

        [SerializeField] private Image backgroundImage = null;
        [SerializeField] private Text label = null;

        private void Awake()
        {
            movesManager = FindObjectOfType<MovesManager>();
        }

        private void OnEnable()
        {
            movesManager.OnMoveEnd += OnMoveEnd;
        }

        private void OnDisable()
        {
            movesManager.OnMoveEnd -= OnMoveEnd;
        }

        private void OnMoveEnd(OnMoveEndEventArgs args)
        {
            string t = args.IsFirstStage ? "Фаза атаки" : "Фаза пополнения";
            string t1 = args.IsFirstStage ? string.Empty : $"Войск в наличии - {args.CurrentPlayer.AvailableAmount}";
            label.text = $"{t} - сейчас ходит {args.CurrentPlayer.Name} {t1}";

            backgroundImage.color = args.CurrentPlayer.Color;
        }
    }
}