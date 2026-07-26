// Unity 
using UnityEngine;

// C# System
using System;

// Project
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.UI.Services
{
    public class UIService : MonoBehaviour
    {
        public GameDataService GameDataService;

        private void OnEnable()
        {
            GameEvents.OnMoneyChanged += UpdateMoneyUI;
            GameEvents.OnMoneyPerClickChanged += UpdateMoneyPerClickUI;
            GameEvents.OnUpgradeMoneyPerClickChanged += UpdateClickUpgradeCostUI;
            GameEvents.OnAutoClickerMoneyChanged += UpdateAutoClickerCostUI;
            GameEvents.OnAutoClickerMoneyPerClickChanged += UpdateAutoClickerMoneyPerClickUI;
            GameEvents.OnAutoClickerMoneyPerClickUpgradeCostChanged += UpdateAutoClickerMoneyPerClickUpgradeCostUI;
            GameEvents.OnAutoClickerSpeedUpgradeCostChanged += UpdateAutoClickerUpgradeSpeedCostUI;
        }
        private void OnDisable()
        {
            GameEvents.OnMoneyChanged -= UpdateMoneyUI;
            GameEvents.OnMoneyPerClickChanged -= UpdateMoneyPerClickUI;
            GameEvents.OnUpgradeMoneyPerClickChanged -= UpdateClickUpgradeCostUI;
            GameEvents.OnAutoClickerMoneyChanged -= UpdateAutoClickerCostUI;
            GameEvents.OnAutoClickerMoneyPerClickChanged -= UpdateAutoClickerMoneyPerClickUI;
            GameEvents.OnAutoClickerMoneyPerClickUpgradeCostChanged -= UpdateAutoClickerMoneyPerClickUpgradeCostUI;
            GameEvents.OnAutoClickerSpeedUpgradeCostChanged -= UpdateAutoClickerUpgradeSpeedCostUI;
        }

        private void Awake()
        {
            GameDataService.AssignData();
            GameDataService.ChangeText(GameDataKey.MoneyCount, $"Money: {GameDataService.GetValue(GameDataKey.MoneyCount)}");
            GameDataService.ChangeText(GameDataKey.MoneyPerClick, $"Money Per Click: {GameDataService.GetValue(GameDataKey.MoneyPerClick)}");
            GameDataService.ChangeText(GameDataKey.ClickUpgradeCost, $"Next Upgrade Cost: {GameDataService.GetValue(GameDataKey.ClickUpgradeCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerCost, $"Auto Clicker Cost: {GameDataService.GetValue(GameDataKey.AutoClickerCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerSpeedUpgradeCost, $"Auto Clicker Upgrade Speed Cost: {GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClick, $"Auto Clicker Money Per Click: {GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClick)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, $"Auto Clicker Money Per Click Upgrade Cost: {GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost)}");
        }
        private void Start()
        {
            GameDataService.AssignData();
            GameDataService.ChangeText(GameDataKey.MoneyCount, $"Money: {GameDataService.GetValue(GameDataKey.MoneyCount)}");
            GameDataService.ChangeText(GameDataKey.MoneyPerClick, $"Money Per Click: {GameDataService.GetValue(GameDataKey.MoneyPerClick)}");
            GameDataService.ChangeText(GameDataKey.ClickUpgradeCost, $"Next Upgrade Cost: {GameDataService.GetValue(GameDataKey.ClickUpgradeCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerCost, $"Auto Clicker Cost: {GameDataService.GetValue(GameDataKey.AutoClickerCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerSpeedUpgradeCost, $"Auto Clicker Upgrade Speed Cost: {GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClick, $"Auto Clicker Money Per Click: {GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClick)}");
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, $"Auto Clicker Money Per Click Upgrade Cost: {GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost)}");
        }

        public void AssignUI()
        {
            UpdateMoneyUI((double)GameDataService.GetValue(GameDataKey.MoneyCount));
            UpdateMoneyPerClickUI((double)GameDataService.GetValue(GameDataKey.MoneyPerClick));
            UpdateClickUpgradeCostUI((double)GameDataService.GetValue(GameDataKey.ClickUpgradeCost));
            UpdateAutoClickerCostUI((double)GameDataService.GetValue(GameDataKey.AutoClickerCost));
            UpdateAutoClickerUpgradeSpeedUI((double)GameDataService.GetValue(GameDataKey.AutoClickerSpeedUpgradeCost));
            UpdateAutoClickerMoneyPerClickUI((double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClick));
            UpdateAutoClickerMoneyPerClickUpgradeCostUI((double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClickUpgradeCost));
        }
        private void UpdateAutoClickerUpgradeSpeedCostUI(double value)
        {
            AutoClickerUpgradeSpeedTextUpdate(value);
        }
        private void UpdateAutoClickerMoneyPerClickUI(double newMoneyCount)
        {
            AutoClickerMoneyPerClickTextUpdate(newMoneyCount);
        }
        private void UpdateAutoClickerMoneyPerClickUpgradeCostUI(double newMoneyCount)
        {
            AutoClickerMoneyPerClickUpgradeCostTextUpdate(newMoneyCount);
        }
        private void UpdateMoneyUI(double newMoneyCount)
        {
            CoinCountTextUpdate(newMoneyCount);
        }
        private void UpdateMoneyPerClickUI(double newMoneyCount)
        {
            MoneyPerClickTextUpdate(newMoneyCount);
        }
        private void UpdateClickUpgradeCostUI(double newMoneyCount)
        {
            ClickUpgradeCostTextUpdate(newMoneyCount);
        }
        private void UpdateAutoClickerCostUI(double newMoneyCount)
        {
            AutoClickerCostTextUpdate(newMoneyCount);
        }
        private void UpdateAutoClickerUpgradeSpeedUI(double newMoneyCount)
        {
            AutoClickerUpgradeSpeedTextUpdate(newMoneyCount);
        }
        private void CoinCountTextUpdate(double? newCoinCount)
        {
            GameDataService.ChangeText(GameDataKey.MoneyCount, $"Money: {Format2F(newCoinCount)}");
        }

        private void MoneyPerClickTextUpdate(double? newMoneyPerClick)
        {
            GameDataService.ChangeText(GameDataKey.MoneyPerClick, $"Money Per Click: {Format2F(newMoneyPerClick)}");
        }

        private void ClickUpgradeCostTextUpdate(double? clickUpgradeCost)
        {
            GameDataService.ChangeText(GameDataKey.ClickUpgradeCost, $"Upgrade cost: {Format2F(clickUpgradeCost)}");
        }

        private void AutoClickerCostTextUpdate(double? autoClickerCost)
        {
            GameDataService.ChangeText(GameDataKey.AutoClickerCost, $"Auto Clicker cost: {Format2F(autoClickerCost)}");
        }

        private void AutoClickerUpgradeSpeedTextUpdate(double? autoClickerUpgradeSpeed)
        {
            GameDataService.ChangeText(GameDataKey.AutoClickerSpeedUpgradeCost, $"Auto Clicker Upgrade Speed Cost: {Format2F(autoClickerUpgradeSpeed)}");
        }

        private void AutoClickerMoneyPerClickTextUpdate(double? autoClickerMoneyPerClick)
        {
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClick, $"Auto Clicker Money Per Click: {Format2F(autoClickerMoneyPerClick)}");
        }

        private void AutoClickerMoneyPerClickUpgradeCostTextUpdate(double? autoClickerMoneyPerClickUpgradeCost)
        {
            GameDataService.ChangeText(GameDataKey.AutoClickerMoneyPerClickUpgradeCost, $"Auto Clicker Money Per Click Upgrade Cost: {Format2F(autoClickerMoneyPerClickUpgradeCost)}");
        }
        private string Format2F(double? value)
        {
            string formattedValue = value?.ToString("F2") ?? "";
            return formattedValue;
        }
    }
}