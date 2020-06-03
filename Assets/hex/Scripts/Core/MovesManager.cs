using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

public class MovesManager : MonoBehaviour
{
    public event Action<Player> OnGameEnd;
    public event Action<Player, bool> OnMoveEnd;

    private int currentPlayerIndex = 0;
    private List<Player> players = new List<Player>();

    private Player currentPlayer = null;
    private Camera cam = null;

    private bool isGameStarted = false;
    private bool isFirstStage = true;
    private int lap = 0;

    [SerializeField] private int startAmount = 10;
    //[SerializeField] private Transform circle = null;
    [SerializeField] private NextStageButton nextStageButton = null;
    [SerializeField] private SliderWindow sliderWindow = null;
    [SerializeField] private GridSettingsWindow gridWindow = null;

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
        grid.Create(gridWindow.GridSize);
        grid.SetPlayers(players, startAmount);

        nextStageButton.Init(NextStage);
        nextStageButton.UpdateVisual(isFirstStage);

        OnMoveEnd?.Invoke(currentPlayer, isFirstStage);
        //Debug.Log("inited");
    }

    private void UpdateCurrentPlayer()
    {
        currentPlayer = players[currentPlayerIndex];
    }

    private void NextStage()
    {
        selectedTile = null;

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

        OnMoveEnd?.Invoke(currentPlayer, isFirstStage);
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

        if (currentPlayer.IsLooser())
        {
            //Debug.Log("пропуск " + currentPlayer.name);
            NextPlayer();
        }
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

            //if (selectedTile.Amount > 1) circle.position = selectedTile.transform.position;
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

                    if (selectedTile.Amount > clickedTile.Amount)
                    {
                        //100% win
                        enemy.RemoveTile(clickedTile);

                        int difference = selectedTile.Amount - clickedTile.Amount;
                        clickedTile.Amount = difference;

                        currentPlayer.AddTile(clickedTile);

                        selectedTile.Amount = 1;
                        selectedTile.UpdateVisual(currentPlayer);

                        selectedTile = clickedTile;
                    }
                }
                else
                {
                    //tile is empty
                    int newAmount = selectedTile.Amount - 1;

                    selectedTile.Amount = 1;
                    selectedTile.UpdateVisual(currentPlayer);

                    clickedTile.Amount = newAmount;
                    currentPlayer.AddTile(clickedTile);

                    selectedTile = clickedTile;

                    //circle.position = clickedTile.Amount > 1 ? clickedTile.transform.position : new Vector3(20, 20);
                }


                CheckPlayerWin();
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

        OnMoveEnd?.Invoke(currentPlayer, isFirstStage);
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

    private void CheckPlayerWin()
    {
        var winners = players.Where(p => !p.IsLooser()).ToArray();

        if (winners.Length == 1)
        {
            IsClickBlocked = true;
            OnGameEnd?.Invoke(winners[0]);
        }
    }

    private void OnGUI()
    {
        if (currentPlayer == null) return;

        var old = GUI.color;
        GUI.color = Color.white;

        var rect = new Rect(10, 10, 500, 30);

        GUI.Label(rect, currentPlayer.name);

        for (int i = 0; i < players.Count; i++)
        {
            GUI.color = players[i].color;
            rect.y += 30;
            string text = players[i].ToString();
            GUI.Label(rect, text);
        }

        GUI.color = Color.white;
        rect.y += 30;
        GUI.Toggle(rect, selectedTile != null, " - Has selected tile");

        GUI.color = old;
    }
}
