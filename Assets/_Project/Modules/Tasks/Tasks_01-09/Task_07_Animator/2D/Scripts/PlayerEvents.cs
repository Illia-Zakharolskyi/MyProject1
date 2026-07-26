using System;

namespace Tasks.Animator2D
{
    public class PlayerEvents
    {
        private static PlayerEvents _instance;

        public static PlayerEvents Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = new PlayerEvents();
                }
                return _instance;
            }
        }

        public PlayerMovementEvents playerMovementEvents { get; private set; }

        public PlayerEvents()
        {
            playerMovementEvents = new PlayerMovementEvents();
        }
    }
}