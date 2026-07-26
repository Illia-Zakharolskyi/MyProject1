// Unity engine
using ClickerGame.Scripts.Data.Enums.Keys;
using ClickerGame.Scripts.Events;
using ClickerGame.Scripts.src.Core.Services;
using UnityEngine;

namespace ClickerGame
{
    public class AnimationService : MonoBehaviour
    {
        public GameDataService GameDataService;
        public GameObject floatingTextPrefab;
        public Transform startPosition;
        public Transform CanvasTransform;

        private void OnEnable()
        {
            GameEvents.OnMoneyChanged += ShowFloatingText;
        }
        private void OnDisable()
        {
            GameEvents.OnMoneyChanged -= ShowFloatingText;
        }

        private void ShowFloatingText(double value)
        {
            GameObject floatingText = Instantiate(floatingTextPrefab, startPosition.position, Quaternion.identity, CanvasTransform);
            FloatingText floatigTextComponent = floatingText.GetComponent<FloatingText>();
            double moneyPerClick = (double)GameDataService.GetValue(GameDataKey.MoneyPerClick);
            string moneyPerClickText = moneyPerClick.ToString("F0");
            floatigTextComponent.text.text = "+" + moneyPerClickText;
        }
    }
}