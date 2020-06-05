using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private Button startButton = null;
        [SerializeField] private Button loadButton = null;

        [SerializeField] private List<GameObject> windows = new List<GameObject>();
        [SerializeField] private GameObject gameplayUI = null;

        private MovesManager movesManager = null;
        private SaveManager saveManager = null;

        private GameObject content = null;

        private void Start()
        {
            movesManager = FindObjectOfType<MovesManager>();
            saveManager = FindObjectOfType<SaveManager>();

            content = transform.GetChild(0).gameObject;

            startButton.onClick.AddListener(() => StartGame(true));
            loadButton.onClick.AddListener(() => StartGame(false));

            loadButton.interactable = saveManager.HasSave;

            OnClick(1);
        }

        private void StartGame(bool newGame)
        {
            gameplayUI.SetActive(true);

            if (newGame)
            {
                var players = FindObjectOfType<PlayersWindow>().Players;

                for (int i = 0, length = players.Count; i < length; i++)
                {
                    var player = players[i].CreatePlayer(i);
                    movesManager.AddPlayer(player);
                }

                movesManager.Init();
            }
            else
            {
                saveManager.Load();
            }

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
