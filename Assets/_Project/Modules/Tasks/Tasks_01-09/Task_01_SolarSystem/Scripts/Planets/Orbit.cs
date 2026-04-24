using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float OrbitSpeed = 10f;

    void Update()
    {
        transform.Rotate(Vector3.up * OrbitSpeed * Time.deltaTime);
    }
}
