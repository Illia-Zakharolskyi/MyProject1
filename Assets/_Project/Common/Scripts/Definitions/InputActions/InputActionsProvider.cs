// UnityEngine using directives
using UnityEngine;

namespace Common.Scripts.Definitions.InputActions
{
    [CreateAssetMenu(fileName = "InputActionsProvider", menuName = "Scriptable Objects/Providers/InputActionsProvider")]
    public class InputActionsProvider : ScriptableObject
    {
        // Properties
        public BaseControls BaseControls
        {
            get
            {
                if (_baseControls == null)
                {
                    _baseControls = new BaseControls();
                }
                return _baseControls;
            }
        }

        // Fields
        private BaseControls _baseControls;

        // Lifecycle Methods
        private void OnDisable()
        {
           #if UNITY_EDITOR 
            if (!UnityEditor.EditorApplication.isPlaying)
                return;
           #endif

            if (_baseControls != null)
            {
                _baseControls.Dispose();
                _baseControls = null;
            }
        }
    }
}