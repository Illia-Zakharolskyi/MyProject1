using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using System.Threading.Tasks;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace Tasks.SORaycast
{
    [CreateAssetMenu(fileName = "Item_Data_Base", menuName = "SO/Tasks/SORaycast/Item_Data_Base")]
    public class ItemDataBase : ScriptableObject
    {
        public List<string> ItemNames;
        public Dictionary<string, ItemDetails> ItemDictionary;

        private Dictionary<string, AsyncOperationHandle<Sprite>> _loadingHandles = new Dictionary<string, AsyncOperationHandle<Sprite>>();

        public void Initialize()
        {
            if (ItemDictionary != null) return;

            ItemNames = new List<string> { "Яблуко", "Меч", "Хліб" };
            ItemDictionary = new Dictionary<string, ItemDetails>
            {
                { "Яблуко", new ItemDetails("Яблуко", "Task_SORaycast_Apple_Icon", "хрумка спокуса") },
                { "Меч", new ItemDetails("Меч", "Task_SORaycast_Sword_Icon", "грізна зброя") },
                { "Хліб", new ItemDetails("Хліб", "Task_SORaycast_Bread_Icon", "всьому голова") }
            };
        }

        public async Task<Sprite> LoadSprite(string address)
        {
            if (_loadingHandles.TryGetValue(address, out var existingHandle) && existingHandle.IsValid())
            {
                return existingHandle.Result;
            }

            var handle = Addressables.LoadAssetAsync<Sprite>(address);
            _loadingHandles[address] = handle;

            return await handle.Task;
        }

        public void ClearAllCache()
        {
            foreach (var handle in _loadingHandles.Values)
            {
                if (handle.IsValid()) Addressables.Release(handle);
            }
            _loadingHandles.Clear();
        }
    }
}