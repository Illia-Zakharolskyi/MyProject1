using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

namespace Project.TowerDefense.Runtime
{
    public class SettingsManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioMixer mainMixer;
        [SerializeField] private Slider masterVolumeSlider;
        [SerializeField] private Slider musicVolumeSlider;
        [SerializeField] private Slider sfxVolumeSlider;

        private const string MasterVolKey = "TowerMasterVol";
        private const string MusicVolKey = "TowerMusicVol";
        private const string SFXVolKey = "TowerSFXVol";

        private void OnEnable()
        {
            // Припускаємо, що слайдер в Unity налаштований від 0.0001 до 1
            // Значення за замовчуванням — 0.5f (50% гучності)
            float savedMasterVolume = PlayerPrefs.GetFloat(MasterVolKey, 0.5f);
            float savedMusicVolume = PlayerPrefs.GetFloat(MusicVolKey, 0.4f);
            float savedSFXVolume = PlayerPrefs.GetFloat(SFXVolKey, 0.6f);

            masterVolumeSlider.value = savedMasterVolume;
            musicVolumeSlider.value = savedMusicVolume;
            sfxVolumeSlider.value = savedSFXVolume;

            // Одразу застосовуємо гучність до міксера при старті
            ApplyVolume(MasterVolKey, savedMasterVolume);
            ApplyVolume(MusicVolKey, savedMusicVolume);
            ApplyVolume(SFXVolKey, savedSFXVolume);

            // Підписуємося на зміну слайдера через код (так надійніше, ніж через інспектор)
            masterVolumeSlider.onValueChanged.AddListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
        }

        private void OnDisable()
        {
            masterVolumeSlider.onValueChanged.RemoveListener(SetMasterVolume);
            musicVolumeSlider.onValueChanged.RemoveListener(SetMusicVolume);
            sfxVolumeSlider.onValueChanged.RemoveListener(SetSFXVolume);

            PlayerPrefs.Save();
        }

        public void SetMasterVolume(float volume)
        {
            ApplyVolume(MasterVolKey, volume);
            PlayerPrefs.SetFloat(MasterVolKey, volume);
        }

        public void SetMusicVolume(float volume)
        {
            ApplyVolume(MusicVolKey, volume);
            PlayerPrefs.SetFloat(MusicVolKey, volume);
        }

        public void SetSFXVolume(float volume)
        {
            ApplyVolume(SFXVolKey, volume);
            PlayerPrefs.SetFloat(SFXVolKey, volume);
        }

        // Окремий метод для зміни гучності в міксері
        private void ApplyVolume(string parameterName, float volume)
        {
            float dbValue = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
            Debug.Log($"Намагаємось встановити {parameterName} на {dbValue} dB (значення слайдера: {volume})");
            mainMixer.SetFloat(parameterName, dbValue);

            bool result = mainMixer.SetFloat(parameterName, dbValue);
            if (!result)
            {
                Debug.LogError($"ПОМИЛКА: Не вдалося знайти параметр {parameterName} у міксері! Перевірте Exposed Parameters.");
            }
        }
    }
}
