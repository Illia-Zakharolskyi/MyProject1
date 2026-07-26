using UnityEngine;

namespace Projects.ApplePicker
{
    public class Floor : MonoBehaviour
    {
        private void OnTriggerEnter2D(Collider2D collision)
        {
            SceneController.Instance.LoadScene("ApplePicker_GameOver");
        }
    }
}
