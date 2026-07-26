using Common.Scripts.Extensions;
using Project.TowerDefense.Runtime;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class PlayerBase : MonoBehaviour
{
    [SerializeField] private GameEvents _gameEvents;
    [SerializeField] private double Health = 5;
    [SerializeField] private double HealthTakenByPenetration = 1;
    [SerializeField] private LayerMask _mask;

    void Start()
    {
        _gameEvents.InvokeMessageRequired(Health, MessageType.PlayerHealth);
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log("Entered");
        if (!_mask.Contains(collision.gameObject.layer))
        {
            return;
        }

        Health -= HealthTakenByPenetration;
        Destroy(collision.gameObject);

        _gameEvents.InvokeMessageRequired(Health, MessageType.PlayerHealth);
        if (Health <= 0) _gameEvents.InvokePlayerDefeated();
    }
}
