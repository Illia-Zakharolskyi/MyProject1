using UnityEngine;

namespace Task.Raycast.Second
{
    public enum TargetLogic
    {
        Linear,
        Arc,
        PerlinChaos,
        Lissajous,
        Dash
    }

    public class TargetLinearMovementLogic : ITargetMovementLogic
    {
        public Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight)
        {
            float totalPath = amplitude * 2f;
            float currentProgress = Mathf.PingPong(Time.time * speed, totalPath);

            float newX = startLocalPosition.x - amplitude + currentProgress;

            return new Vector3(newX, startLocalPosition.y, startLocalPosition.z);
        }
    }
}
