using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WOC
{
    public class GameOverWindow : MonoBehaviour
    {
        [SerializeField] private Text m_Label = null;
        [SerializeField] private GameObject m_Content = null;

        private MovesManager m_MovesManager = null;

        private void Awake()
        {
            m_MovesManager = FindObjectOfType<MovesManager>();
        }

        private void Start()
        {
            ChangeState(false);
        }

        private void OnEnable()
        {
            m_MovesManager.OnGameEnd += OnGameEnd;
        }

        private void OnDisable()
        {
            m_MovesManager.OnGameEnd -= OnGameEnd;
        }

        private void OnGameEnd(OnGameEndEventArgs args)
        {
            ChangeState(true);

            m_Label.text = $"Победил {args.Winner.Name}!";
        }

        private void ChangeState(bool active)
        {
            m_Content.SetActive(active);
        }
    }
}
