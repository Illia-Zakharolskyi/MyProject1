using Project.TowerDefense.Runtime.Definitions;
using Project.TowerDefense.Runtime;
using System;
using UnityEngine;

namespace Project.TowerDefense.Testing
{
    public enum CurrencyType
    {
        Gold,
        Diamond
    }

    public class PlayerCurrencyManager : MonoBehaviour
    {
        public PlayerCurrencyManager Instance { get; private set; }
        [SerializeField] private GameEvents _events;
        [SerializeField] private TowerDefenseData _data;
        private double _gold = 0;
        private int _diamond = 0;

        private void Awake()
        {
            if (Instance == null) Instance = this;
            if (Instance != this) Destroy(gameObject);
        }

        private void Start()
        {
            _gold = _data.startGoldAmount;
            _diamond = _data.startDiamondAmount;

            _events.InvokeMessageRequired(_gold, MessageType.PlayerGold);
            _events.InvokeMessageRequired(_diamond, MessageType.PlayerDiamond);
        }

        private void OnEnable()
        {
            _events.OnEnemyKilledWithData += OnEnemyKilled;
            _events.OnDiamondGained += OnDiamondGained;
            _events.OnEarlyWaveCleared += OnEarlyWaveCleared;
        }
        private void OnDisable()
        {
            _events.OnEnemyKilledWithData -= OnEnemyKilled;
            _events.OnDiamondGained -= OnDiamondGained;
            _events.OnEarlyWaveCleared -= OnEarlyWaveCleared;
        }

        public void OnEnemyKilled(int baseReward)
        {
            // 1. Захищаємо степінь від від'ємних значень (якщо currentWave <= 0, беремо 0)
            uint waveExponent = Math.Max(0, _data.currentWave - 1);

            // 2. Рахуємо рандомний множник
            double randomModifier = UnityEngine.Random.Range(
                (float)_data.rewardForKillingEnemyModifierMin,
                (float)_data.rewardForKillingEnemyModifierMax
            );

            // 3. Розрахунок нагороди
            double calculatedReward = baseReward * Math.Pow(randomModifier, waveExponent);

            // 4. Фінальний захист: якщо щось пішло не так, даємо базову нагороду, а не ламаємо гру
            if (double.IsNaN(calculatedReward) || double.IsInfinity(calculatedReward))
            {
                Debug.LogWarning("[CurrencyManager]: Розрахунок нагороди зламався! Використано базове значення.");
                calculatedReward = baseReward;
            }

            _gold += calculatedReward;
            _events.InvokeMessageRequired(_gold, MessageType.PlayerGold);
        }

        public void OnBossKilled(EnemyType type)
        {
            if (type != EnemyType.Boss) return;

            AddCurrency(_data.gainDiamondPerBoss, CurrencyType.Diamond);
        }

        public bool HasEnoughCurrency(double value, CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.Gold:
                    return _gold >= value;

                case CurrencyType.Diamond:
                    return _diamond >= value;

                default:
                    LogTypeErrorAndReturnFalse(type, typeof(double));
                    return false;
            };
        }

        public void SpendCurrency(double value, CurrencyType type)
        {
            if (!HasEnoughCurrency(value, type))
            {
                Debug.Log("[CurrencyManager]: You are broke!");
                return;
            }

            switch (type)
            {
                case CurrencyType.Gold:
                    _gold -= value;
                    _events.InvokeMessageRequired(_gold, MessageType.PlayerGold);
                    break;

                case CurrencyType.Diamond:
                    _diamond -= (int)value;
                    _events.InvokeMessageRequired(_diamond, MessageType.PlayerDiamond);
                    break;

                default:
                    LogTypeErrorAndReturnFalse(type, typeof(double));
                    break;
            }
        }

        public void AddCurrency(double value, CurrencyType type)
        {
            switch (type)
            {
                case CurrencyType.Gold:
                    _gold += value;
                    _events.InvokeMessageRequired(_gold, MessageType.PlayerGold);
                    break;

                case CurrencyType.Diamond:
                    _diamond += (int)value;
                    _events.InvokeMessageRequired(_diamond, MessageType.PlayerDiamond);
                    break;

                default:
                    LogTypeErrorAndReturnFalse(type, typeof(double));
                    return;
            }
        }

        public void OnDiamondGained(int plusDiamond)
        {
            if (_diamond == _data.maxDiamondAmount) return;

            _diamond += plusDiamond;
            if (_diamond > _data.maxDiamondAmount) _diamond = _data.maxDiamondAmount;
            _events.InvokeMessageRequired(_diamond, MessageType.PlayerDiamond);
        }

        public void OnEarlyWaveCleared()
        {
            double growthFactor = Math.Pow(_data.goldRewardForWaveModifier, _data.currentWave - 1);
            double calculatedReward = _data.baseGoldRewardForWave * growthFactor;

            _gold += calculatedReward;
            _events.InvokeMessageRequired(_gold, MessageType.PlayerGold);
        }

        private bool LogTypeErrorAndReturnFalse(CurrencyType type, Type incomingType)
        {
            Debug.LogError($"[CurrencyManager] Невідповідність типу даних для {type}. Отримано: {incomingType}");
            return false;
        }
    }
}