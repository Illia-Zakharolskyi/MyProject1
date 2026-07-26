// UnityEngine using directives
using UnityEngine;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.BoxZone
{
    public class BoxZoneAudioManager : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private bool isLooped = true;
        [SerializeField] private bool playOnAwake = false;
        [SerializeField] private float spetialBlend = 1.0f;
        [SerializeField] private float minDistance = 1.5f;
        [SerializeField] private float maxDistance = 10f;

        // main private fields
        private AudioSource audioSource;

        // private fields
        private bool isPlaying = false;

        // Lifecycle Methods
        private void Awake()
        {
            audioSource = GetComponent<AudioSource>();
        }

        private void Start()
        {
            if (audioSource == null) return;
            if (spetialBlend < 0f || spetialBlend > 1f) spetialBlend = 1.00f;

            audioSource.loop = isLooped;
            audioSource.playOnAwake = playOnAwake;
            audioSource.spatialBlend = spetialBlend;
            audioSource.minDistance = minDistance;
            audioSource.maxDistance = maxDistance;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                if (isPlaying) return;

                audioSource.pitch = Random.Range(0.9f, 1.1f);
                audioSource.Play();
                isPlaying = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
            {
                audioSource.Stop();
                isPlaying = false;
            }
        }

        // public Functional Methods
        public void UpdateVolume(float volume)
        {
            audioSource.volume = volume;
        }
    }
}
