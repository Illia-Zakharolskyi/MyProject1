using UnityEngine;
using UnityEngine.UI;

namespace Projects.ApplePicker
{
    [RequireComponent(typeof(Button))]
    public class ButtonSoundUI : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private AudioClip clip;

        private void Awake()
        {
            GetComponent<Button>().onClick.AddListener(PlaySound);
        }

        private void PlaySound()
        {
            if (clip == null)
            {
                Debug.Log("Audio Clip is missing on " + gameObject.name);
                return;
            }

            AudioController.Instance.PlayOneShotUISound(clip);
        }
    }
}