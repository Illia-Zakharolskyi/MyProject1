using UnityEngine;

namespace Project.TowerDefense.Runtime.Definitions
{
    [CreateAssetMenu(fileName = "Tower_Defense_Data", menuName = "SO/Projects/TowerDefense/Data")]
    public class TowerDefenseData : ScriptableObject
    {
        //[SerializeField] public int enemyKillGoldReward;

        // Start Values
        [SerializeField] public int startGoldAmount;
        [SerializeField] public int startDiamondAmount;

        // Base Values
        [SerializeField] public int baseGoldRewardForWave;

        // Max Values
        [SerializeField] public int maxDiamondAmount;

        // Gain Values
        [SerializeField] public int gainDiamondPerWaveMin;
        [SerializeField] public int gainDiamondPerWaveMax;
        [SerializeField] public int gainDiamondPerBoss;

        // Upgrades
        [SerializeField] public double damageUpgradeModifierMin = 1.08;
        [SerializeField] public double damageUpgradeModifierMax = 1.12;
        [SerializeField] public double damageUpgradeCostModifierMin = 1.18;
        [SerializeField] public double damageUpgradeCostModifierMax = 1.22;
        [SerializeField] public double goldRewardForWaveModifier = 1.20;

        // per Wave
        [SerializeField] public double rewardForKillingEnemyModifierMin = 1.18;
        [SerializeField] public double rewardForKillingEnemyModifierMax = 1.22;
        [SerializeField] public double increaseEnemyHealthModifierMin = 1.08;
        [SerializeField] public double increaseEnemyHealthModifierMax = 1.12;

        // common
        public uint currentWave = 1;
        public bool isTowerCircleShowing = false;

        void OnEnable()
        {
            isTowerCircleShowing = false;
        }
    }
}
