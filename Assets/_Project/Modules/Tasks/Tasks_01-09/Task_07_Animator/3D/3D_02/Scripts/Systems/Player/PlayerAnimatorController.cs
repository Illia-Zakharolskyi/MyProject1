// UnityEngine using directives
using UnityEngine;
using UnityAnimator = UnityEngine.Animator;

// Common using directives
using Common.Scripts.Definitions.Events;
using Common.Scripts.Definitions.States;

// Project using directives
using Tasks.Learning.Animator.ThreeD.S2.Systems.Player.Combat;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Player
{
    public class PlayerAnimatorController : MonoBehaviour
    {
        [Header("References Settings")]
        [SerializeField] private UnityAnimator animator;
        [SerializeField] private EventsProvider eventsProvider;
        [SerializeField] private PlayerCombatEvents combatEvents;
        [SerializeField] private StatesProvider statesProvider;
        [SerializeField] private Rigidbody playerRb;

        // Hash-based animation parameters
        private readonly int inputXHash = UnityAnimator.StringToHash("InputX");
        private readonly int inputZHash = UnityAnimator.StringToHash("InputZ");
        private readonly int verticalVelocityHash = UnityAnimator.StringToHash("VerticalVelocity");
        private readonly int isGroundHash = UnityAnimator.StringToHash("IsGround");

        // private fields
        private PlayerMovementEvents movementEvents;
        private PlayerState playerState;

        // Lifecycle Methods
        private void Awake()
        {
            if (animator == null || eventsProvider == null || combatEvents == null || statesProvider == null || playerRb == null)
            {
                Debug.LogError($"One or more required references are missing on the {this.name}");
            }

            playerState = statesProvider.PlayerState;
            movementEvents = eventsProvider.PlayerMovementEvents;
        }
        private void OnEnable()
        {
            movementEvents.OnMove += HandleMove;
            movementEvents.OnJump += HandleJump;
            combatEvents.OnAttack += HandleFire;
        }

        private void Update()
        {
            if (playerState == null)
            {
                Debug.LogError("PlayerState reference is missing. Please assign it in the inspector.");
                return;
            }
            
            animator.SetBool(isGroundHash, playerState.IsGrounded);
            animator.SetFloat(verticalVelocityHash, playerRb.linearVelocity.y);
        }

        private void OnDisable()
        {
            movementEvents.OnMove -= HandleMove;
            movementEvents.OnJump -= HandleJump;
            combatEvents.OnAttack -= HandleFire;
        }

        // Functional Methods
        private void HandleMove(Vector3 movement)
        {
            animator.SetFloat(inputXHash, movement.x);
            animator.SetFloat(inputZHash, movement.z);
        }

        private void HandleJump()
        {
            if (playerState.IsGrounded)
            {
                animator.SetTrigger("Jump");
            }
        }

        private void HandleFire(int type)
        {
            animator.SetInteger("AttackType", type);
            animator.SetTrigger("Attack");
        }
    }
}