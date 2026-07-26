// UnityEngine
using UnityEngine;

// Project
using ClickerGame.Scripts.src.Core.Services;

namespace ClickerGame
{
    public class MenuUI : MonoBehaviour
    {
        public SaveLoaderService saveLoaderService;

        public void OnProgressReset()
        {
            saveLoaderService.ResetProgress();
        }
        public void OnSceneLoad(string sceneName)
        {
            SceneController.Instance.LoadScene(sceneName);
        }
    }
}