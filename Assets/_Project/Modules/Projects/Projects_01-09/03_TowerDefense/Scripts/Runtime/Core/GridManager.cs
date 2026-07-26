using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Project.TowerDefense.Runtime.Core
{
    public struct Node
    {
        public Vector2Int Coordinates;
        public bool IsPlaceable;
        public GameObject Occupant;

        public Node(Vector2Int coordinates, bool isPlaceable, GameObject occupant)
        {
            Coordinates = coordinates;
            IsPlaceable = isPlaceable;
            Occupant = occupant;
        }
    }

    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;

        [Header("Refs")]
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private List<TileBase> _occupantTiles;

        private Dictionary<Vector2Int, Node> grid = new();

        void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
            
        }

        void Start()
        {
            InitializeGrid();
        }

        private void InitializeGrid()
        {
            // Отримуємо межі твого Tilemap, де є хоч якісь малюнки
            BoundsInt bounds = _tileMap.cellBounds;

            for (int x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (int y = bounds.yMin; y < bounds.yMax; y++)
                {
                    Vector3Int cellCoords3D = new Vector3Int(x, y, 0);

                    // Перевіряємо, чи є взагалі тайл у цій клітинці
                    if (_tileMap.HasTile(cellCoords3D))
                    {
                        TileBase currentTile = _tileMap.GetTile(cellCoords3D);
                        Vector2Int cellCoords2D = new Vector2Int(x, y);

                        // Якщо цей тайл є у списку доріг — будувати тут заборонено
                        bool isPlaceable = !_occupantTiles.Contains(currentTile);

                        // Заносимо клітинку в базу даних
                        grid[cellCoords2D] = new Node(cellCoords2D, isPlaceable, null);
                    }
                }
            }
            Debug.Log($"Сітку успішно ініціалізовано! Проскановано клітинок: {grid.Count}");
        }

        // ПЕРЕВАГА: Тепер метод перевіряє конкретну точку bottomLeft, яку ми передамо з мишки
        public bool CanBuildArea(Vector2Int bottomLeft, Vector2Int size)
        {
            int width = size.x < 1 ? 1 : size.x;
            int height = size.y < 1 ? 1 : size.y;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int currentCell = bottomLeft + new Vector2Int(x, y);

                    if (!CanBuildAt(currentCell))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        // ПЕРЕВАГА: Займає вказану зону для КОНКРЕТНОЇ створеної вежі
        public void OccupyArea(Vector2Int bottomLeft, Vector2Int size, GameObject tower)
        {
            int width = size.x < 1 ? 1 : size.x;
            int height = size.y < 1 ? 1 : size.y;

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    Vector2Int currentCell = bottomLeft + new Vector2Int(x, y);
                    SetCellOccupant(currentCell, tower);
                }
            }
        }

        public void SetCellOccupant(Vector2Int coords, GameObject tower)
        {
            grid[coords] = new Node(coords, (tower == null), tower);
        }

        public bool CanBuildAt(Vector2Int coords)
        {
            // Тепер, якщо клітинка є в базі (а там тепер вся мапа), ми візьмемо її реальний статус
            if (grid.ContainsKey(coords))
            {
                return grid[coords].IsPlaceable;
            }

            // Якщо раптом клікнули взагалі за межами розмальованого Tilemap
            return true;
        }
    }
}
