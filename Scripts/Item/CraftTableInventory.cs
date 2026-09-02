using UnityEngine;

namespace Wilson.Item
{
    public class CraftTableInventory : MonoBehaviour
    {
        private int storageSize = 6;
        private ItemData[] storageItems;

        public ItemData[] StorageItems => storageItems;

        private void Awake()
        {
            storageItems = new ItemData[storageSize];
        }

        public bool AddItem(ItemData item)
        {
            for (int i = 0; i < storageItems.Length; i++)
            {
                if (storageItems[i] == null)
                {
                    storageItems[i] = item;
                    return true;
                }
            }
            return false;
        }

        public bool RemoveItem(ItemData item)
        {
            for (int i = 0; i < storageItems.Length; i++)
            {
                if (storageItems[i] == item)
                {
                    storageItems[i] = null;
                    return true;
                }
            }
            return false;
        }
    }
}
