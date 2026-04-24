// System using directives
using System;

// UnityEngine using directives
using UnityEngine;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Player.Combat
{
    [CreateAssetMenu(fileName = "PlayerCombatEvents_SO", menuName = "SO/Tasks/Animator/3D/S2/Events/PlayerCombatEvents")]
    public class PlayerCombatEvents : ScriptableObject
    {
        public event Action<int> OnAttack;

        public void RaiseAttack(int type)
        {
            OnAttack?.Invoke(type);
        }
    }
}