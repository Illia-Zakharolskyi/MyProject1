using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class MainMenuVisuals : MonoBehaviour
{
    [Header("Elements")]
    [SerializeField] private RectTransform[] apples;
    [SerializeField] private RectTransform leftLineRT;
    [SerializeField] private RectTransform rightLineRT;

    private void Start()
    {
        // Apple-animations (down-up)
        foreach (var apple in apples)
        {
            apple.DOAnchorPosY(-500f, 5f)
               .SetEase(Ease.Linear)
               .SetLoops(-1, LoopType.Yoyo)
               .SetLink(apple.gameObject);
        }

        // Left branding line animation (rotate 360)
        leftLineRT.DORotate(new Vector3(0, 0, -360), 8f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(leftLineRT.gameObject);

        // right branding line animation (rotate -360)
        rightLineRT.DORotate(new Vector3(0, 0, 360), 8f, RotateMode.FastBeyond360)
            .SetEase(Ease.Linear)
            .SetLoops(-1, LoopType.Incremental)
            .SetLink(rightLineRT.gameObject);
    }
}
