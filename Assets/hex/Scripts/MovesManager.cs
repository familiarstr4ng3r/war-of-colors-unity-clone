using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();

    private Player currentPlayer = null;
    private Camera cam = null;

    private bool isGameStarted = false;
    [SerializeField] private bool isFirstStage = true;
    [SerializeField] private Transform circle = null;
    private HexTile selectedTile = null;

    private void Update()
    {
        HandleInput();
    }

    public void Init()
    {
        cam = Camera.main;
        isGameStarted = true;

        UpdateCurrentPlayer();

        //to do: create grid
        var grid = FindObjectOfType<GridCreator>();
        grid.Create();
        grid.SetPlayers(players);
    }

    private void UpdateCurrentPlayer()
    {
        currentPlayer = players[currentPlayerIndex];
    }

    public void NextMove()
    {
        if (currentPlayerIndex == players.Count - 1)
            currentPlayerIndex = 0;
        else currentPlayerIndex++;

        UpdateCurrentPlayer();
    }

    public void AddPlayer(Player p)
    {
        players.Add(p);
    }

    private void HandleInput()
    {
        if (Input.GetKeyUp(KeyCode.Mouse0) && isGameStarted)
        {
            Vector3 worldPos = cam.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward);

            if (hit.collider)
            {
                var clickedTile = hit.collider.GetComponent<HexTile>();

                if (isFirstStage)
                    HandleMoving(clickedTile);
                else
                    HandleAdding(clickedTile);
            }
        }
    }

    private void HandleMoving(HexTile clickedTile)
    {
        if (!selectedTile)
        {
            if (currentPlayer.HasTile(clickedTile))
            {
                selectedTile = clickedTile;

                circle.position = selectedTile.transform.position;
            }
        }
        else
        {
            if (selectedTile.Amount > 1 && selectedTile.HasNeighbour(clickedTile))
            {
                //to do: check for enemy
                selectedTile.Amount -= 1;
                selectedTile.UpdateVisual(currentPlayer);

                clickedTile.Amount += 1;
                currentPlayer.AddTile(clickedTile);

                selectedTile = null;
            }

        }
    }

    private void HandleAdding(HexTile clickedTile)
    {

    }
}
