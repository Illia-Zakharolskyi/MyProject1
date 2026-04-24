using UnityEngine;

public class AsteroidBeltSpawner : MonoBehaviour
{
    public GameObject AsteroidPrefab; 
    public int AsteroidCount = 200; 
    public float InnerRadius = 60f; 
    public float OuterRadius = 90f;

    void Start()
    {
        for (int i = 0; i < AsteroidCount; i++)
        {
            float angle = Random.Range(0f, 360f);
            float radius = Random.Range(InnerRadius, OuterRadius);

            float x = Mathf.Cos(angle * Mathf.Deg2Rad) * radius;
            float z = Mathf.Sin(angle * Mathf.Deg2Rad) * radius;

            Vector3 pos = new Vector3(x, 0, z);

            Instantiate(AsteroidPrefab, pos, Random.rotation);
        }
    }
}
