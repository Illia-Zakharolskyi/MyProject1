using UnityEngine;

namespace Project.TowerDefense.Runtime.Definitions
{
    [CreateAssetMenu(fileName = "Tower_Data_Testing", menuName = "SO/Projects/TowerDefense/Tower_Data")]
    public class TowerData : ScriptableObject
    {
        public string towerName;
        public GameObject prefab;
        public GameObject ghostPrefab; // Спрайт вежі з напівпрозорістю
        public double cost;
        public Vector2Int size = new Vector2Int(1, 1);
        public float attackRange;
    }
}
