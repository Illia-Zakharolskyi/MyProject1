using UnityEngine;

namespace Task_02_PrefabLevel
{
    public class LetterLGenerator : MonoBehaviour
    {
        public GameObject[] CubePrefabs;
        public int Height = 5;
        public int Width = 3;
        public float Offset = 1.0f;

        void Start()
        {
            GenerateL();
        }
        private void GenerateL()
        {
            for (int i = 0; i < Height; i++)
            {
                Vector3 position = new Vector3(0, i * Offset, 0);
                SpawnCube(position);
            }

            for (int i = 1; i < Width; i++)
            {
                Vector3 position = new Vector3(i * Offset, 0, 0);
                SpawnCube(position);
            }
        }
        void SpawnCube(Vector3 position)
        {
            GameObject randomPrefab = CubePrefabs[Random.Range(0, CubePrefabs.Length)];
            GameObject cube = Instantiate(randomPrefab, transform.position + position, Quaternion.identity);

            cube.transform.parent = transform;
        }
    }
}