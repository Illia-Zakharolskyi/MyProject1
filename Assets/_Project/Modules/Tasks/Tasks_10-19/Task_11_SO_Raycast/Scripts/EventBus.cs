using System;
using UnityEngine;

namespace Tasks.SORaycast
{
    [CreateAssetMenu(fileName = "Event_Bus", menuName = "SO/Tasks/SORaycast/Event_Bus")]
    public class EventBus : ScriptableObject
    {
        public event Action OnJackpot;

        public void InvokeJackpot()
        {
            OnJackpot?.Invoke();
        }
    }
}