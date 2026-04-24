// UnityEngine
using UnityEngine;

namespace Common.Scripts.Definitions.Events
{
    [CreateAssetMenu(fileName = "EventsProvider", menuName = "Scriptable Objects/Providers/EventsProvider")]
    public class EventsProvider : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private PlayerMovementEvents playerMovementEvents;

        // Properties
        public PlayerMovementEvents PlayerMovementEvents => playerMovementEvents;
    }
}