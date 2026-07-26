using ParticleSystem.Generated;
using UnityEngine;
using Common.Scripts.Systems.Player;

namespace Tasks.ParticleSystem
{
    public class BenchController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameObject player;
        [SerializeField] private GameObject interactionHint;
        [SerializeField] private Transform sitPoint;

        [Header("Settings")]
        [SerializeField] private LayerMask targetMask;

        private bool _isSitting = false;
        private bool _isPlayerInTrigger = false;
        private ParticleSystemInputActions.PlayerActions playerActions;
        private ParticleSystemInputActions actions;
        private PlayerController playerMov;
        private Rigidbody playerBody;
        private GameObject playerModel;
        private GameObject playerCam;

        private Vector3 startPlayerPos;
        private Quaternion startPlayerRot;

        private void Awake()
        {
            actions = new ParticleSystemInputActions();
            playerActions = actions.Player;
        }
        private void Start()
        {
            if (!player.TryGetComponent<PlayerController>(out playerMov))
            {
                Debug.LogError($"Component PlayerController is missing on {player.name}", this);
            }
            if (!player.TryGetComponent<Rigidbody>(out playerBody))
            {
                Debug.LogError($"Component Rigidbody is missing on {player.name}", this);
            }

            FindPlayerComponents();
        }
        private void Update()
        {
            if (_isPlayerInTrigger && playerActions.ObjectInteraction.WasPressedThisFrame())
            {
                if (!_isSitting) Interact();
                else StandUp();
            }
        }

        private void OnEnable() => playerActions.Enable();
        private void OnDisable() => playerActions.Disable();

        private void OnTriggerEnter(Collider other)
        {
            if (!IsInLayerMask(other.gameObject)) return;

            _isPlayerInTrigger = true;
            interactionHint.SetActive(!_isSitting);

            if (!_isSitting && player != null)
            {
                startPlayerPos = player.transform.position;
                startPlayerRot = player.transform.rotation;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (!IsInLayerMask(other.gameObject)) return;

            _isPlayerInTrigger = false;
            interactionHint.SetActive(false);
        }

        private void Interact()
        {
            if (!ValidateComponents()) return;

            playerBody.useGravity = false;
            _isSitting = true;
            playerMov.enabled = false;
            playerModel.SetActive(false);

            if (sitPoint != null)
            {
                player.transform.position = sitPoint.position;
                player.transform.rotation = sitPoint.rotation;
            }
        }

        private void StandUp()
        {
            if (!ValidateComponents()) return;

            playerBody.useGravity = true;
            _isSitting = false;
            playerModel.SetActive(true);
            playerMov.enabled = true;

            player.transform.position = startPlayerPos;
            player.transform.rotation = startPlayerRot;
        }

        private bool IsInLayerMask(GameObject obj)
        {
            return ((1 << obj.layer) & targetMask) != 0;
        }

        private void FindPlayerComponents()
        {
            Transform childTrans = player.transform.Find("PlayerModel");
            if (childTrans)
            {
                playerModel = childTrans.gameObject;
            }

            Transform child2Trans = player.transform.Find("Main Camera");
            if (child2Trans)
            {
                playerCam = child2Trans.gameObject;
            }

            if (playerModel == null || playerCam == null)
            {
                Debug.LogWarning($"BenchController: Can't find Player Model or camera in that {player.name}!", this);
            }
        }

        private bool ValidateComponents()
        {
            return playerModel != null && playerCam != null && playerMov != null && playerBody != null;
        }
    }
}
