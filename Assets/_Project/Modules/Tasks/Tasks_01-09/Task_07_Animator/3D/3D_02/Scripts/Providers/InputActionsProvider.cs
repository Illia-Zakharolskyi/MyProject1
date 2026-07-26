// UnityEngine using directives
using UnityEngine;

// Task using directives
using Tasks.Learning.Animator.ThreeD.S2.Generated;

namespace Tasks.Learning.Animator.ThreeD.S2.Providers
{
    [CreateAssetMenu(fileName = "InputActionsProvider_SO", menuName = "SO/Tasks/Animator/3D/S2/Providers/InputActionsProvider")]
    public class InputActionsProvider : ScriptableObject
    {
        // Properties
        public InputActions InputActions
        {
            get
            {
                if (_inputActions == null)
                {
                    _inputActions = new InputActions();
                }
                return _inputActions;
            }
        }

        // Fields
        private InputActions _inputActions;

        // Lifecycle Methods
        private void OnDisable()
        {
           #if UNITY_EDITOR
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
           #endif

            if (_inputActions != null)
            {
                _inputActions.Dispose();
                _inputActions = null;
            }
        }
    }
}