// UnityEngine
using UnityEngine;

namespace Common.Scripts.Definitions.States
{
    [CreateAssetMenu(fileName = "StatesProvider", menuName = "Scriptable Objects/Providers/StatesProvider")]
    public class StatesProvider : ScriptableObject
    {
        [Header("References")]
        [SerializeField] private PlayerState playerState;

        // Properties
        public PlayerState PlayerState => playerState;
    }
}