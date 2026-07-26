using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Tasks.SORaycast
{
    public class ClickAllService : MonoBehaviour, IPointerClickHandler
    {
        private List<SelectableItem> _components = new();
        private void Start()
        {
            Transform root = transform.root;

            var items = root.GetComponentsInChildren<SelectableItem>(true);
            _components.AddRange(items);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            foreach (var script in _components)
            {
                script.OnPointerClick(eventData);
            }
        }
    }
}