using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
        movesManager.OnEnd += OnEnd;
    }

    private void OnDisable()
    {
        movesManager.OnEnd -= OnEnd;
    }

    private void OnEnd(Player winner)
    {
        ChangeState(true);

        label.text = $"Победил {winner.name}!";
    }

    private void ChangeState(bool active)
    {
        content.SetActive(active);
    }
}
