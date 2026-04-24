// UnityEngine
using UnityEngine;

// Project Commons
using Tasks.Learning.Animator3D.Door.Interfaces;


namespace Tasks
{
    public class Player : MonoBehaviour, INameable
    {
        public string Name { get; private set; }

        private void Awake()
        {
            Name = "Player";
        }
    }
}