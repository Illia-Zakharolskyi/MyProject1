// Unity
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;
using ClickerGame.Scripts.src.Features;
using ClickerGame.Scripts.Events;

namespace ClickerGame.Scripts.src.GamePlay.Services
{
    public class AutoClickerUpgradeService : MonoBehaviour
    {
        public GameDataService GameDataService;

        public void UpgradeAutoClickerSpeed()
        {
            if (!GameDataService.GameData.isAutoClickerPurchased)
            {
                Debug.Log("Auto Clicker must be purchased before upgrading speed.");
                return;
            }

            if (GameDataService.GameData.currentAutoClickerSpeedUpgradeLevel == GameDataService.GetValue(GameDataKey.MaxAutoClickerUpgradeSpeedLevel))
            {
                Debug.Log("Auto Clicker Speed is already at maximum level.");
                return;
            }

            if (GameDataService.GetValue(GameDataKey.MoneyCount) < GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost))
            {
                Debug.Log($"Not enough money to upgrade auto clicker speed, current: {GameDataService.GetValue(GameDataKey.MoneyCount)}, required: {GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost)}");
                return;
            }

            GameDataService.ChangeValue(GameDataKey.MoneyCount, (double)GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost), "-");
            GameDataService.GameData.currentAutoClickerSpeedUpgradeLevel++;
            GameDataService.ChangeValue(GameDataKey.AutoClickerSpeedUpgradeCost, (double)GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost), "+");
            GameDataService.ChangeValue(GameDataKey.AutoClickerSpeed, (double)GameDataService.GetValue(GameDataKey.AutoClickerPerLevelSpeedIncrease), "-");
        }
    }
}