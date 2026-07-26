using Project.TowerDefense.Runtime.Definitions;
using Project.TowerDefense.Runtime.UI;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Project.TowerDefense.Runtime
{
    [System.Serializable]
    public struct TowerUIBinding
    {
        public TowerData data;
        public TMP_Text nameText;
        public TMP_Text priceText;
    }

    public class TowerBuildingMenu : MonoBehaviour
    {
        [SerializeField] private TowerUIBinding[] _bindings;

        void Awake()
        {
            foreach (var binding in _bindings)
            {
                binding.priceText.text = NumberFormatter.ReturnFormatted(binding.data.cost, 0);
                binding.nameText.text = binding.data.towerName;
            }
        }
    }
}
