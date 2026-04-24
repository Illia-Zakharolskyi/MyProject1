using UnityEngine;

public class AppController : MonoBehaviour
{
    public static AppController Instance;

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

    public void Exit()
    {
        Application.Quit();
    }
}
