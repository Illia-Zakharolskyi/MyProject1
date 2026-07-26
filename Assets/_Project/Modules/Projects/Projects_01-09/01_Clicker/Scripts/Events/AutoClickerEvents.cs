// System
using System;

public class AutoClickerEvents
{
    // void Actions
    public static event Action OnAutoClickerBought;

    // Values Changed
    public static event Action<double> OnAutoClickerBaseMoneyChanged;

    // Per click values changed
    public static event Action<double> OnAutoClickerBaseMoneyPerClickChanged;
    public static event Action<double> OnAutoClickerBaseMoneyPerClickUpgradeCostChanged;

    // Something worked
    public static event Action<double> OnAutoClickerWorked;

    public static void InvokeOnAutoClickerBought()
    {
        OnAutoClickerBought?.Invoke();
    }
    public static void InvokeOnAutoClickerBaseMoneyChanged(double amount)
    {
        OnAutoClickerBaseMoneyChanged?.Invoke(amount);
    }
    public static void InvokeOnAutoClickerBaseMoneyPerClickChanged(double amount)
    {
        OnAutoClickerBaseMoneyPerClickChanged?.Invoke(amount);
    }
    public static void InvokeOnAutoClickerBaseMoneyPerClickUpgradeCostChanged(double amount)
    {
        OnAutoClickerBaseMoneyPerClickUpgradeCostChanged?.Invoke(amount);
    }
    public static void InvokeOnAutoClickerWorked(double amount)
    {
        OnAutoClickerWorked?.Invoke(amount);
    }
}