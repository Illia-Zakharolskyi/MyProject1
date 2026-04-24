using UnityEngine;
using UnityEngine.SceneManagement;

public class BootstrapLoader : MonoBehaviour
{
    public SceneController controller;

    void Start()
    {
        controller.LoadScene("ApplePicker_MainMenu");
    }
}
