using UnityEngine;

namespace Tasks.Learning.Animator.ThreeD.S2.Systems.Door
{
    public class DoorAudioManager : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioSource audioSource;
        [SerializeField] private AudioClip openClip;
        [SerializeField] private AudioClip closeClip;
        [SerializeField] private DoorEvents doorEvents;

        // Lifecycle Methods
        private void OnEnable()
        {
            doorEvents.OnDoorOpened += OnDoorOpened;
            doorEvents.OnDoorClosed += OnDoorClosed;
        }
        private void OnDisable()
        {
            doorEvents.OnDoorOpened -= OnDoorOpened;
            doorEvents.OnDoorClosed -= OnDoorClosed;
        }

        // public Functional Methods
        public void UpdateVolume(float volume)
        {
            if (audioSource == null) return;

            audioSource.volume = volume;
        }

        // private Functional Methods
        private void OnDoorOpened()
        {
            if (audioSource == null || openClip == null) return;

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(openClip);
        }

        private void OnDoorClosed()
        {
            if (audioSource == null || closeClip == null) return;

            audioSource.pitch = Random.Range(0.9f, 1.1f);
            audioSource.PlayOneShot(closeClip);
        }
    }
}