using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class MenuWindow : MonoBehaviour
    {
        [SerializeField] private Button m_StartButton = null;
        [SerializeField] private Button m_LoadButton = null;

        [SerializeField] private List<GameObject> windows = new List<GameObject>();
        [SerializeField] private GameObject gameplayUI = null;
        [SerializeField] private GameObject m_Content = null;

        private MovesManager m_MovesManager = null;

        private void Start()
        {
            m_MovesManager = FindObjectOfType<MovesManager>();

            m_StartButton.onClick.AddListener(() => StartGame(true));
            m_LoadButton.onClick.AddListener(() => StartGame(false));

            m_LoadButton.interactable = SaveManager.HasSave(SaveHandler.FileName, out var temp);

            OnClick(1);
        }

        private void StartGame(bool newGame)
        {
            gameplayUI.SetActive(true);

            if (newGame)
            {
                List<PlayerElement> players = FindObjectOfType<PlayersWindow>().Players;

                for (int i = 0, length = players.Count; i < length; i++)
                {
                    Player player = players[i].CreatePlayer(i);
                    m_MovesManager.AddPlayer(player);
                }

                m_MovesManager.Init();
            }
            else
            {
                if (SaveManager.Load(SaveHandler.FileName, out SaveData data))
                {
                    m_MovesManager.Load(data);
                }
                else
                {
                    Debug.LogWarning("load save error");
                }
            }

            ChangeState(false);
        }

        private void ChangeState(bool active)
        {
            m_Content.SetActive(active);
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
