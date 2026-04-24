using UnityEngine;

using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.Core.Services
{
    public class SaveLoaderService : MonoBehaviour
    {
        public GameDataService gameDataService;

        private void Awake()
        {
            LoadProgress();
        }
        private void Start()
        {
            LoadProgress();
        }

        public void SaveProgress()
        {

            PlayerPrefs.SetFloat("MoneyCount", ToFloat((double)gameDataService.GetValue(GameDataKey.MoneyCount)));
            PlayerPrefs.SetFloat("MoneyPerClick", ToFloat((double)gameDataService.GetValue(GameDataKey.MoneyPerClick)));
            PlayerPrefs.SetFloat("ClickUpgradeCost", ToFloat((double)gameDataService.GetValue(GameDataKey.ClickUpgradeCost)));
            PlayerPrefs.SetFloat("AutoClickerSpeedUpgradeCost", ToFloat((double)gameDataService.GetValue((GameDataKey.AutoClickerSpeedUpgradeCost))));
            PlayerPrefs.SetFloat("AutoClickerSpeed", ToFloat((double)gameDataService.GetValue((GameDataKey.AutoClickerSpeed))));
            PlayerPrefs.SetFloat("AutoClickerMoneyPerClick", ToFloat((double)gameDataService.GetValue((GameDataKey.AutoClickerMoneyPerClick))));
            PlayerPrefs.SetFloat("AutoClickerMoneyPerClickUpgradeCost", ToFloat((double)gameDataService.GetValue((GameDataKey.AutoClickerMoneyPerClickUpgradeCost))));
            PlayerPrefs.SetInt("IsAutoClickerPurchased", gameDataService.GameData.isAutoClickerPurchased == true ? 1 : 0);
            PlayerPrefs.SetInt("CurrentAutoClickerMoneyPerClickUpgradeLevel", ToInt((double)gameDataService.GetValue((GameDataKey.CurrentAutoClickerMoneyPerClickUpgradeLevel))));
            PlayerPrefs.SetFloat("CurrentAutoClickerSpeedUpgradeLevel", ToFloat((double)gameDataService.GetValue((GameDataKey.CurrentAutoClickerSpeedUpgradeLevel))));
        }


        public void LoadProgress()
        {
            gameDataService.ChangeValue(GameDataKey.MoneyCount, PlayerPrefs.GetFloat("MoneyCount", 0), "=");
            gameDataService.ChangeValue(GameDataKey.MoneyPerClick, PlayerPrefs.GetFloat("MoneyPerClick", 0), "=");
            gameDataService.ChangeValue(GameDataKey.ClickUpgradeCost, PlayerPrefs.GetFloat("ClickUpgradeCost", 0), "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerSpeedUpgradeCost, PlayerPrefs.GetFloat("AutoClickerSpeedUpgradeCost", 0), "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerSpeed, PlayerPrefs.GetFloat("AutoClickerSpeed", 0), "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClick, PlayerPrefs.GetFloat("AutoClickerMoneyPerClick", 0), "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, PlayerPrefs.GetFloat("AutoClickerMoneyPerClickUpgradeCost", 0), "=");
            gameDataService.ChangeValue(GameDataKey.CurrentAutoClickerSpeedUpgradeLevel, PlayerPrefs.GetInt("CurrentAutoClickerSpeedUpgradeLevel", 0), "=");
            gameDataService.ChangeValue(GameDataKey.CurrentAutoClickerMoneyPerClickUpgradeLevel, PlayerPrefs.GetInt("CurrentAutoClickerMoneyPerClickUpgradeLevel", 0), "=");
            gameDataService.GameData.isAutoClickerPurchased = PlayerPrefs.GetInt("IsAutoClickerPurchased", 0) == 1 ? true : false;
        }

        public void ResetProgress()
        {
            PlayerPrefs.DeleteAll();

            gameDataService.ChangeValue(GameDataKey.MoneyCount, 0.00f, "=");
            gameDataService.ChangeValue(GameDataKey.MoneyPerClick, 1.00f, "=");
            gameDataService.ChangeValue(GameDataKey.ClickUpgradeCost, 100.00f, "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerSpeedUpgradeCost, 500.00f, "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerSpeed, 1.00f, "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClick, 1.00f, "=");
            gameDataService.ChangeValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, 500.00f, "=");
            gameDataService.ChangeValue(GameDataKey.CurrentAutoClickerSpeedUpgradeLevel, 1.00f, "=");
            gameDataService.ChangeValue(GameDataKey.CurrentAutoClickerMoneyPerClickUpgradeLevel, 0, "=");
            gameDataService.GameData.isAutoClickerPurchased = false;

            SaveProgress();
        }

        private float ToFloat(double value)
        {
            return (float)value;
        }
        private int ToInt(double value)
        {
            return (int)value;
        }
    }
}