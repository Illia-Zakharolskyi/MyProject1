using UnityEngine;
using DG.Tweening;
using UnityEditor;

namespace Projects.ApplePicker
{
    public class TreeController : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private RectTransform treeRect;

        [Header("Movement Settings")]
        [SerializeField] private float moveDistance = 800f;
        [SerializeField] private float moveTime = 1.5f;
        [SerializeField] private float randomTurnChance = 0.05f;

        [Header("Apple Settings")]
        [SerializeField] private GameObject[] applePrefab;
        [SerializeField] private RectTransform appleParent;
        [SerializeField] private float minAppleCooldown = 0.7f;
        [SerializeField] private float maxAppleCooldown = 1.1f;

        private bool movingRight = false;
        private bool canDropApple = true;

        private void Start()
        {
            Move();
        }

        private void Update()
        {
            if (UnityEngine.Random.value < randomTurnChance * Time.deltaTime)
            {
                movingRight = !movingRight;
                RestartTreeMovement();
            }

            if (canDropApple) 
                StartCoroutine(DropApple());
        }

        private void Move()
        {
            float targetX = movingRight ? moveDistance : -moveDistance;

            treeRect.DOAnchorPos(new Vector2(targetX, treeRect.anchoredPosition.y), moveTime)
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    movingRight = !movingRight;
                    Move();
                })
                .SetLink(treeRect.gameObject);
        }

        private void RestartTreeMovement()
        {
            DOTween.Kill(treeRect);
            Move();
        }

        private System.Collections.IEnumerator DropApple()
        {
            canDropApple = false;

            GameObject apple = Instantiate(
                applePrefab[Random.Range(0, applePrefab.Length)],
                appleParent
            );

            RectTransform appleRect = apple.GetComponent<RectTransform>();

            appleRect.anchoredPosition = new Vector2(
                treeRect.anchoredPosition.x,
                treeRect.anchoredPosition.y - 100f
            );

            yield return new WaitForSeconds(Random.Range(minAppleCooldown, maxAppleCooldown));
            canDropApple = true;
        }
    }
}