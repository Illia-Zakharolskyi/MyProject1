using UnityEngine;

namespace Task.Raycast.Second
{
    public class TargetDashMovementLogic : ITargetMovementLogic
    {

        public Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight)
        {
            float cycle = Mathf.Repeat(Time.time * speed, Mathf.PI * 2);

            float dashPattern = Mathf.Sin(cycle);
            if (dashPattern > 0)
                dashPattern = Mathf.Pow(dashPattern, 4f);
            else
                dashPattern = -Mathf.Pow(Mathf.Abs(dashPattern), 4f);

            float newX = startLocalPosition.x + dashPattern * amplitude;

            float newY = startLocalPosition.y + (Mathf.Abs(dashPattern) * jumpHeight * 0.5f);

            return new Vector3(newX, newY, startLocalPosition.z);
        }
    }
}
