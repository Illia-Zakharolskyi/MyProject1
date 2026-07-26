using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Task.Raycast.Second
{
    public enum TargetType
    {
        Outer,
        Center
    }

    public class TargetPart : MonoBehaviour, IInteractable
    {
        [Header("Settings")]
        [SerializeField ] private  TargetType _type;

        private EventBus _bus;

        public void Initialize(EventBus bus)
        {
            _bus = bus;
        }

        public void Interact(RaycastHit hitInfo)
        {
            if (_bus != null) _bus.InvokeTargetPartHit(_type, hitInfo);
        }
    }
}