using UnityEngine;

namespace Tasks.SORaycast
{
    public class GameInitializer : MonoBehaviour
    {
        private void Awake()
        {
            ItemManager manager = new ItemManager();
        }
    }
}