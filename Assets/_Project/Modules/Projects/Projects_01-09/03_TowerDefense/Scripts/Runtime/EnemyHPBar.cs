using UnityEngine;

public class EnemyHPBar : MonoBehaviour
{
    public Transform enemyTransform; // Сюди передаємо трансформ ворога в 2D
    public Vector3 offset = new Vector3(0, 1.2f, 0); // На скільки юнітів підняти смужку над ворогом

    private RectTransform rectTransform;
    private Camera mainCamera;

    void Start()
    {
        rectTransform = GetComponent<RectTransform>();
        mainCamera = Camera.main; // Кешуємо камеру для кращої продуктивності
    }

    // Використовуємо LateUpdate, щоб смужка рухалася СЛІДОМ за ворогом і камерою,
    // без мікро-посіпувань (jittering)
    void LateUpdate()
    {
        // Якщо ворога знищено — видаляємо і смужку ХП
        if (enemyTransform == null)
        {
            Destroy(gameObject);
            return;
        }

        // Переводимо 2D позицію з ігрового світу на екран (в пікселі Canvas)
        Vector2 screenPosition = mainCamera.WorldToScreenPoint(enemyTransform.position + offset);

        // Встановлюємо позицію UI елемента
        rectTransform.position = screenPosition;
    }
}
