using UnityEngine;
using UnityEngine.AI;

namespace Task.NavMesh
{
    public class EnemyNavMeshController : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _target;
        [SerializeField] private NavMeshAgent _agent;

        private void Update()
        {
            _agent.SetDestination(_target.position);
        }
    }
}
