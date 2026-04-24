// UnityEngine using directives
using UnityEngine;

// Task using directives
using Tasks.Learning.Animator.ThreeD.S2.Generated;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Player.Combat
{
    public class Task07_InputReader : MonoBehaviour
    {
        [Header("References Settings")]
        [SerializeField] private PlayerCombatEvents combatEvents;

        private InputActions.PlayerActions playerCombatActions;

        private void Awake()
        {
            playerCombatActions = new InputActions().Player;
        }

        private void OnEnable()
        {
            playerCombatActions.Enable();
        }
        private void OnDisable()
        {
            playerCombatActions.Disable();
        }

        private void Update()
        {
            if (playerCombatActions.Fire1.WasPressedThisFrame())
            {
                combatEvents.RaiseAttack(1);
            }
            else if (playerCombatActions.Fire2.WasPressedThisFrame())
            {
                combatEvents.RaiseAttack(2);
            }
        }
    }
}
