using UnityEngine;

namespace Projects.ApplePicker
{
    public class GameOverUI : MonoBehaviour
    {
        public void OnRestart()
        {
            SceneController.Instance.LoadScene("ApplePicker_GameLevel");
        }

        public void OnMainMenu()
        {
            SceneController.Instance.LoadScene("ApplePicker_MainMenu");
        }

    }
}
