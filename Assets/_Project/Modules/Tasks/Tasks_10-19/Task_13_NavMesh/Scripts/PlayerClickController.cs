using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

namespace Task.NavMesh
{
    public class PlayerClickController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private GameObject _model;
        [SerializeField] private NavMeshAgent _agent;
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private float _speed = 1;

        private TaskNavMeshActions _actions;

        private void Awake()
        {
            _actions = new TaskNavMeshActions();
        }

        private void Start()
        {
            _agent.speed = _speed;
        }

        private void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Mouse.performed += OnClicked;
        }

        private void OnDisable()
        {
            _actions.Disable();
            _actions.Player.Mouse.performed -= OnClicked;
        }

        private void OnClicked(InputAction.CallbackContext context)
        {
            Vector2 mousePosition = _actions.Player.MousePos.ReadValue<Vector2>();
            Ray ray = _camera.ScreenPointToRay(mousePosition);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit, 100))
            {
                _agent.destination = hit.point;
            }
        }
    }
}
