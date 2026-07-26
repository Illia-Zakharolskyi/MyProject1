// Unity
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;
using ClickerGame.Scripts.src.Features;
using ClickerGame.Scripts.Events;

namespace ClickerGame.Scripts.src.GamePlay.Services
{
    public class AutoClickerMoneyPerClickUpgradeService : MonoBehaviour
    {
        public GameDataService GameDataService;

        public void Upgrade()
        {
            if (!GameDataService.GameData.isAutoClickerPurchased)
            {
                Debug.Log("Auto Clicker must be purchased before upgrading money per click.");
                return;
            }

            if (GameDataService.GameData.currentAutoClickerMoneyPerClickUpgradeLevel == GameDataService.GetValue(GameDataKey.AutoClickerMaxMoneyPerClickUpgradeLevel))
            {
                Debug.Log("Auto Clicker Money Per Click is already at maximum level.");
                return;
            }

            if (GameDataService.GetValue(GameDataKey.MoneyCount) < GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost))
            {
                Debug.Log("Not enough money to upgrade Auto Clicker Money Per Click.");
                return;
            }

            GameDataService.ChangeValue(GameDataKey.MoneyCount, (double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost), "-");
            GameDataService.GameData.currentAutoClickerMoneyPerClickUpgradeLevel++;
            GameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClick, (double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickPerLevelUpgradeIncrease), "+");
            GameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, (double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost), "+");
            GameEvents.InvokeAutoClickerMoneyPerClickChanged((double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClick));
            GameEvents.InvokeAutoClickerMoneyPerClickUpgradeCostChanged((double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost));
        }
    }
}