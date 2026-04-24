// Unity
using UnityEngine;

// Project
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.Gameplay.Controllers
{
    public class ClickerController : MonoBehaviour
    {
        public GameDataService GameDataService;
        public SaveLoaderService SaveLoaderService;

        private void OnEnable()
        {
            GameEvents.OnAutoClickerWorked += AddMoney;
        }
        private void OnDisable()
        {
            GameEvents.OnAutoClickerWorked -= AddMoney;
        }

        public void AddMoney()
        {
            GameDataService.ChangeValue(GameDataKey.MoneyCount, (double)GameDataService.GetValue(GameDataKey.MoneyPerClick), "+");
            SaveLoaderService.SaveProgress();
        }
        private void AddMoney(double moneyPerClick)
        {
            GameDataService.ChangeValue(GameDataKey.MoneyCount, moneyPerClick, "+");
            SaveLoaderService.SaveProgress();
        }
    }
}
