using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private Text label = null;

        private MovesManager movesManager = null;

        private GameObject content = null;

        private void Awake()
        {
            movesManager = FindObjectOfType<MovesManager>();

            content = transform.GetChild(0).gameObject;

            ChangeState(false);
        }

        private void OnEnable()
        {
            movesManager.OnGameEnd += OnEnd;
        }

        private void OnDisable()
        {
            movesManager.OnGameEnd -= OnEnd;
        }

        private void OnEnd(Player winner)
        {
            ChangeState(true);

            label.text = $"Победил {winner.Name}!";
        }

        private void ChangeState(bool active)
        {
            content.SetActive(active);
        }
    }
}
