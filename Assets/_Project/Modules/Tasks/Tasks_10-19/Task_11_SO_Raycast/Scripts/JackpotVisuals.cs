using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Tasks.SORaycast
{
    public class JackpotVisuals : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Transform _root;

        private List<Transform> _toRotate;
        private List<Transform> _toShake;
        private EventBus _bus;

        private void Awake()
        {
            InitAddressablesAsync();
        }

        private void Start()
        {
            _root.gameObject.SetActive(false);
            _toRotate = null;
            _toShake = null;
        }

        private void OnDestroy()
        {
            if (_bus != null)
            {
                _bus.OnJackpot -= Rotate;
            }
        }

        private async void InitAddressablesAsync()
        {
            _bus = await Addressables.LoadAssetAsync<EventBus>("Task_SORaycast_Event_Bus").Task;
            _bus.OnJackpot += Rotate;
        }

        private async void Rotate()
        {
            _root.gameObject.SetActive(true);

            if (_toRotate == null)
            {
                _toRotate = new List<Transform>();
                _toShake = new List<Transform>();

                Transform rot = _root.Find("Rotation");
                Transform shake = _root.Find("Shake");

                foreach (Transform t in rot)
                {
                    _toRotate.Add(t);
                }
                foreach (Transform t in shake)
                {
                    _toShake.Add(t);
                }
            }

            foreach (Transform child in _toRotate)
            {
                child.DORotate(new Vector3(0, 0, -360), 3.5f, RotateMode.FastBeyond360)
                    .SetEase(Ease.Linear)
                    .SetLink(child.gameObject);
            }
            foreach (Transform child in _toShake)
            {
                Sequence movSequence = DOTween.Sequence();
                movSequence.Append(child.DORotate(new Vector3(0, 0, 15), 0.5f));
                movSequence.Append(child.DORotate(new Vector3(0, 0, -15), 0.5f));
                movSequence.SetLoops(3);
                movSequence.SetLink(child.gameObject);
            }

            await Task.Delay(3500);
            _root.gameObject.SetActive(false);
        }
    }
}