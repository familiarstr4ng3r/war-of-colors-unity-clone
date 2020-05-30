using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();

    //private Player currentPlayer = null;

    private void Start()
    {
        //currentPlayer = players[currentPlayerIndex];
    }

    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space)) NextMove();
    }

    public void NextMove()
    {
        if (currentPlayerIndex == players.Count - 1)
        {
            currentPlayerIndex = 0;
        }
        else currentPlayerIndex++;

        //currentPlayer = players[currentPlayerIndex];
    }

    public void AddPlayer(Player p)
    {
        players.Add(p);
    }
}
