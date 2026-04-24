// Unity
using UnityEngine;
using TMPro;


namespace ClickerGame.Scripts.Data.ScriptableObjects.GameData
{
    [CreateAssetMenu(fileName = "GameData", menuName = "Data/GameData")]
    public class GameData : ScriptableObject
    {
        public TMP_Text MoneyText;
        public TMP_Text MoneyPerClickText;
        public TMP_Text ClickUpgradeCostText;
        public TMP_Text AutoClickerCostText;
        public TMP_Text AutoClickerUpgradeSpeedCostText;
        public TMP_Text AutoClickerMoneyPerClickText;
        public TMP_Text AutoClickerMoneyPerClickUpgradeCostText;

        public double MoneyCount = 0.00f;
        public double MoneyPerClick = 1.00f;
        public double ClickUpgradeCost = 100.00f;
        public double AutoClickerCost = 1000.00f;
        public double AutoClickerSpeedUpgradeCost = 500.00f;
        public double AutoClickerSpeed = 1.00f;
        public double AutoClickerPerLevelSpeedIncrease = 0.10f;
        public int MaxAutoClickerUpgradeSpeedLevel = 3;
        public double AutoClickerMoneyPerClick = 1.00f;
        public double AutoClickerMoneyPerClickUpgradeCost = 500.00f;
        public int AutoClickerMaxMoneyPerClickUpgradeLevel = 3;
        public double AutoClickerMoneyPerClickPerLevelUpgradeIncrease = 1.00f;
        public bool isAutoClickerPurchased = false;
        public int currentAutoClickerMoneyPerClickUpgradeLevel = 0;
        public int currentAutoClickerSpeedUpgradeLevel = 0;
    }
}
