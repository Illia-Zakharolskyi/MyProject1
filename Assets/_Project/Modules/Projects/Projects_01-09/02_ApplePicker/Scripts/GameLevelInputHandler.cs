using UnityEngine;
using UnityEngine.InputSystem;

namespace Projects.ApplePicker
{
    public class GameLevelInputHandler : MonoBehaviour
    {
        private ApplePickerInputActions.UIActions _actions;

        private void Awake()
        {
            _actions = new ApplePickerInputActions().UI;
        }

        private void OnEnable()
        {
            _actions.Enable();
            _actions.Pause.performed += OnPausePressed;
        }
        private void OnDisable()
        {
            _actions.Disable();
            _actions.Pause.performed -= OnPausePressed;
        }

        private void OnPausePressed(InputAction.CallbackContext context)
        {
            PauseManager.Instance.TogglePause();
        }
    }
}
