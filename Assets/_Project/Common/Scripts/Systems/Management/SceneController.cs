using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    [SerializeField] private GlobalEvents _events;

    public static SceneController Instance;
    private  Stack<string> _history = new();

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
        SceneManager.sceneLoaded += OnSceneLoaded;
        _events.OnSceneLoad += LoadScene;
        _events.OnPreviousScene += LoadPreviousScene;
        _events.OnUnloadSceneAsync += UnloadSceneAsync;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        _events.OnSceneLoad -= LoadScene;
        _events.OnPreviousScene -= LoadPreviousScene;
        _events.OnUnloadSceneAsync -= UnloadSceneAsync;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (_history.Count == 0 && scene.name != "Bootstrap")
        {
            _history.Push(scene.name);
        }
    }

    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != "Bootstrap")
            _history.Push(currentScene);

        SceneManager.LoadScene(sceneName, mode);
    }

    public void LoadPreviousScene()
    {
        if (_history.Count == 0)
        {
            Debug.LogWarning("No previous scene to load");
            return;
        }

        string previousScene = _history.Pop();
        SceneManager.LoadScene(previousScene);
    }

    public async void UnloadSceneAsync(string sceneName)
    {
        if (SceneManager.GetSceneByName(sceneName).isLoaded)
        {
            await SceneManager.UnloadSceneAsync(sceneName);
        }
        else
        {
            Debug.LogWarning($"Scene {sceneName} is not loaded");
        }
    }
}