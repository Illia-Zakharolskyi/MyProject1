using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Projects.ApplePicker
{
    public class PauseManager : MonoBehaviour
    {
        public static PauseManager Instance;
        private bool _paused = false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        public void TogglePause()
        {
            if (_paused)
            {
                ResumeGame();
            }
            else if (!_paused)
            {
                PauseGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0.0f;
            SceneController.Instance.LoadScene("ApplePicker_PauseMenu", LoadSceneMode.Additive);
            _paused = true;
        }

        private void ResumeGame()
        {
            StartCoroutine(AfterDelay());
        }
        private IEnumerator AfterDelay()
        {
            SceneController.Instance.UnloadSceneAsync("ApplePicker_PauseMenu");
            _paused = false;
            yield return new WaitForSecondsRealtime(1f);

            Time.timeScale = 1.0f;
        }

        public void GoToMainMenu()
        {
            Time.timeScale = 1.0f;
            SceneController.Instance.LoadScene("ApplePicker_MainMenu");
        }
    }
}