using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine.AddressableAssets;

namespace Tasks.SORaycast
{
    public class ItemManager
    {
        public static ItemManager Instance;
        private List<ItemDetails> _currentItems;
        private EventBus _bus;

        public ItemManager()
        { 
            Instance = this;

            _currentItems = new();

            InitAddressablesAsync();
        }

        private async void InitAddressablesAsync()
        {
            _bus = await Addressables.LoadAssetAsync<EventBus>("Task_SORaycast_Event_Bus").Task;
        }

        public async void AddItem(ItemDetails itemDetails)
        {
            _currentItems.Add(itemDetails);

            if (_currentItems.Count == 3)
            {
                HashSet<string> sprites = new();

                foreach (var item in _currentItems)
                {
                    sprites.Add(item.ItemSpriteReference);
                }

                if (sprites.Count == 1)
                {
                    while (_bus == null)
                    {
                        await Task.Delay(50);
                    }

                    _bus.InvokeJackpot();
                }

                _currentItems.Clear();
            }
        }
    }
}