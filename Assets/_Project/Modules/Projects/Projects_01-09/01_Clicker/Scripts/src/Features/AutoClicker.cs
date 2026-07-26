// Unity
using UnityEngine;

// C# System
using System.Collections;

// Project
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame.Scripts.src.Features
{
    public class AutoClicker : MonoBehaviour
    {
        private Coroutine autoClickerCoroutine;
        public GameDataService GameDataService;
        private void OnEnable()
        {
            GameEvents.OnAutoClickerBought += StartAutoClicker;
        }
        private void OnDisable()
        {
            GameEvents.OnAutoClickerBought -= StartAutoClicker;
        }
        private void Start()
        {
            if (GameDataService.GameData.isAutoClickerPurchased)
            {
                StartAutoClicker();
            }
        }
        private void StartAutoClicker()
        {
            if (autoClickerCoroutine != null)
                return;

            GameDataService.GameData.isAutoClickerPurchased = true;
            autoClickerCoroutine = StartCoroutine(AutoClickerRoutine());
        }
        private IEnumerator AutoClickerRoutine()
        {
            while (true)
            {
                GameEvents.InvokeAutoClickerWorked((double)GameDataService.GetValue(GameDataKey.AutoClickerMoneyPerClick));
                double autoClickerSpeed = (double)GameDataService.GetValue(GameDataKey.AutoClickerSpeed);
                float result = (float)autoClickerSpeed;
                yield return new WaitForSeconds(result);
            }
        }
    }
}