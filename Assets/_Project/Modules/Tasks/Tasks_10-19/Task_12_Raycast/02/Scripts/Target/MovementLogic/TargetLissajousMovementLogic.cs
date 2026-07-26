using UnityEngine;

namespace Task.Raycast.Second
{
    public class TargetLissajousMovementLogic : ITargetMovementLogic
    {
        public Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight)
        {
            float angleX = Time.time * speed;
            float angleY = Time.time * speed * 2.5f;

            float microJitter = Mathf.Sin(Time.time * 10f) * 0.1f;

            float newX = startLocalPosition.x + Mathf.Sin(angleX + microJitter) * amplitude;
            float newY = startLocalPosition.y + Mathf.Cos(angleY) * jumpHeight;

            return new Vector3(newX, newY, startLocalPosition.z);
        }
    }
}
