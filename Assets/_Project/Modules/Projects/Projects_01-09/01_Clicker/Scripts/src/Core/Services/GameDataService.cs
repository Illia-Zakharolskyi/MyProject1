// Unity
using UnityEngine;
using TMPro;

// C# System
using System.Collections.Generic;

// Project
using ClickerGame.Scripts.Data.ScriptableObjects.GameData;
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.Core.Services
{
    public class GameDataService : MonoBehaviour
    {
        public GameData GameData;
        public TMP_Text MoneyText;
        public TMP_Text MoneyPerClickText;
        public TMP_Text ClickUpgradeCostText;
        public TMP_Text ClickerUpgradeCostText;
        public TMP_Text AutoClickerSpeedUpgradeText;
        public TMP_Text AutoClickerMoneyPerClickText;
        public TMP_Text AutoClickerMoneyPerClickUpgradeCostText;

        private Dictionary<GameDataKey, double> changeValueDictionary;
        private Dictionary<GameDataKey, TMP_Text> valueTextDictionary;
        private readonly List<string> _possibleOperations = new List<string>() { "+", "-", "=" };

        public void Awake()
        {
            AssignData();
            changeValueDictionary = new Dictionary<GameDataKey, double>()
            {
                { GameDataKey.MoneyCount, GameData.MoneyCount },
                { GameDataKey.MoneyPerClick, GameData.MoneyPerClick },
                { GameDataKey.ClickUpgradeCost, GameData.ClickUpgradeCost },
                { GameDataKey.AutoClickerCost, GameData.AutoClickerCost },
                { GameDataKey.AutoClickerSpeed, GameData.AutoClickerSpeed },
                { GameDataKey.AutoClickerSpeedUpgradeCost, GameData.AutoClickerSpeedUpgradeCost },
                { GameDataKey.MaxAutoClickerUpgradeSpeedLevel, GameData.MaxAutoClickerUpgradeSpeedLevel },
                { GameDataKey.AutoClickerPerLevelSpeedIncrease, GameData.AutoClickerPerLevelSpeedIncrease },
                { GameDataKey.AutoClickerMoneyPerClick, GameData.AutoClickerMoneyPerClick },
                { GameDataKey.AutoClickerMoneyPerClickUpgradeCost, GameData.AutoClickerMoneyPerClickUpgradeCost },
                { GameDataKey.AutoClickerMaxMoneyPerClickUpgradeLevel, GameData.AutoClickerMaxMoneyPerClickUpgradeLevel },
                { GameDataKey.AutoClickerMoneyPerClickPerLevelUpgradeIncrease, GameData.AutoClickerMoneyPerClickPerLevelUpgradeIncrease },
                { GameDataKey.CurrentAutoClickerMoneyPerClickUpgradeLevel, GameData.currentAutoClickerMoneyPerClickUpgradeLevel },
                { GameDataKey.CurrentAutoClickerSpeedUpgradeLevel, GameData.currentAutoClickerSpeedUpgradeLevel }
            };
            valueTextDictionary = new Dictionary<GameDataKey, TMP_Text>()
            {
                { GameDataKey.MoneyCount, GameData.MoneyText },
                { GameDataKey.MoneyPerClick, GameData.MoneyPerClickText },
                { GameDataKey.ClickUpgradeCost, GameData.ClickUpgradeCostText },
                { GameDataKey.AutoClickerCost, GameData.AutoClickerCostText },
                { GameDataKey.AutoClickerSpeedUpgradeCost, GameData.AutoClickerUpgradeSpeedCostText },
                { GameDataKey.AutoClickerMoneyPerClick, GameData.AutoClickerMoneyPerClickText },
                { GameDataKey.AutoClickerMoneyPerClickUpgradeCost, GameData.AutoClickerMoneyPerClickUpgradeCostText }
            };
        }
        public void AssignData()
        {
            GameData.MoneyText = MoneyText;
            GameData.MoneyPerClickText = MoneyPerClickText;
            GameData.ClickUpgradeCostText = ClickUpgradeCostText;
            GameData.AutoClickerCostText = ClickerUpgradeCostText;
            GameData.AutoClickerUpgradeSpeedCostText = AutoClickerSpeedUpgradeText;
            GameData.AutoClickerMoneyPerClickText = AutoClickerMoneyPerClickText;
            GameData.AutoClickerMoneyPerClickUpgradeCostText = AutoClickerMoneyPerClickUpgradeCostText;
        }

        public void ChangeText(GameDataKey valueName, string newValue)
        {
            if (!ChangeTextCriticalValidator(valueName, newValue))
            {
                Debug.LogError("Invalid parameters for class GameDataService.cs, ChangeText(string, string)");
                return;
            }

            valueTextDictionary[valueName].text = newValue;
        }

        public void ChangeValue(GameDataKey valueName, double value, string operation)
        {
            if (!ChangeValueCriticalValidator(valueName, value, operation))
            {
                Debug.LogError("Invalid parameters for Class GameDataService.cs, ChangeValue(string, int, string)");
                return;
            }
            if (!ChangeValueNonCricialValidator(valueName, value, operation))
            {
                return;
            }

            switch (operation)
            {
                case "+":
                    changeValueDictionary[valueName] += value;
                    break;
                case "-":
                    changeValueDictionary[valueName] -= value;
                    break;
                case "=":
                    changeValueDictionary[valueName] = value;
                    break;
            }

            switch (valueName)
            {
                case GameDataKey.MoneyCount:
                    GameEvents.InvokeMoneyChanged(changeValueDictionary[valueName]);
                    break;
                case GameDataKey.MoneyPerClick:
                    GameEvents.InvokeMoneyPerClickChanged(changeValueDictionary[valueName]);
                    break;
                case GameDataKey.ClickUpgradeCost:
                    GameEvents.InvokeUpgradeMoneyPerClickChanged(changeValueDictionary[valueName]);
                    break;
                case GameDataKey.AutoClickerSpeedUpgradeCost:
                    GameEvents.InvokeAutoClickerSpeedUpgradeCostChanged(changeValueDictionary[valueName]);
                    break;
                case GameDataKey.AutoClickerMoneyPerClick:
                    GameEvents.InvokeAutoClickerMoneyPerClickUpgradeCostChanged(changeValueDictionary[valueName]);
                    break;
                case GameDataKey.AutoClickerMoneyPerClickUpgradeCost:
                    GameEvents.InvokeAutoClickerMoneyPerClickChanged((changeValueDictionary[valueName]));
                    break;
            }
        }
        public double? GetValue(GameDataKey valueName)
        {
            if (!GetValueCriticalValidator(valueName))
            {
                Debug.LogError("Invalid parameters for GameDataService.cs, GetValue(string)");
                return null;
            }

            return changeValueDictionary[valueName];
        }
        // The game is not working as intended
        private bool ChangeValueCriticalValidator(GameDataKey valueName, double value, string operation)
        {
            // Is the value name null or empty?
            if (value < 0) { return false; }
            if (string.IsNullOrEmpty(operation)) { return false; }

            // Is the operation valid?
            if (!changeValueDictionary.ContainsKey(valueName)) { return false; }
            if (!_possibleOperations.Contains(operation)) { return false; }

            return true;
        }
        // player can see warnings but the game can still work
        private bool ChangeValueNonCricialValidator(GameDataKey valueName, double value, string operation)
        {
           if (changeValueDictionary[valueName] < value && operation == "-")
           {
                Debug.LogWarning($"Not enough {valueName} to perform the operation. Current {valueName}: {changeValueDictionary[valueName]}, required: {value}");
                return false;
           }

           return true;
        }
        private bool ChangeTextCriticalValidator(GameDataKey valueName, string newValue)
        {
            if (string.IsNullOrEmpty(newValue)) { return false; }
            if (!valueTextDictionary.ContainsKey(valueName)) { return false; }
            return true;
        }
        private bool GetValueCriticalValidator(GameDataKey valueName)
        {
            if (!changeValueDictionary.ContainsKey(valueName)) return false;

            return true;
        }
    }
}