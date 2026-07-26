using System;

namespace Tasks.SORaycast
{
    [Serializable]
    public struct ItemDetails
    {
        public string ItemName;
        public string ItemSpriteReference;
        public string ItemDescription;

        public ItemDetails(string name, string spriteReference, string description)
        {
            ItemName = name;
            ItemSpriteReference = spriteReference;
            ItemDescription = description;
        }
    }
}