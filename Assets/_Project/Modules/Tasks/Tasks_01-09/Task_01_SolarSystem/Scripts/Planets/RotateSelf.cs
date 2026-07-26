using UnityEngine;

public class MoveSelf : MonoBehaviour
{
    public float RotationSpeed = 10f;

    void FixedUpdate()
    {
        transform.Rotate(Vector3.up * RotationSpeed * Time.deltaTime);
    }
}
