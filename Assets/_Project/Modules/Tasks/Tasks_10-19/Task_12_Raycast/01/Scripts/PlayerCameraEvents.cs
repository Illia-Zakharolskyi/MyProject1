using System;
using UnityEngine;

namespace Task.Raycast
{
    [CreateAssetMenu(fileName = "Player_Camera_Events", menuName = "SO/Tasks/Raycast/Player_Camera_Events")]
    public class PlayerCameraEvents : ScriptableObject
    {
        public event Action<RaycastHit> OnRaycastHit;

        public void InvokeRaycastHit(RaycastHit hit)
        {
            OnRaycastHit?.Invoke(hit);
        }
    }
}