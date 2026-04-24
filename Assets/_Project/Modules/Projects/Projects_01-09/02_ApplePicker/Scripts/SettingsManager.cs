using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private Slider masterVolumeSlider;
    [SerializeField] private Slider musicVolumeSlider;
    [SerializeField] private Slider sfxVolumeSlider;
    [SerializeField] private Slider uiVolumeSlider;

    private void OnEnable()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVol", 50);
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVol", 50);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVol", 50);
        float savedUIVolume = PlayerPrefs.GetFloat("UIVol", 50);

        masterVolumeSlider.value = savedMasterVolume;
        musicVolumeSlider.value = savedMusicVolume;
        sfxVolumeSlider.value = savedSFXVolume;
        uiVolumeSlider.value = savedUIVolume;
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

    public void SetMasterVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
        mainMixer.SetFloat("MasterVol", dbValue);
        PlayerPrefs.SetFloat("MasterVol", volume);
    }

    public void SetMusicVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
        mainMixer.SetFloat("MusicVol", dbValue);
        PlayerPrefs.SetFloat("MusicVol", volume);
    }

    public void SetSFXVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
        mainMixer.SetFloat("SFXVol", dbValue);
        PlayerPrefs.SetFloat("SFXVol", volume);
    }

    public void SetUIVolume(float volume)
    {
        float dbValue = Mathf.Log10(Mathf.Max(0.0001f, volume)) * 20;
        mainMixer.SetFloat("UIVol", dbValue);
        PlayerPrefs.SetFloat("UIVol", volume);
    }
}
