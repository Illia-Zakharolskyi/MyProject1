using UnityEngine;

namespace Projects.ApplePicker
{
    public class PlayerBasket : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private GameLevel level;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            GameObject.Destroy(collision.gameObject);
            level.UpdateScore();
        }
    }
}
