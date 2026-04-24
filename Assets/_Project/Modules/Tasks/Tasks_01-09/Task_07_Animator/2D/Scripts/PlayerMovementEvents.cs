using System;

namespace Tasks.Animator2D
{
    public class PlayerMovementEvents
    {
        // Idle Events
        public event Action OnIdle;

        // Jumped Events
        public event Action OnJumped;

        // Walked Events
        public event Action OnRightWalked;
        public event Action OnLeftWalked;

        // Idle Methods
        public void InvokeIdle() => OnIdle?.Invoke();

        // Jumped Methods
        public void InvokeJumped() => OnJumped?.Invoke();


        // Walked Methods
        public void InvokeRightWalked() => OnRightWalked?.Invoke();
        public void InvokeLeftWalked() => OnLeftWalked?.Invoke();
    }
}