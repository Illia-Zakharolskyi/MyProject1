using System;
using UnityEngine;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Door
{
    [CreateAssetMenu(fileName = "DoorEvents_SO", menuName = "SO/Tasks/Animator/3D/S2/Events/DoorEvents")]
    public class DoorEvents : ScriptableObject
    {
        public event Action OnDoorOpened;
        public event Action OnDoorClosed;

        public void RaiseDoorOpened()
        {
            OnDoorOpened?.Invoke();
        }

        public void RaiseDoorClosed()
        {
            OnDoorClosed?.Invoke();
        }
    }
}