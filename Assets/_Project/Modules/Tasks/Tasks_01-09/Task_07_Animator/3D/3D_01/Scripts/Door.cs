// UnityEngine
using UnityEngine;
using UnityEngine.InputSystem;

// Project Commons
using Tasks.Learning.Animator3D.Door.Interfaces;

namespace Tasks
{
    public class Door : MonoBehaviour, IInteractable, INameable
    {
        public Animator animator;
        private bool isOpen = true;
        private bool isPlayerInRange = false;
        public GameObject InteractHint;
        public string Name { get; private set; }

        private void Awake()
        {
            Name = "Door";
        }

        private void Start()
        {
            InteractHint.SetActive(false);
        }

        private void Update()
        {
            if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame)
            {
                Interact();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (!other.TryGetComponent<INameable>(out var nameable))
            {
                Debug.Log($"{other} hasn't component INameable");
                return;
            }

            if (nameable.Name == "Player")
            {
                Debug.Log("Player in range of door");
                isPlayerInRange = true;
                InteractHint.SetActive(true);
            }
        }
        private void OnTriggerExit(Collider other)
        {
            if (!other.TryGetComponent<INameable>(out var nameable))
            {
                Debug.Log($"{other} hasn't component INameable");
                return;
            }

            if (nameable.Name == "Player")
            {
                Debug.Log("Player in range of door");
                isPlayerInRange = false;
                InteractHint.SetActive(false);
            }
        }
        public void Interact()
        {
            isOpen = !isOpen;
            animator.SetBool("Open", isOpen);
        }
    }
}