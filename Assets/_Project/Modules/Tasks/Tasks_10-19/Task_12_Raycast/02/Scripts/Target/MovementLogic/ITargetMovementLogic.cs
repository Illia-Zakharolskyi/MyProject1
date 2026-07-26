using UnityEngine;

namespace Task.Raycast.Second
{
    public interface ITargetMovementLogic
    {
        Vector3 Calculate(Vector3 startLocalPosition, float amplitude, float speed, float jumpHeight);
    }
}
