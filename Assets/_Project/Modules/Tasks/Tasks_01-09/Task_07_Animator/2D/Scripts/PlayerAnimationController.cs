using UnityEngine;

namespace Tasks.Animator2D
{
    public class PlayerAnimationController : MonoBehaviour
    {
        private Animator animator;

        private void Awake()
        {
            animator = GetComponent<Animator>();
        }

        private void OnEnable()
        {
            PlayerEvents.Instance.playerMovementEvents.OnIdle += HandleIdle;
            PlayerEvents.Instance.playerMovementEvents.OnLeftWalked += HandleWalk;
            PlayerEvents.Instance.playerMovementEvents.OnRightWalked += HandleWalk;
            PlayerEvents.Instance.playerMovementEvents.OnJumped += HandleJump;
        }
        private void OnDisable()
        {
            PlayerEvents.Instance.playerMovementEvents.OnIdle -= HandleIdle;
            PlayerEvents.Instance.playerMovementEvents.OnLeftWalked -= HandleWalk;
            PlayerEvents.Instance.playerMovementEvents.OnRightWalked -= HandleWalk;
            PlayerEvents.Instance.playerMovementEvents.OnJumped -= HandleJump;
        }

        private void HandleIdle()
        {
            animator.SetBool("IsWalking", false);
        }
        private void HandleWalk()
        {
            animator.SetBool("IsWalking", true);
        }
        private void HandleJump()
        {
            animator.SetTrigger("Jump");
        }
    }
}
