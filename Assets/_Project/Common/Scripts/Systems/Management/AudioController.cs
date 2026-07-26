using UnityEngine;

public class AudioController : MonoBehaviour
{
    [SerializeField] private GlobalEvents _events;
    [SerializeField] private float _minSFXPitch = 0.9f;
    [SerializeField] private float _maxSFXPitch = 1.1f;

    [Header("Outputs")]
    [SerializeField] private AudioSource musicSource;
    [SerializeField] private AudioSource sfxSource;
    [SerializeField] private AudioSource uiSource;

    public static AudioController Instance;

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
        _events.OnPlayMusic += PlayMusic;
        _events.OnStopMusic += StopMusic;
        _events.OnPlayOneShotSFXSound += PlayOneShotSFXSound;
        _events.OnPlayOneShotUISound += PlayOneShotUISound;
    }
    private void OnDisable()
    {
        _events.OnPlayMusic -= PlayMusic;
        _events.OnStopMusic -= StopMusic;
        _events.OnPlayOneShotSFXSound -= PlayOneShotSFXSound;
        _events.OnPlayOneShotUISound -= PlayOneShotUISound;
    }

    public void PlayMusic(AudioClip clip)
    {
        if (musicSource.clip == clip && musicSource.isPlaying) return;

        musicSource.clip = clip;
        musicSource.loop = true;
        musicSource.Play();
    }

    public void StopMusic()
    {
        if (musicSource.isPlaying) musicSource.Stop();
    }

    public void PlayOneShotSFXSound(AudioClip clip)
    {
        sfxSource.pitch = Random.Range(_minSFXPitch, _maxSFXPitch);
        sfxSource.PlayOneShot(clip);
    }

    public void PlayOneShotUISound(AudioClip clip)
    {
        uiSource.PlayOneShot(clip);
    }
}
