// C# System
using System;

namespace ClickerGame.Scripts.Events
{
    public class GameEvents
    {
        public static event Action<double> OnMoneyChanged;
        public static event Action<double> OnMoneyPerClickChanged;
        public static event Action<double> OnUpgradeMoneyPerClickChanged;
        public static event Action<double> OnAutoClickerMoneyChanged;
        public static event Action OnAutoClickerBought;
        public static event Action<double> OnAutoClickerWorked;
        public static event Action<double> OnAutoClickerMoneyPerClickChanged;
        public static event Action<double> OnAutoClickerMoneyPerClickUpgradeCostChanged;
        public static event Action<double> OnAutoClickerSpeedUpgradeCostChanged;
        public static void InvokeMoneyChanged(double newMoneyCount)
        {
            OnMoneyChanged?.Invoke(newMoneyCount);
        }
        public static void InvokeMoneyPerClickChanged(double newMoneyCount)
        {
            OnMoneyPerClickChanged?.Invoke(newMoneyCount);
        }
        public static void InvokeUpgradeMoneyPerClickChanged(double newMoneyCount)
        {
            OnUpgradeMoneyPerClickChanged?.Invoke(newMoneyCount);
        }
        public static void InvokeAutoClickerBought()
        {
            OnAutoClickerBought?.Invoke();
        }
        public static void InvokeAutoClickerWorked(double amount)
        {
            OnAutoClickerWorked?.Invoke(amount);
        }
        public static void InvokeAutoClickerMoneyChanged(double newMoneyCount)
        {
            OnAutoClickerMoneyChanged?.Invoke(newMoneyCount);
        }

        public static void InvokeAutoClickerMoneyPerClickChanged(double newMoneyCount)
        {
            OnAutoClickerMoneyPerClickChanged?.Invoke(newMoneyCount);
        }

        public static void InvokeAutoClickerMoneyPerClickUpgradeCostChanged(double newMoneyCount)
        {
            OnAutoClickerMoneyPerClickUpgradeCostChanged?.Invoke(newMoneyCount);
        }

        public static void InvokeAutoClickerSpeedUpgradeCostChanged(double amount)
        {
            OnAutoClickerSpeedUpgradeCostChanged?.Invoke(amount);
        }
    }
}