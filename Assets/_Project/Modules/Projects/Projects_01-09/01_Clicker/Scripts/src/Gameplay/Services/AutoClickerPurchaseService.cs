// Unity
using UnityEngine;

// Project
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.GamePlay.Services
{
    public class AutoClickerPurchaseService : MonoBehaviour
    {
        public GameDataService gameDataService;

        public void BuyAutoClicker()
        {
            if (gameDataService.GameData.isAutoClickerPurchased)
            {
                Debug.Log("Auto clicker already purchased.");
                return;
            }

            if ((double)gameDataService.GetValue(GameDataKey.MoneyCount) < (double)gameDataService.GetValue(GameDataKey.AutoClickerCost))
            {
                Debug.Log($"Not enough money to buy auto clicker, current: {(double)gameDataService.GetValue(GameDataKey.AutoClickerCost)}, required: {(double)gameDataService.GetValue(GameDataKey.AutoClickerCost)}");
                return;
            }

            gameDataService.GameData.isAutoClickerPurchased = true;
            gameDataService.ChangeValue(GameDataKey.MoneyCount, (double)gameDataService.GetValue(GameDataKey.AutoClickerCost), "-");
            GameEvents.InvokeAutoClickerBought();
        }
    }
}