// Unity
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.Gameplay.Controllers
{
    public class UpgradeController : MonoBehaviour
    {
        public GameDataService GameDataService;

        public void UpgradeMoneyPerClick()
        {
            if (GameDataService.GetValue(GameDataKey.ClickUpgradeCost) == null || GameDataService.GetValue(GameDataKey.MoneyPerClick) == null)
            {
                Debug.LogError("Invalid parameters for class UpgradeController.cs, UpgradeMoneyPerClick()");
                return;
            }
            if (GameDataService.GetValue(GameDataKey.MoneyCount) < GameDataService.GetValue(GameDataKey.ClickUpgradeCost))
            {
                Debug.LogWarning($"Not enough Money to perform the operation. Current Money: {(int)GameDataService.GetValue(GameDataKey.MoneyCount)}, required: {(int)GameDataService.GetValue(GameDataKey.ClickUpgradeCost)}");
                return;
            }

            GameDataService.ChangeValue(GameDataKey.MoneyCount, (double)GameDataService.GetValue(GameDataKey.ClickUpgradeCost), "-");
            GameDataService.ChangeValue(GameDataKey.MoneyPerClick, 1.00f, "+");
            GameDataService.ChangeValue(GameDataKey.ClickUpgradeCost, 100.00f, "+");
        }
    }
}