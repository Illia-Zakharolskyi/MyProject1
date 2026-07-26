using UnityEngine;

namespace Task.Raycast.Second
{
    public interface IInteractable
    {
         void Interact(RaycastHit hitInfo);
    }
}
