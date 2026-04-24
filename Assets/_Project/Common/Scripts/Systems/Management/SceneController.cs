using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections.Generic;

public class SceneController : MonoBehaviour
{
    public static SceneController Instance;
    private Stack<string> History = new Stack<string>();

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (History.Count == 0 && scene.name != "Bootstrap")
        {
            History.Push(scene.name);
        }
    }

    public void LoadScene(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
    {
        string currentScene = SceneManager.GetActiveScene().name;

        if (currentScene != "Bootstrap")
            History.Push(currentScene);

        SceneManager.LoadScene(sceneName, mode);
    }

    public void LoadPreviousScene()
    {
        if (History.Count == 0)
        {
            Debug.LogWarning("No previous scene to load");
            return;
        }

        string previousScene = History.Pop();
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
