using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;

namespace Project.TowerDefense.Runtime.UI
{
    public class MainMenuManager : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private GlobalEvents _events;
        [SerializeField] private GameObject _mainMenuPanel;
        [SerializeField] private GameObject _optionsPanel;
        [SerializeField] private GameObject _creditsPanel;
        [SerializeField] private AudioClip _backgroundMusic;
        [SerializeField] private GlobalEvents _globEvents;
        [SerializeField] private AudioMixer _mainMixer;

        private const string MasterVolKey = "TowerMasterVol";
        private const string MusicVolKey = "TowerMusicVol";
        private const string SFXVolKey = "TowerSFXVol";

        void Start()
        {
            float savedMasterVolume = PlayerPrefs.GetFloat(MasterVolKey, 0.8f);
            float masterDb = Mathf.Log10(Mathf.Max(0.0001f, savedMasterVolume)) * 20;

            float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolKey, 0.4f);
            float musicDb = Mathf.Log10(Mathf.Max(0.0001f, savedMusicVolume)) * 20;

            float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolKey, 0.6f);
            float sfxDb = Mathf.Log10(Mathf.Max(0.0001f, savedSFXVolume)) * 20;

            _mainMixer.SetFloat(MasterVolKey, masterDb);
            _mainMixer.SetFloat(MusicVolKey, musicDb);
            _mainMixer.SetFloat(SFXVolKey, sfxDb);
        }

        void OnEnable()
        {
            if (_backgroundMusic != null) _globEvents.InvokePlayMusic(_backgroundMusic);
        }

        void OnDisable()
        {
            _globEvents.InvokeStopMusic();
        }

        public void OnPlay()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("TowerDefense_Game");
        }

        public void OnOptions()
        {
            _mainMenuPanel.SetActive(false);
            _optionsPanel.SetActive(true);
        }

        public void OnMainMenu()
        {
            _mainMenuPanel.SetActive(true);
            _optionsPanel.SetActive(false);
            _creditsPanel.SetActive(false);
        }

        public void OnExit()
        {
            _events.InvokeAppExit();
        }

        public void OnCredits()
        {
            _creditsPanel.SetActive(true);
            _mainMenuPanel.SetActive(false);
        }
    }
}
