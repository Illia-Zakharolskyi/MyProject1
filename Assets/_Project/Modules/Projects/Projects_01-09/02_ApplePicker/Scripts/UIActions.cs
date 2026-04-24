using UnityEngine;

namespace Projects.ApplePicker
{
    public class UIActions : MonoBehaviour
    {

        public void QuitGame()
        {
            AppController.Instance.Exit();
        }

        public void LoadScene(string sceneName)
        {
            SceneController.Instance.LoadScene(sceneName);
        }

        public void LoadPreviousScene()
        {
            SceneController.Instance.LoadPreviousScene();
        }

        public void TogglePause()
        {
            PauseManager.Instance.TogglePause();
        }
        public void GoToMainMenu()
        {
            PauseManager.Instance.GoToMainMenu();
        }
    }
}