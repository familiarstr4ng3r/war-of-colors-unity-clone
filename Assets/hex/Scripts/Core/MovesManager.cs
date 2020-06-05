using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;

namespace WOC
{
    public class MovesManager : MonoBehaviour
    {
        public event Action<Player> OnGameEnd;
        public event Action<Player, bool, List<Player>, GridCreator> OnMoveEnd;

        //[SerializeField] private Transform circle = null;
        private SliderWindow sliderWindow = null;

        private int currentPlayerIndex = 0;
        private Player currentPlayer = null;
        private List<Player> players = new List<Player>();

        private bool isGameStarted = false;
        private bool isFirstStage = true;
        private int lap = 0;

        private HexTile selectedTile = null;

        public static bool IsClickBlocked = false;

        private GridCreator grid = null;
        private CameraController cameraController = null;

        private void Awake()
        {
            cameraController = FindObjectOfType<CameraController>();
            sliderWindow = FindObjectOfType<SliderWindow>();
            grid = FindObjectOfType<GridCreator>();
        }

        private void Update()
        {
            HandleInput();
        }

        public void Init()
        {
            isGameStarted = true;
            IsClickBlocked = false;

            UpdateCurrentPlayer();

            grid.Create();
            grid.SetPlayers(players);

            OnMoveEnd?.Invoke(currentPlayer, isFirstStage, players, grid);
        }

        public void Load(SaveData data)
        {
            isGameStarted = true;
            IsClickBlocked = false;

            isFirstStage = data.IsFirstStage;
            currentPlayerIndex = data.CurrentPlayerIndex;

            players = new List<Player>(data.Players);
            UpdateCurrentPlayer();

            grid.Width = data.Width;
            grid.Height = data.Height;

            grid.Create();
            grid.Load(data);

            OnMoveEnd?.Invoke(currentPlayer, isFirstStage, players, grid);
        }

        private void UpdateCurrentPlayer()
        {
            currentPlayer = players[currentPlayerIndex];
        }

        public void NextStage()
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

            OnMoveEnd?.Invoke(currentPlayer, isFirstStage, players, grid);
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

                Vector3 worldPos = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);
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
                if (!selectedTile) return;

                if (selectedTile.Amount > 1 && selectedTile.HasNeighbour(clickedTile))
                {
                    bool isEnemyTile = !clickedTile.IsEmpty() && currentPlayer.Index != clickedTile.Data.PlayerIndex;

                    if (isEnemyTile)
                    {
                        var enemy = players[clickedTile.Data.PlayerIndex];

                        if (selectedTile.Amount > clickedTile.Amount)
                        {
                            int difference = selectedTile.Amount - clickedTile.Amount;
                            clickedTile.Player = currentPlayer;
                            clickedTile.Amount = difference;

                            selectedTile.Amount = 1;

                            currentPlayer.TilesCount++;
                            enemy.TilesCount--;

                            selectedTile = clickedTile;
                        }
                        else if (selectedTile.Amount == clickedTile.Amount)
                        {
                            bool attackerWin = UnityEngine.Random.Range(0, 2) == 0;

                            if (attackerWin)
                            {
                                enemy.TilesCount--;

                                //selectedTile.Player = null;
                                selectedTile.Amount = 1;

                                clickedTile.Player = currentPlayer;
                                clickedTile.Amount = 1;

                                currentPlayer.TilesCount++;
                            }
                            else
                            {
                                enemy.TilesCount++;

                                selectedTile.Player = enemy;
                                selectedTile.Amount = 1;

                                //clickedTile.Player = currentPlayer;
                                clickedTile.Amount = 1;

                                currentPlayer.TilesCount--;
                            }

                            selectedTile = null;
                        }
                        else if (selectedTile.Amount < clickedTile.Amount)
                        {
                            int difference = Mathf.Abs(selectedTile.Amount - clickedTile.Amount);

                            selectedTile.Amount = 0;
                            selectedTile.Player = null;

                            clickedTile.Amount = difference;

                            selectedTile = null;

                            currentPlayer.TilesCount--;
                        }
                    }
                    else
                    {
                        int newAmount = selectedTile.Amount - 1;

                        clickedTile.Player = currentPlayer;
                        clickedTile.Amount = newAmount;

                        selectedTile.Amount = 1;
                        selectedTile = clickedTile;
                        currentPlayer.TilesCount++;

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
            }
        }

        public void OnAddCLick(int amount)
        {
            currentPlayer.AvailableAmount -= amount;
            selectedTile.Amount += amount;

            sliderWindow.Deactivate();
            OnMoveEnd?.Invoke(currentPlayer, isFirstStage, players, grid);
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

        //

        private void OnGUI()
        {
            if (currentPlayer == null) return;

            var old = GUI.color;
            GUI.color = Color.white;

            var rect = new Rect(10, 10, 500, 30);

            GUI.Label(rect, currentPlayer.Name);

            for (int i = 0; i < players.Count; i++)
            {
                rect.y += 30;

                //GUI.color = players[i].Color;
                //GUI.Label(rect, "III");
                //GUI.color = Color.white;

                string text = players[i].ToString();
                GUI.Label(rect, text);
            }

            GUI.color = Color.white;
            rect.y += 30;
            GUI.Toggle(rect, selectedTile != null, " - Has selected tile");

            GUI.color = old;
        }
    }
}
