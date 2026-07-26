using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

namespace Task.CameraLight
{
    public class LightOrb : MonoBehaviour
    {
        [Header("Refs")]
        [SerializeField] private Material orbMaterial;
        [SerializeField] private Light orbLight;
        [SerializeField] private Transform playerTransform;
        [SerializeField] private Volume globVol;

        [Header("Settings")]
        [SerializeField] private float maxDistance = 10.00f;
        [SerializeField] private float maxIntensity = 5.00f;
        [SerializeField] private float pulseSpeed = 5.00f;
        

        private bool allOkay = true;
        private Transform myTransform;
        private Bloom bloom;

        private void Awake()
        {
            myTransform = this.gameObject.transform;
        }

        private void OnEnable()
        {
            orbMaterial.color = Color.yellow;
            orbLight.color = Color.yellow;
        }

        private void Start()
        {
            if (orbMaterial == null || orbMaterial == null)
            {
                Debug.Log($"Smth is missing on {this.name}. Check it properly.");
                allOkay = false;
                return;
            }
            if (globVol != null && globVol.profile.TryGet<Bloom>(out var blooms))
            {
                bloom = blooms;
                bloom.intensity.overrideState = true;
            }
        }

        private void Update()
        {
            ColorCheck();
            DistanceCheck();
        }

        private void ColorCheck()
        {
            if (!allOkay) return;


            if (Keyboard.current.digit1Key.wasPressedThisFrame)
            {
                orbMaterial.color = Color.yellow;
                orbLight.color = Color.yellow;
            }
            else if (Keyboard.current.digit2Key.wasPressedThisFrame)
            {
                orbMaterial.color = Color.blue;
                orbLight.color = Color.blue;
            }
            else if (Keyboard.current.digit3Key.wasPressedThisFrame)
            {
                orbMaterial.color = Color.red;
                orbLight.color = Color.red;
            }
        }

        private void DistanceCheck()
        {
            float distance = Vector3.Distance(playerTransform.position, myTransform.position);
            float ratio = 1f - (distance / maxDistance);
            ratio = Mathf.Clamp01(ratio);

            if (ratio > 0.9f)
            {
                float pulse = (Mathf.Sin(Time.time * pulseSpeed) + 1f) / 2f;
                // К базовой интенсивности добавляем "бонус" от пульсации
                bloom.intensity.value = (ratio * maxIntensity) + (pulse * 2f);
                orbLight.intensity = (ratio * maxIntensity) + (pulse * 2f);
            }
            else
            {
                bloom.intensity.value = ratio * maxIntensity;
                orbLight.intensity = ratio * maxIntensity;
            }
        }
    }
}