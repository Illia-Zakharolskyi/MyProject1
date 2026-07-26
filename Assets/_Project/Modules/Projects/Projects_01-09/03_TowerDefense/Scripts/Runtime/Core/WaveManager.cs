using Project.TowerDefense.Runtime.Definitions;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Project.TowerDefense.Runtime
{
    public class WaveManager : MonoBehaviour
    {
        [Header("refs")]
        [SerializeField] private Tilemap _tileMap;
        [SerializeField] private Transform _enemiesParent;
        [SerializeField] private Transform _hpBarsParent;

        [Header("Spawn Points (Початок шляху)")]
        [SerializeField] private Transform[] _pointsToSpawn;
        [SerializeField] private Transform _pointToBossSpawn;

        [Header("Paths (Точки повороту)")]
        [SerializeField] private Transform[] _pointsToGo1;
        [SerializeField] private Transform[] _pointsToGo2;
        [SerializeField] private Transform[] _pointsBossToGo;

        [Header("Settings")]
        [SerializeField] private GameObject _enemyPrefab;
        [SerializeField] private float _timeBetweenEnemies = 1f;
        [SerializeField] private float _timeBeforeFirstWave = 5f;
        [SerializeField] private float _earlyClearWaveTime = 10f;
        [SerializeField] private float rotationSpeed = 10f;
        [SerializeField] private GameEvents _events;
        [SerializeField] private TowerDefenseData _data;
        [SerializeField] private int _minEnemiesInWave = 5;
        [SerializeField] private int _maxEnemiesInWave = 5;
        [SerializeField] private int _needWavesToIncreaseMaxEnemies = 6; // скільки хвиль потрібно пройти, щоб збільшити максимальну кількість ворогів
        [SerializeField] private int _timesToIncreaseMaxEnemies = 6; // скільки разів можна збільшувати максимальну кількість ворогів
        [SerializeField] private double _minPossibleHp = 0.8d; // модифікатор здоров'я для максимальної кількості ворогів (60%)
        [SerializeField] private double _maxPossibleHp = 2.5d; // модифікатор здоров'я для мінімальної кількості ворогів (250%)
        [SerializeField] private int _waveAfterIgnoreMinPossibleHp = 100; // хвиля, після якої хп не буде зменьшуватися від кількості ворогів
        [SerializeField] private int _waveStartCountingEnemiesHp = 10; // хвиля, після якої починаємо рахувати хп ворогів залежачи від іх кількості
        [SerializeField] private float _startDelayTime = 30f; // Час на початку гри
        [SerializeField] private float _finalDelayTime = 10f;  // Час, до якого ми прагнемо
        [SerializeField] private uint _waveToLockTime = 50;   // Хвиля, на якій час зафіксується
        [SerializeField] private GameObject _bossPrefab;
        [SerializeField] private int _spawnBossEveryWaves = 10;
        [SerializeField] private float _timeBeforeEnemiesSpawnIfBoss = 1; // скільки чекаємо часу щоб спавнити ворогів коли спочатку заспавнили боса?

        private List<Vector3[]> _gridPaths = new();
        private List<Vector3[]> _bossPath = new();

        private uint _currentWave = 0;
        private int _enemiesAlive = 0;
        private int _timesMaxEnemiesIncreased = 0;
        private int _enemiesCount;

        private System.Random random = new();
        private Coroutine _spawnEnemiesCoroutine = null;
        private bool _gamePlaying = true;

        void OnEnable()
        {
            _events.OnEnemyKilled += OnEnemyDestroyed;
        }

        void OnDisable()
        {
            _events.OnEnemyKilled -= OnEnemyDestroyed;
        }

        void Start()
        {
            Vector3[] _gridPath1 = ConvertTransformsToGridCenters(_pointsToSpawn[0], _pointsToGo1);
            Vector3[] _gridPath2 = ConvertTransformsToGridCenters(_pointsToSpawn[1], _pointsToGo2);
            Vector3[] _bossPath1 = new Vector3[_pointsBossToGo.Length + 1];
            _bossPath1[0] = _pointToBossSpawn.position;
            for (int i = 0; i < _pointsBossToGo.Length; i++)
            {
                _bossPath1[i + 1] = _pointsBossToGo[i].position;
            }

            _gridPaths.Add(_gridPath1);
            _gridPaths.Add(_gridPath2);
            _bossPath.Add(_bossPath1);

            _data.currentWave = 1;
            _events.InvokeMessageRequired("Ready?", MessageType.WaveCount);

            StartCoroutine(StartWaves());
        }

        private IEnumerator StartWaves()
        {
            while (_gamePlaying)
            {
                if (_currentWave == 0)
                {
                    _currentWave++;
                    StartCoroutine(TimeBeforeFirstWaveCoroutine());
                    yield return new WaitForSeconds(_timeBeforeFirstWave);
                }

                _events.InvokeMessageRequired(_currentWave, MessageType.WaveCount);
                if (_currentWave % _needWavesToIncreaseMaxEnemies == 0 && _timesMaxEnemiesIncreased < _timesToIncreaseMaxEnemies)
                {
                    _maxEnemiesInWave++;
                    _timesMaxEnemiesIncreased++;
                }

                _enemiesCount = Random.Range(_minEnemiesInWave, _maxEnemiesInWave + 1);
                double hpModifier = CalculateHpModifier(_enemiesCount, _minEnemiesInWave, _maxEnemiesInWave);
                _spawnEnemiesCoroutine = StartCoroutine(SpawnEnemyRoutine(_enemiesCount, hpModifier));

                double waveTimer = GetTimeBeforeNextWave(_currentWave);

                while (waveTimer > 0)
                {
                    if (_enemiesAlive == 0)
                    {
                        if (_spawnEnemiesCoroutine != null)
                        {
                            StopCoroutine(_spawnEnemiesCoroutine);
                            _spawnEnemiesCoroutine = null;
                        }

                        break;
                    }

                    else if (_enemiesAlive > 0)
                    {
                        _events.InvokeMessageRequired(waveTimer, MessageType.NextWaveTime);
                        waveTimer -= 1f;
                        yield return new WaitForSeconds(1f);
                    }
                }

                if (_enemiesAlive == 0)
                {
                    _events.InvokeEarlyWaveCleared();
                    yield return StartCoroutine(EarlyClearTimeRoutine());
                }

                _currentWave++;

                _data.currentWave = _currentWave;
                _events.InvokeDiamondGiven(random.Next(_data.gainDiamondPerWaveMin, _data.gainDiamondPerWaveMax));
            }
        }

        private IEnumerator EarlyClearTimeRoutine()
        {
             double timer = _earlyClearWaveTime;

            while (timer > 0)
            {
                _events.InvokeMessageRequired(timer, MessageType.NextWaveTime);
                timer -= 1f;
                yield return new WaitForSeconds(1f);
            }
        }

        private void SpawnBoss()
        {
            GameObject enemyObj = Instantiate(_bossPrefab, _pointToBossSpawn.position, Quaternion.identity, _enemiesParent);

            if (enemyObj.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (_currentWave > 1) enemy.IncreaseHP(_currentWave, _data.increaseEnemyHealthModifierMin, _data.increaseEnemyHealthModifierMax);
                enemy.InitializePath(_bossPath[0], rotationSpeed);
                enemy.HpBarParent = _hpBarsParent;
            }
        }

        public double CalculateHpModifier(int currentEnemies, int minEnemies, int maxEnemies)
        {

            //Запобігаємо діленню на нуль, якщо мін. і макс. кількість ворогів однакова
            if (maxEnemies <= minEnemies || _currentWave < _waveStartCountingEnemiesHp)
            {
                return 1.0d; // Повертаємо дефолтні 100% здоров'я
            }

            // 1. Определяем "нижнюю границу" HP в зависимости от волны:
            // До пороговой волны опускаем HP до _minPossibleHp (например, 0.6).
            // После пороговой волны опускаем максимум до 1.0 (обычные 100% HP).
            double currentMinHp = (_currentWave >= _waveAfterIgnoreMinPossibleHp) ? 1.0d : _minPossibleHp;

            // 2. Ограничиваем количество врагов в рамках [minEnemies, maxEnemies]
            currentEnemies = Mathf.Clamp(currentEnemies, minEnemies, maxEnemies);

            // 3. Находим прогресс от 0.0 (мин. врагов) до 1.0 (макс. врагов)
            double progress = (double)(currentEnemies - minEnemies) / (maxEnemies - minEnemies);

            // 4. Интерполяция:
            // При progress = 0 (мало врагов)  -> получаем _maxPossibleHp (например, 1.5)
            // При progress = 1 (много врагов) -> получаем currentMinHp (0.6 ДО порога, или 1.0 ПОСЛЕ порога)
            double modifier = _maxPossibleHp - (progress * (_maxPossibleHp - currentMinHp));

            return modifier;

            /*// 1. Знаходимо "прогрес" кількості ворогів від 0.0 до 1.0
            // (0.0 - ворогів мінімум, 1.0 - ворогів максимум)
            double progress = (double)(currentEnemies - minEnemies) / (maxEnemies - minEnemies);

            // 2. Лінійно інтерполюємо між максимумом та мінімумом.
            // При progress = 0 (мінімум ворогів) отримаємо maxPossibleHp (1.5)
            // При progress = 1 (максимум ворогів) отримаємо minPossibleHp (0.6)
            double modifier = _maxPossibleHp - (progress * (_maxPossibleHp - _minPossibleHp));

            return modifier;*/
        }

        public float GetTimeBeforeNextWave(uint currentWave)
        {
            if (currentWave >= _waveToLockTime)
            {
                return _finalDelayTime;
            }

            if (currentWave <= 1)
            {
                return _startDelayTime;
            }

            float progress = (float)(currentWave - 1) / (_waveToLockTime - 1);
            // Плавно зменшуємо час від стартового (e.g.15с) до фінального (e.g. 5с)
            return Mathf.Lerp(_startDelayTime, _finalDelayTime, progress);
        }

        private IEnumerator TimeBeforeFirstWaveCoroutine()
        {
            float timeFloat = _timeBeforeFirstWave;
            double time = (double)timeFloat;

            while (time > 0)
            {
                _events.InvokeMessageRequired(time, MessageType.NextWaveTime);
                time -= 1;
                yield return new WaitForSeconds(1f);
            }
        }

        private void OnEnemyDestroyed()
        {
            _enemiesAlive--;
            if (_enemiesAlive < 0) _enemiesAlive = 0;
        }

        private IEnumerator SpawnEnemyRoutine(int enemyCount, double hpModifier)
        {
            if (_currentWave % _spawnBossEveryWaves == 0)
            {
                SpawnBoss();
                yield return new WaitForSeconds(_timeBeforeEnemiesSpawnIfBoss);
            }

            for (int i = 0; i < enemyCount; i++)
            {
                SpawnEnemy(_gridPaths[random.Next(_gridPaths.Count)], hpModifier);
                _enemiesAlive++;
                yield return new WaitForSeconds(_timeBetweenEnemies);
            }
        }

        private void SpawnEnemy(Vector3[] path, double hpModifier)
        {
            if (path == null || path.Length == 0) return;

            GameObject enemyObj = Instantiate(_enemyPrefab, path[0], Quaternion.identity, _enemiesParent);

            if (enemyObj.TryGetComponent<Enemy>(out Enemy enemy))
            {
                if (_currentWave > 1) enemy.IncreaseHP(_currentWave, _data.increaseEnemyHealthModifierMin, _data.increaseEnemyHealthModifierMax);
                enemy.ChangeHPDependingOnCount(hpModifier);
                enemy.InitializePath(path, rotationSpeed);
                enemy.HpBarParent = _hpBarsParent;
            }
        }

        private Vector3[] ConvertTransformsToGridCenters(Transform spawnPoint, Transform[] turnPoints)
        {
            List<Vector3> worldPoints = new List<Vector3>();

            Vector3Int spawnCell = _tileMap.WorldToCell(spawnPoint.position);
            worldPoints.Add(_tileMap.GetCellCenterWorld(spawnCell));

            foreach (Transform t in turnPoints)
            {
                Vector3Int cell = _tileMap.WorldToCell(t.position);
                worldPoints.Add(_tileMap.GetCellCenterWorld(cell));
            }

            return worldPoints.ToArray();
        }
    }
}
