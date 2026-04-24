// Unity engine
using TMPro;
using UnityEngine;

namespace ClickerGame
{
    public class FloatingText : MonoBehaviour
    {
        public float moveSpeed = 50.00f;
        public float fadeSpeed = 2.00f;

        private CanvasGroup canvasGroup;
        private RectTransform rectTransform;
        public TextMeshProUGUI text;

        private void Start()
        {
            canvasGroup = GetComponent<CanvasGroup>();
            rectTransform = GetComponent<RectTransform>();
        }

        private void Update()
        {
            rectTransform.anchoredPosition += Vector2.up * moveSpeed * Time.deltaTime;
            canvasGroup.alpha -= fadeSpeed * Time.deltaTime;

            if (canvasGroup.alpha <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}