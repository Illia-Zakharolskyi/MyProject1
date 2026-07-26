using UnityEngine;
using DG.Tweening;

namespace Task.Raycast.Second
{
    [RequireComponent(typeof(BoxCollider))]
    public class Door : MonoBehaviour
    {
        [SerializeField] private Transform _pivot;
        [SerializeField] private float _startY;
        [SerializeField] private float _duration;
        [SerializeField] private float _maxY;

        private void OnTriggerEnter(Collider other)
        {
            _pivot.DOMoveY(_maxY, _duration);
        }

        private void OnTriggerExit(Collider other)
        {
            _pivot.DOMoveY(_startY, _duration);
        }
    }
}
