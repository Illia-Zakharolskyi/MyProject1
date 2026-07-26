using Project.TowerDefense.Runtime;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Project.TowerDefense.InTesting
{
    public class AudioManager : MonoBehaviour
    {
        [SerializeField] private GlobalEvents _globalEvents;
        [SerializeField] private GameEvents _gameEvents;

        [SerializeField] private AudioClip _onClickAudio;
        [SerializeField] private float _sameSoundCooldown = 0.05f;

        private float _lastTimeTowerBulletSoundPlayed;
        private TDInputActions _actions;

        void Awake()
        {
            _actions = new TDInputActions();
        }

        void OnEnable()
        {
            _actions.Enable();
            _actions.Player.Click.performed += PlayClickAudio;

            _gameEvents.OnOneShotSFXRequested += HandleOneShotSFXRequest;
        }

        void OnDisable()
        {
            _actions.Disable();
            _actions.Player.Click.performed -= PlayClickAudio;

            _gameEvents.OnOneShotSFXRequested -= HandleOneShotSFXRequest;
        }

        void PlayClickAudio(InputAction.CallbackContext cbc)
        {
            Debug.Log("Yes");

            _globalEvents.InvokeOneShotSFXSound(_onClickAudio);
        }

        void HandleOneShotSFXRequest(AudioClip audioClip, Runtime.AudioType audioType)
        {
            switch (audioType)
            {
                case Runtime.AudioType.TowerBullet:
                    if (Time.time - _lastTimeTowerBulletSoundPlayed > _sameSoundCooldown)
                    {
                        _globalEvents.InvokeOneShotSFXSound(audioClip);
                        _lastTimeTowerBulletSoundPlayed = Time.time;
                    }
                    return;

                default:
                    return;
            }
        }
    }
}
