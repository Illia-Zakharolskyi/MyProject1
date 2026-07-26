using System;
using UnityEngine;

namespace Project.TowerDefense.Runtime
{
    public enum AudioType
    {
        TowerBullet
    }

    public enum MessageType
    {
        PlayerHealth,
        PlayerGold,
        PlayerDiamond,
        WaveCount,
        NextWaveTime
    }

    public enum EnemyType
    {
        Basic,
        Boss
    }

    public struct MessageData
    {
        public double DoubleValue;
        public uint UintValue;
        public string StringValue;

        // Зручні конструктори (імпліцитні перетворення)
        public static implicit operator MessageData(double v) => new MessageData { DoubleValue = v };
        public static implicit operator MessageData(uint v) => new MessageData { UintValue = v };
        public static implicit operator MessageData(string v) => new MessageData { StringValue = v };
    }

    [CreateAssetMenu(fileName = "Game_Events", menuName = "SO/Projects/01-09/TowerDefense/Game_Events")]
    public class GameEvents : ScriptableObject
    {
        public event Action OnGameOver;
        public event Action<bool> OnTowerUpgradePanel; // true == open, false == close
        public event Action<Sprite, uint, MonoBehaviour> OnTowerUpgradeMenuDataGiven;
        public event Action<AudioClip, AudioType> OnOneShotSFXRequested;

        public event Action<MessageData, MessageType> OnMessageRequired;
        public event Action OnPlayerDefeated;
        public event Action OnEnemyKilled;
        public event Action<EnemyType> OnEnemyKilledWithType;
        public event Action<int> OnEnemyKilledWithData;
        public event Action<int> OnDiamondGained;
        public event Action OnEarlyWaveCleared;

        public void InvokeGameOver()
        {
            OnGameOver?.Invoke();
        }

        public void InvokeTowerBuildingPanel(bool isOpen)
        {
            OnTowerUpgradePanel?.Invoke(isOpen);
        }

        public void InvokeTowerUpgradeMenuDataGiven(Sprite towerSprite, uint towerLevel, MonoBehaviour script)
        {
            OnTowerUpgradeMenuDataGiven?.Invoke(towerSprite, towerLevel, script);
        }

        public void InvokeOneShotSFXRequested(AudioClip audioClip, AudioType audioType)
        {
            OnOneShotSFXRequested?.Invoke(audioClip, audioType);
        }

        public void InvokeMessageRequired(MessageData data, MessageType type)
        {
            OnMessageRequired?.Invoke(data, type);
        }

        public void InvokePlayerDefeated()
        {
            OnPlayerDefeated?.Invoke();
        }

        public void InvokeEnemyKilled()
        {
            OnEnemyKilled?.Invoke();
        }

        public void InvokeEnemyKilledWithData(int baseRewardForKill)
        {
            OnEnemyKilledWithData?.Invoke(baseRewardForKill);
        }

        public void InvokeDiamondGiven(int plusDiamond)
        {
            OnDiamondGained?.Invoke(plusDiamond);
        }

        public void InvokeEnemyKilledWithType(EnemyType type)
        {
            OnEnemyKilledWithType?.Invoke(type);
        }

        public void InvokeEarlyWaveCleared()
        {
            OnEarlyWaveCleared?.Invoke();
        }
    }
}
