using UnityEngine;

namespace Task.Raycast.Second
{
    public class TargetArcMovementLogic : ITargetMovementLogic
    {
        public Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight)
        {
            float totalPath = amplitude * 2f;
            float currentProgress = Mathf.PingPong(Time.time * speed, totalPath);

            float newX = startLocalPosition.x - amplitude + currentProgress;
            float newY = startLocalPosition.y + Mathf.Sin((currentProgress / totalPath) * Mathf.PI) * jumpHeight;

            return new Vector3(newX, newY, startLocalPosition.z);
        }
    }
}
