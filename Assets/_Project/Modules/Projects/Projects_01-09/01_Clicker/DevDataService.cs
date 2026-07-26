// UnityEngine
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Core.Services;
using ClickerGame.Scripts.Data.Enums.Keys;

namespace ClickerGame
{
    public class DevDataService : MonoBehaviour
    {
        public GameDataService gameDataService;
        public SaveLoaderService saveLoaderService;

#if UNITY_EDITOR
        [ContextMenu("Give 100000 Money")]
        public void GiveMoney()
        {
            ChangeFloatValue(GameDataKey.MoneyCount, 100000.00f);
            Debug.Log("Dev: Money set to 100000");
        }
#endif

        private void ChangeFloatValue(GameDataKey valueName, float newAmount)
        {
            gameDataService.ChangeValue(valueName, newAmount, "=");
            saveLoaderService.SaveProgress();
        }
    }
}