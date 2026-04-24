using UnityEngine;
using UnityEngine.InputSystem;

namespace Projects.ApplePicker
{
    public class PlayerBasketMovement : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform rectTransform;
        [SerializeField] private RectTransform canvasTransform;

        private ApplePickerInputActions.UIActions _uiActions;

        private float _canvasHalfWidth;
        private float _basketHalfWidth;

        private void Awake()
        {
            _uiActions = new ApplePickerInputActions().UI;

            _canvasHalfWidth = canvasTransform.rect.width * 0.5f;
            _basketHalfWidth = rectTransform.rect.width * 0.5f;
        }

        private void OnEnable()
        {
            _uiActions.Enable();
            _uiActions.MousePosition.performed += OnMovement;
        }

        private void OnDisable()
        {
            _uiActions.Disable();
            _uiActions.MousePosition.performed -= OnMovement;
        }

        private void OnMovement(InputAction.CallbackContext context)
        {
            Vector2 rawValue = context.ReadValue<Vector2>();

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                canvasTransform,
                rawValue,
                null,
                out Vector2 localValue
            );

            float clampedX = Mathf.Clamp(
                localValue.x,
                -_canvasHalfWidth + _basketHalfWidth,
                _canvasHalfWidth - _basketHalfWidth
            );

            rectTransform.anchoredPosition = new Vector2(clampedX, rectTransform.anchoredPosition.y);
        }
    }
}