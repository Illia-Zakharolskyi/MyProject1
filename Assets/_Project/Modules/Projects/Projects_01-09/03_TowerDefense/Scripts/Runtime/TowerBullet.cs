using Project.TowerDefense.Runtime;
using UnityEngine;

public class TowerBullet : MonoBehaviour
{
    private Transform _target;
    private float _speed;
    private double _damage;
    [SerializeField] private GameEvents _gameEvents;

    private Vector3 _lastTargetPosition;

    public void Setup(Transform target, float speed, double damage, AudioClip clip)
    {
        _target = target;
        _speed = speed;
        _damage = damage;

        _lastTargetPosition = target.position;
        _gameEvents.InvokeOneShotSFXRequested(clip, Project.TowerDefense.Runtime.AudioType.TowerBullet);
    }

    void Update()
    {
        if (_target == null)
        {
            transform.position = Vector3.MoveTowards(transform.position, _lastTargetPosition, _speed * Time.deltaTime);

            if (Vector3.Distance(transform.position, _lastTargetPosition) < 0.1f) Destroy(gameObject);
            return;
        }

        _lastTargetPosition = _target.position;
        transform.position = Vector3.MoveTowards(transform.position, _target.position, _speed * Time.deltaTime);

        //Перевірка на доліт до цілі
        if (Vector3.Distance(transform.position, _target.position) < 0.1f)
        {
            if (_target.TryGetComponent<Enemy>(out var script))
            {
                script.TakeDamage(_damage);
            }
            Destroy(gameObject);
        }
    }
}

public interface IFindTargetStrategy
{
    Transform? FindBestTarget(Collider2D[] caughtEnemies); // повертає найкращу ціль з масиву caughtEnemies, або null якщо ціль не знайдена
}

public class FirstTargetStrategy : IFindTargetStrategy
{
    public Transform? FindBestTarget(Collider2D[] caughtEnemies)
    {
        if (caughtEnemies.Length > 0)
        {
            Enemy bestTarget = null;
            foreach (Collider2D enemyCollider in caughtEnemies)
            {
                if (enemyCollider.TryGetComponent<Enemy>(out Enemy currentEnemy))
                {
                    if (bestTarget == null) { bestTarget = currentEnemy; continue; }

                    if (currentEnemy.CurrentPointIndex > bestTarget.CurrentPointIndex)
                    {
                        bestTarget = currentEnemy;
                    }
                    else if (currentEnemy.CurrentPointIndex == bestTarget.CurrentPointIndex)
                    {
                        if (currentEnemy.GetDistanceToNextPoint() < bestTarget.GetDistanceToNextPoint())
                        {
                            bestTarget = currentEnemy;
                        }
                    }
                }
            }
            return bestTarget.transform != null ? bestTarget.transform : null;
        }
        else
        {
            return null;
        }
    }
}

public class LastTargetStrategy : IFindTargetStrategy
{
    public Transform? FindBestTarget(Collider2D[] caughtEnemies)
    {
        if (caughtEnemies.Length > 0)
        {
            Enemy bestTarget = null;
            foreach (Collider2D enemyCollider in caughtEnemies)
            {
                if (enemyCollider.TryGetComponent<Enemy>(out Enemy currentEnemy))
                {
                    if (bestTarget == null) { bestTarget = currentEnemy; continue; }
                    if (currentEnemy.CurrentPointIndex < bestTarget.CurrentPointIndex)
                    {
                        bestTarget = currentEnemy;
                    }
                    else if (currentEnemy.CurrentPointIndex == bestTarget.CurrentPointIndex)
                    {
                        if (currentEnemy.GetDistanceToNextPoint() > bestTarget.GetDistanceToNextPoint())
                        {
                            bestTarget = currentEnemy;
                        }
                    }
                }
            }
            return bestTarget.transform != null ? bestTarget.transform : null;
        }
        else
        {
            return null;
        }
    }
}

public class StrongestTargetStrategy : IFindTargetStrategy
{
    public Transform? FindBestTarget(Collider2D[] caughtEnemies)
    {
        if (caughtEnemies.Length > 0)
        {
            Enemy bestTarget = null;
            foreach (Collider2D enemyCollider in caughtEnemies)
            {
                if (enemyCollider.TryGetComponent<Enemy>(out Enemy currentEnemy))
                {
                    if (bestTarget == null) { bestTarget = currentEnemy; continue; }
                    if (currentEnemy.CurrentHP > bestTarget.CurrentHP)
                    {
                        bestTarget = currentEnemy;
                    }
                }
            }
            return bestTarget.transform != null ? bestTarget.transform : null;
        }
        else
        {
            return null;
        }
    }
}

public class WeakestTargetStrategy : IFindTargetStrategy
{
    public Transform? FindBestTarget(Collider2D[] caughtEnemies)
    {
        if (caughtEnemies.Length > 0)
        {
            Enemy bestTarget = null;
            foreach (Collider2D enemyCollider in caughtEnemies)
            {
                if (enemyCollider.TryGetComponent<Enemy>(out Enemy currentEnemy))
                {
                    if (bestTarget == null) { bestTarget = currentEnemy; continue; }
                    if (currentEnemy.CurrentHP < bestTarget.CurrentHP)
                    {
                        bestTarget = currentEnemy;
                    }
                }
            }
            return bestTarget.transform != null ? bestTarget.transform : null;
        }
        else
        {
            return null;
        }
    }
}