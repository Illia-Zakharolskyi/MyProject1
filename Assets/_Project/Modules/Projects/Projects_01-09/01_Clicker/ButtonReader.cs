// UnityEngine
using UnityEngine;
using UnityEngine.InputSystem;

namespace ClickerGme
{
    public class ButtonReader : MonoBehaviour
    {
        public void Update()
        {
            if (Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                Debug.Log("Escape key was pressed");
                Application.Quit();
            }
        }
    }
}