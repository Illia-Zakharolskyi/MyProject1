// System using directives
using System;

// UnityEngine using directives
using UnityEngine;

namespace Common.Scripts.Definitions.Events
{
    [CreateAssetMenu(fileName = "PlayerMovementEvents", menuName = "Scriptable Objects/Events/PlayerMovementEvents")]
    public class PlayerMovementEvents : ScriptableObject
    {
        public event Action OnJump;
        public event Action<Vector3> OnMove;

        public void RaiseJump()
        {
            OnJump?.Invoke();
        }

        public void RaiseMove(Vector3 movement)
        {
            OnMove?.Invoke(movement);
        }
    }
}