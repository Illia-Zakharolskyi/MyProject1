using System;
using UnityEngine;

namespace Task.Raycast.Second
{
    [CreateAssetMenu(fileName = "Event_Bus", menuName = "SO/Tasks/Raycast/Second/Event_Bus")]
    public class EventBus : ScriptableObject
    {
        public event Action OnTargetHit;
        public event Action<TargetType, RaycastHit> OnTargetPartHit;

        public void InvokeTargetHit()
        {
            OnTargetHit?.Invoke();
        }

        public void InvokeTargetPartHit(TargetType type, RaycastHit hitInfo)
        {
            OnTargetPartHit?.Invoke(type, hitInfo);
        }
    }
}