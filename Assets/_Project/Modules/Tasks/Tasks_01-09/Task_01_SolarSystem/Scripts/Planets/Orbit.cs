using UnityEngine;

public class Orbit : MonoBehaviour
{
    public float OrbitSpeed = 10f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * OrbitSpeed * Time.deltaTime);
    }
}
