using Project.TowerDefense.Testing;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;
using Project.TowerDefense.Runtime.Core;
using System.Collections;
using Project.TowerDefense.Runtime.Definitions;

namespace Project.TowerDefense.Runtime.Placement
{
    public class PlacementManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private PlayerCurrencyManager _currencyManager;
        [SerializeField] private GameObject _NotEnoughMoneyToBuyTowerReaction;
        [SerializeField] private UI.GameUIManager _uiManager;
        [SerializeField] private GameObject _PlacementCancelHint;
        [SerializeField] private GameObject _gridVisual;

        private TowerData _currentTowerData; // Поточна вежа для побудови
        private TowerGhost _spawnedGhost;    // Поточний привид на екрані

        private TDInputActions _inputActions;
        private TDInputActions.PlayerActions _actions;
        private Camera _mainCamera;

        private Coroutine _NotEnoughMoneyToBuyTowerReactionCoroutine;

        void Awake()
        {
            _inputActions = new TDInputActions();
            _actions = _inputActions.Player;
            _mainCamera = Camera.main;
        }

        void OnEnable()
        {
            _actions.Enable();
            _actions.Click.performed += OnClick;
            _actions.CancelTowerPlacement.performed += CancelPlacement;
        }

        void OnDisable()
        {
            _actions.Disable();
            _actions.Click.performed -= OnClick;
            _actions.CancelTowerPlacement.performed -= CancelPlacement;
            CancelPlacement();
        }

        void Update()
        {
            if (_currentTowerData == null || _spawnedGhost == null) return;

            // Оновлюємо позицію привида кожен кадр за мишкою
            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3Int cellCoords = _tileMap.WorldToCell(mouseWorldPos);

            Vector3 snapPosition = GetSnapPosition(cellCoords, _currentTowerData.size);
            _spawnedGhost.transform.position = snapPosition;

            // Перевіряємо валідність для підсвітки (Гроші + Вільне місце)
            Vector2Int bottomLeftCell = new Vector2Int(cellCoords.x, cellCoords.y);
            bool canBuild = HasEnoughCurrency() && GridManager.Instance.CanBuildArea(bottomLeftCell, _currentTowerData.size);

            _spawnedGhost.SetValid(canBuild);
        }

        // Метод викликається з UI при натисканні на кнопку вежі
        public void SelectTower(TowerData towerData)
        {
            if (_spawnedGhost != null) Destroy(_spawnedGhost.gameObject);

            if (!_currencyManager.HasEnoughCurrency(towerData.cost, CurrencyType.Diamond))
            {
                //StartCoroutine(NotEnoughMoneyCoroutine());
                if (_NotEnoughMoneyToBuyTowerReactionCoroutine != null)
                {
                    StopCoroutine(_NotEnoughMoneyToBuyTowerReactionCoroutine);
                }

                _NotEnoughMoneyToBuyTowerReactionCoroutine = StartCoroutine(NotEnoughMoneyCoroutine());
                return;
            }

            _currentTowerData = towerData;
            GameObject ghostObj = Instantiate(towerData.ghostPrefab);
            _spawnedGhost = ghostObj.GetComponent<TowerGhost>();
            _gridVisual.SetActive(true);
            _spawnedGhost.SetRange(towerData.attackRange);
            _PlacementCancelHint.SetActive(true);

            _uiManager.OnTowerChoosen();
        }
        private IEnumerator NotEnoughMoneyCoroutine()
        {
            _NotEnoughMoneyToBuyTowerReaction.SetActive(true);
            yield return new WaitForSecondsRealtime(0.5f);
            _NotEnoughMoneyToBuyTowerReaction.SetActive(false);
            _NotEnoughMoneyToBuyTowerReactionCoroutine = null;
        }

        public void CancelPlacement(InputAction.CallbackContext context = new InputAction.CallbackContext())
        {
            _currentTowerData = null;
            if (_spawnedGhost != null)
            {
                Destroy(_spawnedGhost.gameObject);
                _spawnedGhost = null;
            }
            if (_PlacementCancelHint != null) _PlacementCancelHint.SetActive(false);
            if (_gridVisual != null) _gridVisual.SetActive(false);
        }

        private void OnClick(InputAction.CallbackContext context)
        {
            if (_currentTowerData == null) return;

            Vector3 mouseWorldPos = GetMouseWorldPosition();
            Vector3Int cellCoords = _tileMap.WorldToCell(mouseWorldPos);
            Vector2Int bottomLeftCell = new Vector2Int(cellCoords.x, cellCoords.y);

            // 1. Перевірка грошей
            if (!HasEnoughCurrency())
            {
                Debug.LogWarning("Недостатньо золота!");
                return;
            }

            // 2. Перевірка сітки
            if (GridManager.Instance.CanBuildArea(bottomLeftCell, _currentTowerData.size))
            {
                Vector3 spawnPosition = GetSnapPosition(cellCoords, _currentTowerData.size);

                GameObject newTower = Instantiate(_currentTowerData.prefab, spawnPosition, Quaternion.identity);
                GridManager.Instance.OccupyArea(bottomLeftCell, _currentTowerData.size, newTower);


                _currencyManager.SpendCurrency(_currentTowerData.cost, CurrencyType.Diamond);

                CancelPlacement(); 
            }
        }

        private Vector3 GetMouseWorldPosition()
        {
            Vector2 mouseScreenPos = _actions.ClickPos.ReadValue<Vector2>();
            Vector3 mouseWorldPos = _mainCamera.ScreenToWorldPoint(mouseScreenPos);
            mouseWorldPos.z = 0f;
            return mouseWorldPos;
        }

        // Розрахунок центру з урахуванням розміру вежі (наприклад 1х1 чи 2х2)
        private Vector3 GetSnapPosition(Vector3Int cellCoords, Vector2Int size)
        {
            Vector3 cellCenter = _tileMap.GetCellCenterWorld(cellCoords);

            // Якщо ширина або висота парна, потрібно змістити центр на половину клітинки
            float offsetX = (size.x % 2 == 0) ? _tileMap.cellSize.x / 2f : 0f;
            float offsetY = (size.y % 2 == 0) ? _tileMap.cellSize.y / 2f : 0f;

            return cellCenter + new Vector3(offsetX, offsetY, 0f);
        }

        private bool HasEnoughCurrency()
        {
            if (_currencyManager == null || _currentTowerData == null) return false;

            return _currencyManager.HasEnoughCurrency(_currentTowerData.cost, CurrencyType.Diamond);
        }
    }
}