// UnityEngine using directives
using UnityEngine;
using UnityEngine.InputSystem;

// Project Common using directives
using Common.Scripts.Definitions.Events;
using Common.Scripts.Definitions.InputActions;
using Common.Scripts.Definitions.States;


namespace Common.Scripts.Systems.Player
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Player Settings")]
        [SerializeField] private float forwardSpeed = 5f;
        [SerializeField] private float backwardSpeed = 3f;
        [SerializeField] private float strafeSpeed = 4f;
        [SerializeField] private float jumpForce = 6f;

        [Header("References Settings")]
        [SerializeField] private StatesProvider statesProvider;
        [SerializeField] private EventsProvider eventsProvider;
        [SerializeField] private InputActionsProvider inputActionsProvider;

        [Header("SphereCast Settings")]
        [SerializeField] private float sphereCastRadius = 0.5f;
        [SerializeField] private float sphereCastDistance = 0.3f;

        [Header("Layers Settings")]
        [SerializeField] private LayerMask groundLayer;

        // private physics fields
        private Rigidbody rb;

        // private fields
        private PlayerState playerState;
        private PlayerMovementEvents playerMovementEvents;
        private BaseControls.PlayerActions playerInputActions;

        // Lifecycle Methods
        private void Awake()
        {
            if (!TryGetComponent(out rb))
            {
                Debug.LogError("Rigidbody component is missing on the player object.");
                return;
            }
            rb.freezeRotation = true;

            if (statesProvider != null) playerState = statesProvider.PlayerState;
            if (eventsProvider != null) playerMovementEvents = eventsProvider.PlayerMovementEvents;
            if (inputActionsProvider != null) playerInputActions = inputActionsProvider.BaseControls.Player;
        }

        private void OnEnable()
        {
            if (inputActionsProvider == null)
            {
                return;
            }
                
            inputActionsProvider.BaseControls.Enable();
            playerInputActions.Jump.performed += Jump;
        }
        
        private void FixedUpdate()
        {
            if (playerInputActions.Move == null) return;

            bool isGrounded = IsGrounded();
            if (playerState != null)
            {
                playerState.IsGrounded = isGrounded;
            }

            Vector2 move = playerInputActions.Move.ReadValue<Vector2>();

            if (playerMovementEvents != null)
            {
                playerMovementEvents.RaiseMove(new Vector3(move.x, 0, move.y));
            }

            float x = move.x;
            float z = move.y;

            float speedZ = z > 0 ? forwardSpeed : backwardSpeed;

            Vector3 movement = new Vector3(x * strafeSpeed, 0, z * speedZ);
            movement = transform.TransformDirection(movement);

            rb.MovePosition(rb.position + movement * Time.fixedDeltaTime);
        }

        private void OnDisable()
        {
            if (inputActionsProvider == null)
            {
                return;
            }
            
            playerInputActions.Jump.performed -= Jump;
            inputActionsProvider.BaseControls.Disable();
        }

        // Functional Methods
        private void Jump(InputAction.CallbackContext context)
        {
            if (IsGrounded())
            {
                rb.linearVelocity = new Vector3(rb.linearVelocity.x, jumpForce, rb.linearVelocity.z);
                playerMovementEvents.RaiseJump();
            }
        }
        private bool IsGrounded()
        {
            return Physics.SphereCast(transform.position, sphereCastRadius, Vector3.down, out _, sphereCastDistance, groundLayer);
        }
    }
}