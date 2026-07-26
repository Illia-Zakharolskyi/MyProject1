using Project.TowerDefense.Runtime.Definitions;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Project.TowerDefense.Runtime
{
    public class Enemy : MonoBehaviour
    {
        private Vector3[] _worldPath;
        private int _currentPointIndex = 0;
        private float _rotationSpeed = 10f; // Будемо отримувати від менеджера хвиль
        [SerializeField] private float _moveSpeed = 2f;
        public int CurrentPointIndex => _currentPointIndex;
        [SerializeField] private int _baseRewardForKill;
        [SerializeField] private double _maxHP;
        [SerializeField] private TowerDefenseData _data;
        [SerializeField] private GameEvents _events;
        public double MaxHP => _maxHP;
        private double _currentHP;
        public double CurrentHP => _currentHP;
        [SerializeField] private double _baseHP;
        public double BaseHP => _baseHP;
        [SerializeField] private GameObject _hpBarPrefab;
        [NonSerialized] public Transform HpBarParent;
        [SerializeField] private EnemyType _type = EnemyType.Basic;

        private Slider hpSlider;

        void Start()
        {
            _currentHP = _maxHP;
            SpawnHPBar();
        }

        public void TakeDamage(double amount)
        {
            _currentHP -= amount;
            UpdateHPBar();
            if (_currentHP <= 0)
            {
                _events.InvokeEnemyKilled();
                _events.InvokeEnemyKilledWithData(_baseRewardForKill);
                if (_type == EnemyType.Boss) _events.InvokeEnemyKilledWithType(EnemyType.Boss);
                Destroy(gameObject);
            }
        }

        public void IncreaseHP(uint wave, double minModifier, double maxModifier)
        {
            _maxHP = _baseHP * Math.Pow(UnityEngine.Random.Range((float)minModifier, (float)maxModifier), wave - 1);
            _currentHP = _maxHP;

            UnityEngine.Debug.Log($"[HP Debug] Хвиля: {wave}. Базове ХП: {_baseHP}. Макс ХП після модифікатора хвилі: {_maxHP}");

            /*_maxHP = _baseHP * Math.Pow(minModifier, wave - 1);
            _currentHP = _maxHP;*/
        }

        public void ChangeHPDependingOnCount(double hpModifier)
        {
            if (hpModifier <= 0)
            {
                Debug.Log("Nah");
                return;
            }

            _maxHP *= hpModifier;
            _currentHP = _maxHP;
        }

        void SpawnHPBar()
        {
            // 1. Шукаємо наш єдиний Canvas на сцені (можна за тегом або зробити синглтон)
            Canvas _canvas = FindAnyObjectByType<Canvas>();

            if (_canvas != null)
            {
                // 2. Спавнюємо смужку всередині цього Canvas
                GameObject hpBarGo = Instantiate(_hpBarPrefab);
                hpBarGo.transform.SetParent(HpBarParent);

                // 3. Передаємо скрипту смужки трансформ цього конкретного ворога
                EnemyHPBar followerScript = hpBarGo.GetComponent<EnemyHPBar>();
                if (followerScript != null)
                {
                    followerScript.enemyTransform = this.transform;
                }

                // 4. Отримуємо компонент Slider, щоб міняти заповнення при отриманні урону
                hpSlider = hpBarGo.GetComponent<Slider>();
                hpSlider.maxValue = 1f;
                hpSlider.value = 1f;
            }
        }

        void UpdateHPBar()
        {
            hpSlider.value = (float)(_currentHP / _maxHP);
        }

        public float GetDistanceToNextPoint()
        {
            if (_worldPath == null || _currentPointIndex >= _worldPath.Length) return 0f;
            return Vector3.Distance(transform.position, _worldPath[_currentPointIndex]);
        }

        // Оновлений метод ініціалізації: тепер приймає ще й швидкість повороту
        public void InitializePath(Vector3[] worldPoints, float rotSpeed)
        {
            _worldPath = worldPoints;
            _rotationSpeed = rotSpeed;

            if (_worldPath.Length > 0)
            {
                transform.position = _worldPath[0];

                // Миттєво розвертаємо ворога «носом» до першої цілі при спавні
                LookAtTargetImmediate();

                StartCoroutine(FollowPathRoutine());
            }
        }

        private IEnumerator FollowPathRoutine()
        {
            while (_currentPointIndex < _worldPath.Length)
            {
                Vector3 targetPosition = _worldPath[_currentPointIndex];

                while (Vector3.Distance(transform.position, targetPosition) > 0.05f)
                {
                    // 1. Рух вперед
                    transform.position = Vector3.MoveTowards(transform.position, targetPosition, _moveSpeed * Time.deltaTime);

                    // 2. Поворот у бік цілі
                    RotateTowards(targetPosition);

                    yield return null;
                }

                _currentPointIndex++;
            }
        }

        // Розрахунок кута і плавний розворот по осі Z
        private void RotateTowards(Vector3 target)
        {
            Vector3 direction = target - transform.position;

            if (direction != Vector3.zero)
            {
                // Математика 2D-повороту: рахуємо кут на площині XY
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);

                // Плавний поворот через Slerp
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
            }
        }

        // Щоб ворог не спавнився боком, а одразу дивився куди треба
        private void LookAtTargetImmediate()
        {
            if (_worldPath.Length > 1)
            {
                Vector3 direction = _worldPath[1] - transform.position;
                if (direction != Vector3.zero)
                {
                    float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                    transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);
                }
            }
        }
    }
}
