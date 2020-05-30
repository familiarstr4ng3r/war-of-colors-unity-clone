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
    private bool isFirstStage = true;
    private int lap = 0;

    [SerializeField] private int startAmount = 10;
    [SerializeField] private Transform circle = null;
    [SerializeField] private NextStageButton nextStageButton = null;
    [SerializeField] private SliderWindow sliderWindow = null;

    private HexTile selectedTile = null;

    public static bool IsClickBlocked = false;

    private void Update()
    {
        HandleInput();
    }

    public void Init()
    {
        cam = Camera.main;
        isGameStarted = true;
        IsClickBlocked = false;

        UpdateCurrentPlayer();

        var grid = FindObjectOfType<GridCreator>();
        grid.Create();
        grid.SetPlayers(players, startAmount);

        nextStageButton.Init(NextStage);
        nextStageButton.UpdateVisual(isFirstStage);
    }

    private void UpdateCurrentPlayer()
    {
        currentPlayer = players[currentPlayerIndex];
    }

    private void NextStage()
    {
        if (isFirstStage)
        {
            currentPlayer.OnSecondStage();
        }
        else
        {
            NextPlayer();
        }

        isFirstStage = !isFirstStage;

        nextStageButton.UpdateVisual(isFirstStage);
    }

    private void NextPlayer()
    {
        if (currentPlayerIndex == players.Count - 1)
        {
            currentPlayerIndex = 0;
            lap++;
        }
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
            if (IsClickBlocked) return;

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
        if (currentPlayer.HasTile(clickedTile))
        {
            selectedTile = clickedTile;

            if (selectedTile.Amount > 1) circle.position = selectedTile.transform.position;
        }
        else
        {
            if (selectedTile && selectedTile.Amount > 1 && selectedTile.HasNeighbour(clickedTile))
            {
                //to do: check for enemy

                //clicked is new
                var enemy = IsPlayerTile(clickedTile);
                
                if (enemy != null)
                {
                    //tile is enemy
                }
                else
                {
                    //tile is empty
                    int newAmount = selectedTile.Amount - 1;

                    selectedTile.Amount = 1;
                    selectedTile.UpdateVisual(currentPlayer);

                    clickedTile.Amount = newAmount;
                    currentPlayer.AddTile(clickedTile);
                }

                //selectedTile = null;
                circle.position = clickedTile.Amount > 1 ? clickedTile.transform.position : new Vector3(20, 20);
            }
        }
    }

    private void HandleAdding(HexTile clickedTile)
    {
        if (currentPlayer.AvailableAmount > 0 && currentPlayer.HasTile(clickedTile))
        {
            selectedTile = clickedTile;
            sliderWindow.Activate(currentPlayer.AvailableAmount);
            IsClickBlocked = true;
        }
    }

    public void OnAddCLick(int amount)
    {
        currentPlayer.AvailableAmount -= amount;
        selectedTile.Amount += amount;

        selectedTile.UpdateVisual(currentPlayer);

        sliderWindow.Deactivate();
        IsClickBlocked = false;
    }

    private Player IsPlayerTile(HexTile tile)
    {
        Player player = null;

        for (int i = 0, length = players.Count; i < length; i++)
        {
            if (players[i].HasTile(tile))
            {
                player = players[i];
                break;
            }
        }

        return player;
    }
}
