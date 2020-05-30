using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersWindow : MonoBehaviour
{
    [SerializeField] private PlayerElement playerPrefab = null;
    [SerializeField] private Transform playersParent = null;
    [SerializeField] private Button addPlayerButton = null;
    [SerializeField] private Button startGameButton = null;

    private List<PlayerElement> players = new List<PlayerElement>();

    private int minPlayersCount = 2;

    private void Start()
    {
        addPlayerButton.onClick.AddListener(AddPlayer);
        startGameButton.onClick.AddListener(StartGame);

        PopulateWindow();
    }

    private void PopulateWindow()
    {
        for (int i = 0; i < playersParent.childCount - 1; i++)
        {
            Destroy(playersParent.GetChild(i).gameObject);
        }

        for (int i = 0; i < minPlayersCount; i++)
        {
            AddPlayer();
        }
    }

    private void AddPlayer()
    {
        var element = Instantiate(playerPrefab, playersParent);

        int index = playersParent.childCount - 2;
        element.transform.SetSiblingIndex(index);

        players.Add(element);

        if (players.Count >= minPlayersCount) UpdateFirst();
    }
    
    public void DeletePlayer(int index)
    {
        //Debug.Log(index);

        var element = players[index];
        players.Remove(element);

        Destroy(element.gameObject);

        UpdateFirst();
    }

    private void UpdateFirst()
    {
        for (int i = 0; i < minPlayersCount; i++)
        {
            players[i].UpdateVisual(players.Count == minPlayersCount);
        }

        ValidateNames();
    }

    public void ValidateNames()
    {
        bool active = true;

        for (int i = 0, length = players.Count; i < length; i++)
        {
            if (!players[i].HasName())
            {
                active = false;
                break;
            }
        }

        startGameButton.interactable = active;
    }

    private void StartGame()
    {
        var manager = FindObjectOfType<MovesManager>();

        for (int i = 0, length = players.Count; i < length; i++)
        {
            var player = players[i].CreatePlayer();
            manager.AddPlayer(player);
        }

        manager.Init();

        gameObject.SetActive(false);
    }
}
