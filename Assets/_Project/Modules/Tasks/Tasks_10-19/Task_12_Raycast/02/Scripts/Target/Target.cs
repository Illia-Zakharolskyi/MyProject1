using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Task.Raycast.Second
{
    public class Target : MonoBehaviour
    {
        [Header("Refs")]
        [Tooltip("Parent for object creating")]
        [SerializeField] private Transform _parent;

        [Tooltip("prefabs that appear when player shoot the target with raycast")]
        [SerializeField] private GameObject[] _hitPrefabs;

        [Tooltip("prefabs that appear when player shoot the center of target with raycast")]
        [SerializeField] private GameObject[] _bullseyePrefabs;

        [Header("Settings")]
        [Tooltip("duration of life for objects that appear when player shoot the target")]
        [SerializeField] private float _hitDuration = 1f;

        [Tooltip("duration of life for objects that appear when player shoot the center of target")]
        [SerializeField] private float _bullseyeDuration = 0.5f;

        [SerializeField] private TargetLogic _logic = TargetLogic.Linear;

        [SerializeField] private float _movSpeed = 0f;
        [SerializeField] private float _moveAmplitude = 0f;
        [SerializeField] private float _jumpHeight = 3f;
        

        public float MoveAmplitude => _moveAmplitude;

        private EventBus _bus;
        private MeshRenderer[] _textRenders;
        private bool _subscribed = false;
        private Vector3 _startLocalPosition;
        private ITargetMovementLogic _movementLogic;

        private void Awake()
        {
            InitAddressables();

            _startLocalPosition = transform.localPosition;

            _movementLogic = _logic switch
            {
                TargetLogic.Linear => new TargetLinearMovementLogic(),
                TargetLogic.Arc => new TargetArcMovementLogic(),
                TargetLogic.PerlinChaos => new TargetPerlinChaosMovementLogic(),
                TargetLogic.Lissajous => new TargetLissajousMovementLogic(),
                TargetLogic.Dash => new TargetDashMovementLogic(),
                _ => new TargetLinearMovementLogic()
            };

            TMP_Text[] texts = GetComponentsInChildren<TMP_Text>(true);
            List<MeshRenderer> renders = new();
            foreach (var text in texts)
            {
                if (!text.gameObject.TryGetComponent<MeshRenderer>(out var render)) continue;
                render.enabled = false;
               renders.Add(render);
            }
            _textRenders = renders.ToArray();
        }

        public void OnEnable()
        {
            if (_bus != null && !_subscribed)
            {
                _bus.OnTargetPartHit += OnPartHit;
                _subscribed = true;
            }
        }

        public void OnDisable()
        {
            if (_bus != null && _subscribed)
            {
                _bus.OnTargetPartHit -= OnPartHit;
                _subscribed = false;
            }
        }

        private void Update()
        {
            if (_movSpeed > 0 && _moveAmplitude > 0)
            {
                transform.localPosition = _movementLogic.Calculate(_startLocalPosition, MoveAmplitude, _movSpeed, _jumpHeight);
            }
        }

        public void SetPhysicsParams(float speed, float ampiltude)
        {
            _movSpeed = speed;
            _moveAmplitude = ampiltude;
        }

        private async void InitAddressables()
        {
            _bus = await Addressables.LoadAssetAsync<EventBus>("Task_Raycast_Second_EventBus").Task;
            if (this == null) return;

            if (!_subscribed)
            {
                _bus.OnTargetPartHit += OnPartHit;
                _subscribed = true;
            }

            TargetPart[] parts = GetComponentsInChildren<TargetPart>(true);
            foreach (var part in parts)
            {
                part.Initialize(_bus);
            }
        }

        private void OnPartHit(TargetType type, RaycastHit hitInfo)
        {
            if (!hitInfo.transform.IsChildOf(transform))
            {
                return;
            }

            DamagedFromWeapon(hitInfo);

            if (type == TargetType.Center)
            {
                Bullseye(hitInfo);
                ShowAllTextsForTimeAsync(_bullseyeDuration);
            }

        }

        private void DamagedFromWeapon(RaycastHit hitInfo)
        {
            for (int i = 0; i < _hitPrefabs.Length; i++)
            {
                GameObject obj = GameObject.Instantiate(_hitPrefabs[i], hitInfo.point, hitInfo.transform.rotation, _parent);
                GameObject.Destroy(obj, _hitDuration);
            }
        }

        private void Bullseye(RaycastHit hitInfo)
        {
            for (int i = 0; i < _bullseyePrefabs.Length; i++)
            {
                GameObject obj = GameObject.Instantiate(_bullseyePrefabs[i], hitInfo.point, Quaternion.identity, _parent);
                GameObject.Destroy(obj, _bullseyeDuration);
            }
        }

        private async void ShowAllTextsForTimeAsync(float durationSeconds)
        {
            foreach (var render in _textRenders)
            {
                if (render != null) render.enabled = true;
            }

            int delayMs = (int)(durationSeconds * 1000);

            await System.Threading.Tasks.Task.Delay(delayMs);

            if (this == null) return;

            foreach (var render in _textRenders)
            {
                if (render != null) render.enabled = false;
            }
        }
    }
}