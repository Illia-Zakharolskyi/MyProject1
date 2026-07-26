using Project.TowerDefense.InTesting;
using Project.TowerDefense.Runtime;
using Project.TowerDefense.Runtime.Definitions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform[] _firePoints;
    [SerializeField] private GameObject _bulletPrefab;
    [SerializeField] private Sprite _towerSprite;
    [SerializeField] private GameEvents _events;
    [SerializeField] private TowerDefenseData _data;
    [SerializeField] private AudioClip _onShootAudio;

    [Header("Settings")]
    [SerializeField] private float _attackRadius = 10f;
    [SerializeField] private LayerMask _mask;
    [SerializeField] private float _intervalBetweenEnemiesFinding = 0.1f;
    [SerializeField] private float _shootCooldown = 1f;
    [SerializeField] private float _bulletSpeed = 12f;
    [SerializeField] private double _bulletDamage = 1.0;
    [SerializeField] private double _upgradeCost = 100.0;
    [SerializeField] public IFindTargetStrategy _findTargetStrategy = new FirstTargetStrategy();
    /*[SerializeField] private float _coolDownSlowdownModifier = 1.04f; // Множник, на який збільшується кулдаун (4% збільшення кулдауну)
    [SerializeField] private int _coolDownSlowdownLevel = 5; // Кожні 5 рівнів кулдаун збільшується на множник
    [SerializeField] private int _coolDownTimes = 5; // Кількість разів, на які можна збільшити кулдаун*/
    [SerializeField] private GameObject _radiusCircle;
    //private int _currentCoolDownTimes = 0;
    public double BulletDamage => _bulletDamage;
    private uint _currentLevel = 1;
    public uint CurrentLevel => _currentLevel;
    public bool _upgradeActive = false;
    private bool _isFirstPlaced = true;
    public int _cursorStrategyIndex = 0;

    private Transform _targetEnemy = null;
    private readonly System.Random rnd = new();

    void Start()
    {
        List<string> warnings = new();

        if (_firePoints == null)
        {
            warnings.Add("no fire points to shoot");
        }

        if (_bulletPrefab == null)
        {
            warnings.Add("no sprite for bullet");
        }

        if (warnings.Count > 0)
        {
            Debug.LogWarning($"Component {this.name} on {gameObject.name} needs to be filled!");
            foreach (string warning in warnings)
            {
                Debug.LogWarning(warning);
            }
        }
    }

    void OnEnable()
    {
        // Запускаємо корутини: одна шукає, друга стріляє. Вони працюють паралельно і автономно.
        StartCoroutine(CheckRadiusRoutine());
        StartCoroutine(ShootRoutine());
    }

    void OnDisable()
    {
        StopAllCoroutines();
    }

    void Update()
    {
        // ПОВОРОТ ВЕЖІ
        if (_targetEnemy != null)
        {
            Vector3 direction = _targetEnemy.position - transform.position;
            if (direction != Vector3.zero)
            {
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
                // Плавно повертаємо вежу (швидкість 10f)
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, 10f * Time.deltaTime);
            }
        }
    }

    // КОРУТИНА СТРІЛЬБИ
    IEnumerator ShootRoutine()
    {
        while (true)
        {
            if (_targetEnemy != null)
            {
                // Обираємо рандомну точку вогню
                int index = rnd.Next(_firePoints.Length);
                Transform shootFrom = _firePoints[index];

                // Спавним кулю
                GameObject bulletObj = Instantiate(_bulletPrefab, shootFrom.position, Quaternion.identity);

                // Передаємо кулі ціль та швидкість
                if (bulletObj.TryGetComponent<TowerBullet>(out var bullet))
                {
                    bullet.Setup(_targetEnemy, _bulletSpeed, _bulletDamage, _onShootAudio);
                }

                // Чекаємо кулдаун перед наступним пострілом
                yield return new WaitForSeconds(_shootCooldown);
            }
            else
            {
                // Якщо ворога немає — чекаємо кадр і перевіряємо знову в наступному
                yield return null;
            }
        }
    }

    // КОРУТИНА ПОШУКУ
    IEnumerator CheckRadiusRoutine()
    {
        while (true)
        {
            Collider2D[] caughtEnemies =  Physics2D.OverlapCircleAll(transform.position, _attackRadius, _mask);
            Transform? bestTarget = _findTargetStrategy.FindBestTarget(caughtEnemies);

            if (bestTarget == null)
            {
                _targetEnemy = null;
            }
            else
            {
                _targetEnemy = bestTarget;
            }

            yield return new WaitForSeconds(_intervalBetweenEnemiesFinding);
        }
    }

    public void OnMouseDown()
    {
        Debug.Log("Yeah!");
        /*if (UnityEngine.EventSystems.EventSystem.current.IsPointerOverGameObject())
        {
            Debug.Log("Nah");
            return;
        }*/

        if (!_upgradeActive && !_isFirstPlaced && !_data.isTowerCircleShowing)
        {
            _events.InvokeTowerUpgradeMenuDataGiven(_towerSprite, _currentLevel, this);
            _data.isTowerCircleShowing = true;
            return;
        }
        else
        {
            if (_isFirstPlaced) _isFirstPlaced = false;
            return;
        }
    }

    public void UpgradeStats(double minModifier, double maxModifier)
    {
        _bulletDamage *= Random.Range((float)minModifier, (float)maxModifier);
        _currentLevel++;

        /*if (_currentLevel % _coolDownSlowdownLevel == 0 && _currentCoolDownTimes < _coolDownTimes)
        {
            _shootCooldown *= _coolDownSlowdownModifier;
            _currentCoolDownTimes++;
        }*/

        /*_bulletDamage *= minModifier;
        _currentLevel++;*/
    }

    public double GetUpgradeCost()
    {
        return _upgradeCost;
    }

    public void IncreaseUpgradeCost(double minModifier, double maxModifier)
    {
        _upgradeCost *= Random.Range((float)minModifier, (float)maxModifier);
    }

    public void ShowRadiusCircle()
    {
        _radiusCircle.transform.localScale = new Vector3(_attackRadius * 2, _attackRadius * 2, 0);
        _radiusCircle.SetActive(true);
    }

    public void HideRadiusCircle()
    {
        _radiusCircle.SetActive(false);
    }
}