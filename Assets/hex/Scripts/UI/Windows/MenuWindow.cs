using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private Button startButton = null;
        [SerializeField] private List<GameObject> windows = new List<GameObject>();
        [SerializeField] private GameObject gameplayUI = null;

        private GameObject content = null;

        private void Start()
        {
            content = transform.GetChild(0).gameObject;

            startButton.onClick.AddListener(StartGame);

            OnClick(1);
        }

        private void StartGame()
        {
            var manager = FindObjectOfType<MovesManager>();
            var players = FindObjectOfType<PlayersWindow>().Players;

            for (int i = 0, length = players.Count; i < length; i++)
            {
                var player = players[i].CreatePlayer(i);
                manager.AddPlayer(player);
            }

            gameplayUI.SetActive(true);
            manager.Init();

            ChangeState(false);
        }

        private void ChangeState(bool active)
        {
            content.SetActive(active);
        }

        public void OnClick(int index)
        {
            for (int i = 0, length = windows.Count; i < length; i++)
            {
                var w = windows[i].transform;
                var content = w.GetChild(0).gameObject;

                bool active = index == w.GetSiblingIndex();

                content.SetActive(active);
            }
        }
    }
}
