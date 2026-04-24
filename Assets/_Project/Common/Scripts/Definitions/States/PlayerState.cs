// UnityEngine using directives
using UnityEngine;

namespace Common.Scripts.Definitions.States
{
    [CreateAssetMenu(fileName = "PlayerState", menuName = "Scriptable Objects/States/PlayerState")]
    public class PlayerState : ScriptableObject
    {
        public bool IsGrounded;
    }
}