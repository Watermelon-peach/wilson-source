using UnityEngine;
using Wilson.UI;
using Wilson.Utility;

namespace Wilson.Item
{
    public class Inventory : Singleton<Inventory>
    {
        [SerializeField] private int maxSlotCount = 5;
        private ItemData[] items;
        public ItemData[] Items => items;

        protected override void Awake()
        {
            base.Awake(); // 꼭 호출해줘야 싱글톤 초기화됨
            items = new ItemData[maxSlotCount];
        }

        public bool AddItem(ItemData newItem)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == null)
                {
                    items[i] = newItem;
                    InventoryUI.Instance.RefreshUI();
                    return true;
                }
            }

            StartCoroutine(AlertBarUI.Instance.ShowAlert("인벤토리가 가득 찼습니다!!"));
            return false;
        }

        public bool RemoveItem(ItemData item)
        {
            for (int i = 0; i < items.Length; i++)
            {
                if (items[i] == item)
                {
                    items[i] = null;
                    InventoryUI.Instance.RefreshUI();
                    return true;
                }
            }

            return false;
        }
    }
    
}
