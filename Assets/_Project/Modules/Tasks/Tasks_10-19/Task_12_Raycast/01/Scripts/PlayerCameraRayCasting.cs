using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Task.Raycast
{
    public class PlayerCameraRayCasting : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Camera _camera;

        [Header("Settings")]
        [SerializeField] private float _distance;

        private PlayerCameraEvents _events;

        private void Awake()
        {
            InitAddressables();
        }

        private void Update()
        {
            Ray ray = _camera.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.5f));

            Debug.DrawRay(ray.origin, ray.direction * _distance, Color.yellow);

            if (Physics.Raycast(ray, out var hit, _distance))
            {
                if (_events != null) _events.InvokeRaycastHit(hit);
            }
        }

        private async void InitAddressables()
        {
            _events = await Addressables.LoadAssetAsync<PlayerCameraEvents>("Player_Camera_Events").Task;
        }
    }
}
