using UnityEngine;
using UnityEngine.InputSystem;

namespace Tasks.Animator2D
{
    public class InputManager : MonoBehaviour
    {
        public SpriteRenderer playerSpriteRenderer;
        private string currentPlayerState = "Idle";
        private Keyboard kbd;

        private void Awake()
        {
            kbd = Keyboard.current;
        }

        private void Update()
        {
            if (kbd == null) return;

            if (kbd.aKey.isPressed) ChangeAnimation("Left");
            else if (kbd.dKey.isPressed) ChangeAnimation("Right");
            else ChangeAnimation("Idle");

            if (kbd.spaceKey.wasPressedThisFrame) PlayerEvents.Instance.playerMovementEvents.InvokeJumped();
        }

        private void ChangeAnimation(string newState)
        {
            if (currentPlayerState == newState) return;

            currentPlayerState = newState;

            switch (newState)
            {
                case "Right":
                    playerSpriteRenderer.flipX = false;
                    PlayerEvents.Instance.playerMovementEvents.InvokeRightWalked();
                    break;
                case "Left":
                    playerSpriteRenderer.flipX = true;
                    PlayerEvents.Instance.playerMovementEvents.InvokeLeftWalked();
                    break;
                case "Idle":
                    PlayerEvents.Instance.playerMovementEvents.InvokeIdle();
                    break;
            }
        }
    }
}