using UnityEngine;
using UnityEngine.AddressableAssets;
using Common.Scripts.Extensions;

namespace Task.Raycast
{
    public class PlayerRaycastHandler : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private LayerMask interactMask;

        private PlayerCameraEvents _events;

        private void Awake()
        {
            InitAddressables();
        }

        private void OnDestroy()
        {
            _events.OnRaycastHit -= OnRaycastHit;
        }

        private async void InitAddressables()
        {
            _events = await Addressables.LoadAssetAsync<PlayerCameraEvents>("Player_Camera_Events").Task;
            _events.OnRaycastHit += OnRaycastHit;
        }

        private void OnRaycastHit(RaycastHit hitInfo)
        {
            if (!interactMask.Contains(hitInfo.collider.transform.gameObject.layer))
            {
                return;
            }

            Debug.Log($"Active interactable: {hitInfo.collider.transform.gameObject.name}");
        }
    }
}
