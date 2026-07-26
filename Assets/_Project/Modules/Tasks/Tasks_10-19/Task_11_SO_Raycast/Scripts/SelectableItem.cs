using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Tasks.SORaycast
{
    public class SelectableItem : MonoBehaviour, IPointerClickHandler
    {
        [Header("Refs")]
        [SerializeField] private ItemDataBase dataBase;

        private TMP_Text _itemNameElement;
        private Image _itemImage;
        private TMP_Text _itemDescElement;

        private void Start()
        {
            dataBase.Initialize();

            Transform parent = transform.parent;

            _itemNameElement = parent.Find("Name").GetComponent<TMP_Text>();
            _itemImage = transform.GetComponent<Image>();
            _itemDescElement = parent.Find("Desc").GetComponent<TMP_Text>();
        }

        public async void OnPointerClick(PointerEventData eventData)
        {
            string item = dataBase.ItemNames[Random.Range(0, dataBase.ItemNames.Count)];
            ItemDetails itemDetails = dataBase.ItemDictionary[item];

            ItemManager.Instance.AddItem(itemDetails);

            Sprite itemSprite = await dataBase.LoadSprite(itemDetails.ItemSpriteReference);

            _itemNameElement.text = itemDetails.ItemName;
            _itemImage.sprite = itemSprite;
            _itemDescElement.text = itemDetails.ItemDescription;
        }
    }
}