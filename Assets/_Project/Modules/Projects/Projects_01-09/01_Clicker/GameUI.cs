// UnityEngine
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Gameplay.Controllers;
using ClickerGame.Scripts.src.GamePlay.Services;

namespace ClickerGame
{
    public class GameUI : MonoBehaviour
    {
        public UpgradeController upgradeController;
        public AutoClickerUpgradeService autoClickerUpgradeService;
        public AutoClickerPurchaseService autoClickerPurchaseService;
        public AutoClickerMoneyPerClickUpgradeService autoClickerMoneyPerClickUpgradeService;
        public ClickerController clickerController;

        public void OnMoneyPerClickUpgrade()
        {
            upgradeController.UpgradeMoneyPerClick();
        }

        public void OnAutoClickerSpeedUpgrade()
        {
            autoClickerUpgradeService.UpgradeAutoClickerSpeed();
        }

        public void OnAutoClickerBuy()
        {
            autoClickerPurchaseService.BuyAutoClicker();
        }

        public void OnAutoClickerMoneyPerClickUpgrade()
        {
            autoClickerMoneyPerClickUpgradeService.Upgrade();
        }

        public void OnMoneyAdd()
        {
            clickerController.AddMoney();
        }

        public void OnPreviousSceneLoad()
        {
            SceneController.Instance.LoadPreviousScene();
        }
    }
}