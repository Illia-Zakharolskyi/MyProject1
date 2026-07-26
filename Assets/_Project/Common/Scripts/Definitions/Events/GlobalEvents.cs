using System;
using UnityEngine;
using UnityEngine.SceneManagement;

    [CreateAssetMenu(fileName = "Global_Events", menuName = "SO/Global_Events")]
    public class GlobalEvents : ScriptableObject
    {
        // app
        public event Action OnAppExit;

        // scenes
        public event Action<string, LoadSceneMode> OnSceneLoad;
        public event Action OnPreviousScene;
        public event Action<string> OnUnloadSceneAsync;

        // music
        public event Action<AudioClip> OnPlayMusic;
        public event Action OnStopMusic;
        public event Action<AudioClip> OnPlayOneShotSFXSound;
        public event Action<AudioClip> OnPlayOneShotUISound;

        // app
        public void InvokeAppExit()
        {
            OnAppExit?.Invoke();
        }

        // scenes
        public void InvokeSceneLoad(string sceneName, LoadSceneMode mode = LoadSceneMode.Single)
        {
            OnSceneLoad?.Invoke(sceneName, mode);
        }
        public void InvokePreviousScene()
        {
            OnPreviousScene?.Invoke();
        }
        public void InvokeUnloadSceneAsync(string sceneName)
        {
            OnUnloadSceneAsync?.Invoke(sceneName);
        }

        // music
        public void InvokePlayMusic(AudioClip clip)
        {
            OnPlayMusic?.Invoke(clip);
        }
        public void InvokeStopMusic()
        {
            OnStopMusic?.Invoke();
        }
        public void InvokeOneShotSFXSound(AudioClip clip)
        {
            OnPlayOneShotSFXSound?.Invoke(clip);
        }
        public void InvokeOneShotUISound(AudioClip clip)
        {
            OnPlayOneShotUISound?.Invoke(clip);
        }
    }