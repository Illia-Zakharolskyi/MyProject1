using UnityEngine;
using UnityEngine.Audio;

public class MainMenu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private AudioMixer mainMixer;

    private void Start()
    {
        float savedMasterVolume = PlayerPrefs.GetFloat("MasterVol", 0.50f);
        float savedMusicVolume = PlayerPrefs.GetFloat("MusicVol", 0.50f);
        float savedSFXVolume = PlayerPrefs.GetFloat("SFXVol", 0.50f);
        float savedUIVolume = PlayerPrefs.GetFloat("UIVol", 0.50f);

        float dbMasterVolume = Mathf.Log10(Mathf.Max(0.0001f, savedMasterVolume)) * 20;
        float dbMusicVolume = Mathf.Log10(Mathf.Max(0, 0001f, savedMusicVolume)) * 20;
        float dbSFXVolume = Mathf.Log10(Mathf.Max(0, 0001f, savedSFXVolume)) * 20;
        float dbUIVolume = Mathf.Log10(Mathf.Max(0, 0001f, savedUIVolume)) * 20;

        mainMixer.SetFloat("MasterVol", dbMasterVolume);
        mainMixer.SetFloat("MusicVol", dbMusicVolume);
        mainMixer.SetFloat("SFXVol", dbSFXVolume);
        mainMixer.SetFloat("UIVol", dbUIVolume);
    }
}
