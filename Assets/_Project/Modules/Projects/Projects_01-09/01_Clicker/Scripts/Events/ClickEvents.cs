// System
using System;

namespace Project_01_RefactoredClickerGame.Scripts.Events
{
    public class ClickEvents
    {
        public static event Action<double> OnBaseMoneyPerClickChanged;
        public static event Action<double> OnUpgradeBaseMoneyPerClickChanged;

        public static void InvokeOnBaseMoneyPerClickChanged(double amount)
        {
            OnBaseMoneyPerClickChanged?.Invoke(amount);
        }

        public static void InvokeOnUpgradeBaseMoneyPerClickChanged(double amount)
        {
            OnUpgradeBaseMoneyPerClickChanged?.Invoke(amount);
        }
    }
}