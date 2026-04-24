// UnityEngine using directives
using UnityEngine;
using UnityEngine.InputSystem;

// Task using directives
using Tasks.Learning.Animator.ThreeD.S2.Generated;
using Tasks.Learning.Animator.ThreeD.S2.Providers;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Door
{
    public class Door : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private UnityEngine.Animator animator;
        [SerializeField] private GameObject interactHint;
        [SerializeField] private InputActionsProvider provider;
        [SerializeField] private DoorEvents doorEvents;

        private bool open = false;
        private bool playerInRange = false;

        private InputActions.PlayerActions playerActions;

        // Lifecycle Methods
        private void Awake()
        {
            playerActions = provider.InputActions.Player;

            if (animator == null || interactHint == null || playerActions.Interact == null)
            {
                Debug.LogError($"One or more required references are missing on the {this.name}");
                return;
            }
        }
        private void Start()
        {
            interactHint.SetActive(false);
        }

        private void OnEnable()
        {
            if (playerActions.Interact == null) return;

            playerActions.Enable();
            playerActions.Interact.performed += OnInteract;
        }

        private void OnDisable()
        {
            if (playerActions.Interact == null) return;

            playerActions.Disable();
            playerActions.Interact.performed -= OnInteract;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            if (interactHint == null) return;

            playerInRange = true;
            interactHint.SetActive(true);
        }
        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer != LayerMask.NameToLayer("Player")) return;
            if (interactHint == null) return;

            playerInRange = false;
            interactHint.SetActive(false);
        }

        // Functional Methods
        private void OnInteract(InputAction.CallbackContext context)
        {
            if (playerInRange)
            {
                open = !open;
                animator.SetBool("Open", open);

                if (open) doorEvents.RaiseDoorOpened();
                else doorEvents.RaiseDoorClosed();
            }
        }
    }
}