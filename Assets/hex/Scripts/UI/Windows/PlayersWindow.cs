using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayersWindow : MonoBehaviour
{
    [SerializeField] private PlayerElement playerPrefab = null;
    [SerializeField] private Transform playersParent = null;
    [SerializeField] private Button addPlayerButton = null;

    private List<PlayerElement> players = new List<PlayerElement>();

    public List<PlayerElement> Players => players;

    private int minPlayersCount = 2;

    private void Start()
    {
        addPlayerButton.onClick.AddListener(AddPlayer);

        PopulateWindow();
    }

    private void PopulateWindow()
    {
        for (int i = 0; i < playersParent.childCount; i++)
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
        int i = players.Count;

        PlayerElement element = Instantiate(playerPrefab, playersParent);
        element.Init(i);
        element.transform.SetSiblingIndex(i);
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

        //ValidateNames();
    }

    //public void ValidateNames()
    //{
    //    bool active = true;

    //    for (int i = 0, length = players.Count; i < length; i++)
    //    {
    //        if (!players[i].HasName())
    //        {
    //            active = false;
    //            break;
    //        }
    //    }

    //    startGameButton.interactable = active;
    //}
}
