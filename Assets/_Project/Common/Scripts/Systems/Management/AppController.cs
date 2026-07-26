using UnityEngine;

public class AppController : MonoBehaviour
{
    [SerializeField] private GlobalEvents _events;

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

    private void OnEnable()
    {
        _events.OnAppExit += Exit;
    }
    private void OnDisable()
    {
        _events.OnAppExit -= Exit;
    }

    public void Exit()
    {
        Application.Quit();
    }
}
