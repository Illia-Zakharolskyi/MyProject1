using UnityEngine;

namespace Project.TowerDefense.Runtime
{
    public class TowerGhost : MonoBehaviour
    {
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Transform _rangeCircle;

        public void SetValid(bool isValid)
        {
            _spriteRenderer.color = isValid ? new Color(0, 1, 0, 0.5f) : new Color(1, 0, 0, 0.5f);
        }

        public void SetRange(float range)
        {
            if (_rangeCircle != null)
                _rangeCircle.localScale = new Vector3(range * 2, range * 2, 1);
        }
    }
}
