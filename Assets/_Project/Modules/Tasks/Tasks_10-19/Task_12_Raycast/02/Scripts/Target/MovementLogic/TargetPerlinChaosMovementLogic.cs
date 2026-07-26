using UnityEngine;

namespace Task.Raycast.Second
{
    public class TargetPerlinChaosMovementLogic : ITargetMovementLogic
    {
        private float _randomSeedX = Random.Range(0f, 100f);
        private float _randomSeedY = Random.Range(100f, 200f);

        public Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight)
        {
            float timeX = Time.time * speed + _randomSeedX;
            float timeY = Time.time * speed + _randomSeedY;

            float noiseX = (Mathf.PerlinNoise(timeX, 0f) - 0.5f) * 2f;
            float noiseY = (Mathf.PerlinNoise(0f, timeY) - 0.5f) * 2f;

            float newX = startLocalPosition.x + noiseX * amplitude;
            float newY = startLocalPosition.y + noiseY * jumpHeight;

            return new Vector3(newX, newY, startLocalPosition.z);
        }
    }
}
