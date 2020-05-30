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

    private void Start()
    {
        addPlayerButton.onClick.AddListener(AddPlayer);

        for (int i = 0; i < playersParent.childCount - 1; i++)
        {
            Destroy(playersParent.GetChild(i).gameObject);
        }

        AddPlayer();
    }

    private void AddPlayer()
    {
        var element = Instantiate(playerPrefab, playersParent);

        int index = playersParent.childCount - 2;
        element.transform.SetSiblingIndex(index);

        players.Add(element);
    }
    
    public void DeletePlayer(int index)
    {
        //Debug.Log(index);

        var element = players[index];
        players.Remove(element);

        Destroy(element.gameObject);
    }
}
