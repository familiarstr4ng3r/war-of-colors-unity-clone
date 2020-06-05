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

        [SerializeField] private ClickPointer pointer = null;

        private int currentPlayerIndex = 0;
        private Player currentPlayer = null;
        private List<Player> players = new List<Player>();

        private bool isGameStarted = false;
        private bool isFirstStage = true;
        private int lap = 0;

        private float dragTimer = 0;
        [SerializeField] private float dragTimeThreshold = 1;//задержка удержания в секундах
        private float dragDistanceThreshold = 25;//задержка дистанции в пикселях (играет маленькую роль)

        private bool isDragging = false;
        private Vector3 clickedPos = Vector3.zero;
        private Vector3 dragPos = Vector3.zero;

        private HexTile selectedTile = null;
        private HexTile clickedTile = null;

        private GridCreator grid = null;
        private CameraController cameraController = null;
        private SliderWindow sliderWindow = null;

        public static bool IsClickBlocked = false;

        private void Awake()
        {
            cameraController = FindObjectOfType<CameraController>();
            sliderWindow = FindObjectOfType<SliderWindow>();
            grid = FindObjectOfType<GridCreator>();

            pointer.Deactivate();
        }

        private void Update()
        {
            if (isGameStarted)
            {
                HandleInput();

                if (isDragging)
                {
                    dragTimer += Time.deltaTime;
                    dragPos = Input.mousePosition;

                    HandleDragging();
                }
            }
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

            pointer.Deactivate();
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
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                isDragging = true;
                clickedPos = Input.mousePosition;
            }
            else if (Input.GetKeyUp(KeyCode.Mouse0))
            {
                if (!IsClickBlocked)
                {
                    Vector3 worldPos = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);
                    RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward);

                    if (hit.collider)
                    {
                        var clickedTile = hit.collider.GetComponent<HexTile>();

                        if (isFirstStage)
                        {
                            if (currentPlayer.HasTile(clickedTile))
                            {
                                if (clickedTile.Amount > 1)
                                {
                                    selectedTile = clickedTile;
                                    pointer.Activate(selectedTile.transform.position);
                                }
                            }
                            else 
                            {
                                if (selectedTile && selectedTile.Amount > 1 && selectedTile.HasNeighbour(clickedTile))
                                    HandleMovingToNewTile(clickedTile, selectedTile.Amount);
                            }
                        }
                        else
                        {
                            HandleAdding(clickedTile);
                        }
                    }
                }

                isDragging = false;
                dragTimer = 0;
            }
        }

        private void HandleDragging()
        {
            if (dragTimer > dragTimeThreshold && Vector3.Distance(clickedPos, dragPos) < dragDistanceThreshold)
            {
                Vector3 worldPos = cameraController.Camera.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(worldPos, Vector3.forward);

                if (hit.collider)
                {
                    clickedTile = hit.collider.GetComponent<HexTile>();

                    bool isEnemyTile = !clickedTile.IsEmpty() && currentPlayer.Index != clickedTile.Data.PlayerIndex;

                    if (isEnemyTile && selectedTile && 
                        selectedTile.Amount > 1 && selectedTile.HasNeighbour(clickedTile))
                    {
                        //открыть окно слайдера с максимальным значением selectedTile.amount
                        //выбрать в слайдере кол-во и напасть на clickedTile с выбранным количеством

                        Debug.Log("new feauture");
                        sliderWindow.Activate(selectedTile.Amount, true);
                    }
                }

                isDragging = false;
                dragTimer = 0;
            }
        }

        private void HandleMovingToNewTile(HexTile clickedTile, int attackAmount)
        {
            bool isEnemyTile = !clickedTile.IsEmpty() && 
                currentPlayer.Index != clickedTile.Data.PlayerIndex;

            bool isFull = attackAmount == selectedTile.Amount;

            if (isFull)
            {
                if (isEnemyTile)
                {
                    var enemy = players[clickedTile.Data.PlayerIndex];

                    if (attackAmount > clickedTile.Amount)
                    {
                        int difference = attackAmount - clickedTile.Amount;
                        clickedTile.Player = currentPlayer;
                        clickedTile.Amount = difference;

                        selectedTile.Amount = 1;

                        currentPlayer.TilesCount++;
                        enemy.TilesCount--;

                        selectedTile = clickedTile;

                        pointer.Activate(selectedTile.transform.position);
                    }
                    else if (attackAmount == clickedTile.Amount)
                    {
                        bool attackerWin = UnityEngine.Random.Range(0, 2) == 0;

                        if (attackerWin)
                        {
                            enemy.TilesCount--;

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

                            clickedTile.Amount = 1;

                            currentPlayer.TilesCount--;
                        }

                        selectedTile = null;

                        pointer.Deactivate();
                    }
                    else if (attackAmount < clickedTile.Amount)
                    {
                        int difference = Mathf.Abs(attackAmount - clickedTile.Amount);

                        selectedTile.Amount = 0;
                        selectedTile.Player = null;

                        clickedTile.Amount = difference;

                        selectedTile = null;

                        currentPlayer.TilesCount--;

                        pointer.Deactivate();
                    }
                }
                else
                {
                    int newAmount = attackAmount - 1;

                    clickedTile.Player = currentPlayer;
                    clickedTile.Amount = newAmount;

                    selectedTile.Amount = 1;
                    selectedTile = clickedTile;
                    currentPlayer.TilesCount++;

                    if (clickedTile.Amount > 1)
                    {
                        pointer.Activate(selectedTile.transform.position);
                    }
                    else
                    {
                        pointer.Deactivate();
                    }
                    //circle.position = clickedTile.Amount > 1 ? clickedTile.transform.position : new Vector3(20, 20);
                }
            }
            else
            {
                if (attackAmount >= clickedTile.Amount)
                {
                    clickedTile.Player = currentPlayer;

                    int difference = Math.Max(1, attackAmount - clickedTile.Amount);
                    clickedTile.Amount = difference;
                }
                else if (attackAmount < clickedTile.Amount)
                {
                    clickedTile.Amount -= attackAmount;
                }

                selectedTile.Amount -= attackAmount;
            }

            CheckPlayerWin();
        }

        private void HandleAdding(HexTile clickedTile)
        {
            if (currentPlayer.AvailableAmount > 0 && currentPlayer.HasTile(clickedTile))
            {
                selectedTile = clickedTile;
                sliderWindow.Activate(currentPlayer.AvailableAmount, false);
            }
        }

        public void OnAddCLick(int amount)
        {
            if (isFirstStage)
            {
                HandleMovingToNewTile(clickedTile, amount);
                clickedTile = null;
            }
            else
            {
                currentPlayer.AvailableAmount -= amount;
                selectedTile.Amount += amount;
            }

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

#if UNITY_EDITOR

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

#endif
    }
}
