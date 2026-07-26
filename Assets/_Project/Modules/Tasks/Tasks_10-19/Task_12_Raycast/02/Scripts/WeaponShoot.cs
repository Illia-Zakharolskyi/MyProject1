using UnityEngine;
using UnityEngine.InputSystem;
using Common.Scripts.Extensions;
using System.Linq;
using UnityEngine.AddressableAssets;

namespace Task.Raycast.Second
{
    public class WeaponShoot : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Camera _mainCamera;
        [SerializeField] private Transform _placeToShot;
        [SerializeField] private GameObject[] _muzzleFlashPrefabs;
        [SerializeField] private LayerMask _shootMask;
        [SerializeField] private Transform _parent;

        [Header("Settings")]
        [SerializeField] private float _maxDistance = 100f;
        [SerializeField] private float _duration = 0.1f;

        private TaskRaycastSecondActions _actions;
        private EventBus _bus;

        private void Awake()
        {
            InitAddressables();
            _actions = new TaskRaycastSecondActions();
        }

        private void OnEnable()
        {
            _actions.Enable();
            _actions.Player.WeaponShoot.performed += Shoot;
        }

        private void OnDisable()
        {
            _actions.Disable();
            _actions.Player.WeaponShoot.performed -= Shoot;
        }

        private async void InitAddressables()
        {
            _bus = await Addressables.LoadAssetAsync<EventBus>("Task_Raycast_Second_EventBus").Task;
        }

        private void Shoot(InputAction.CallbackContext context)
        {
            Ray ray = _mainCamera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

            if (_muzzleFlashPrefabs != null && _placeToShot != null)
            {
                for (int i = 0; i < _muzzleFlashPrefabs.Length; i++)
                {
                    GameObject flash = Instantiate(_muzzleFlashPrefabs[i], _placeToShot.position, _placeToShot.rotation, _parent);
                    Destroy(flash, _duration);
                }
            }

            if (!Physics.Raycast(ray, out var hitInfo, _maxDistance, _shootMask)) return;

            if (_bus != null) _bus.InvokeTargetHit();

            if (hitInfo.collider.gameObject.TryGetComponent<IInteractable>(out var interactable))
            {
                interactable.Interact(hitInfo);
            }
        }
    }
}